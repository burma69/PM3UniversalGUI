using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;

namespace PM3UniversalGUI
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            splitContainer2.Parent = splitContainer1.Panel2;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer1.SplitterDistance = splitContainer1.Width * 30 / 100;

            btnReloadCommands_Click(this, null);
        }



        public void OnDataReceivedEventHandler(object sender, DataReceivedEventArgs e)
        {
            int i = e.Data.IndexOf("proxmark3>");
            if (i < 0) i = e.Data.IndexOf("pm3 -->");

            if (i < 0)
                AppendText(ConsoleTextBox, ConsoleTextBox.ForeColor, e.Data + "\r\n");
            else AppendText(ConsoleTextBox, Color.Goldenrod, e.Data + "\r\n", true);
        }


        private void COMPortBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.PM3.InitClient(COMPortBox.SelectedItem.ToString());
            Program.PM3.ClientProcess.OutputDataReceived += OnDataReceivedEventHandler;
            Program.PM3.StartClient();

            defaultToolTip.SetToolTip(btnRun, null);
            
        }

        private void COMPortBox_DropDown(object sender, EventArgs e)
        {
            COMPortBox.Items.Clear();
            COMPortBox.Items.AddRange(SerialPort.GetPortNames());

        }

        private void MainForm_Load(object sender, EventArgs e)
        {


        }

        private void AppendText(RichTextBox TargetTextBox, Color TextColor, string msg, bool Bold = false, bool Italic = false)
        {
            TargetTextBox.Invoke(new EventHandler(delegate
            {
                TargetTextBox.SelectedText = string.Empty;
                //ConsoleTextBox.SelectionFont = new Font(_displaywindow.SelectionFont, FontStyle.Bold);
                TargetTextBox.SelectionColor = TextColor;
                FontStyle fs = new FontStyle();
                if (Bold) fs = fs | FontStyle.Bold;
                if (Italic) fs = fs | FontStyle.Italic;
                TargetTextBox.SelectionFont = new Font(TargetTextBox.Font, fs);
                TargetTextBox.AppendText(msg);
                TargetTextBox.ScrollToCaret();
            }));
        }

        private void btnReloadCommands_Click(object sender, EventArgs e)
        {
            Program.PM3.LoadCommands();

            PM3CommandsTree.Nodes.Clear();
            PM3CommandsTree.Nodes.Add("PM3");

            for (int cIndex = 0; cIndex < Program.PM3.Commands.Count; cIndex++)
            {
                PM3Command c = Program.PM3.Commands[cIndex];
                string[] path = c.Command.Split(' ');
                TreeNode n = PM3CommandsTree.Nodes[0];

                for (int i = 0; i < path.Count(); i++)
                {
                    if (n.Nodes.ContainsKey(path[i])) n = n.Nodes[path[i]];
                    else
                    {
                        TreeNode t = new TreeNode(path[i]);
                        t.Tag = cIndex;
                        t.Name = t.Text;
                        t.Text += " (" + c.Description + ")";
                        if (i == path.Count() - 1)
                            t.ToolTipText = c.Description;
                        n.Nodes.Add(t);
                        n = t;
                    }
                }
            }

            PM3CommandsTree.Nodes[0].Expand();
        }

        private void PM3CommandsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e == null || e.Node.Tag == null) return;
            int ItemIndex = (int)e.Node.Tag; //156
            if (ItemIndex < 0 || ItemIndex >= Program.PM3.Commands.Count) return;
            PM3Command cmd = Program.PM3.Commands[ItemIndex];

            commandComboBox.Text = cmd.Command;

            if (cmd.DescriptionFull == null) Program.PM3.ExtractCommandParams(cmd);

            CommandParamsContainer.Controls.Clear();

            CommandDescriptionTextBox.Text = "";
            if (cmd.DescriptionFull != null) AppendText(CommandDescriptionTextBox, Color.FromArgb(255, 224, 224, 224), cmd.DescriptionFull + "\r\n", false, true);
            if (cmd.Usage != null) AppendText(CommandDescriptionTextBox, Color.FromArgb(255, 192, 192, 255), cmd.Usage + "\r\n", true);
            if (cmd.Options != null) AppendText(CommandDescriptionTextBox, Color.FromArgb(255, 128, 128, 255), cmd.Options + "\r\n");
            if (cmd.Examples != null) AppendText(CommandDescriptionTextBox, Color.FromArgb(255, 255, 128, 0), cmd.Examples + "\r\n", false, true);
            CommandDescriptionTextBox.SelectionStart = 0;
            CommandDescriptionTextBox.SelectionLength = 0;
            CommandDescriptionTextBox.ScrollToCaret();

            Control PreviousControl = null;
            for (int i = 0; i < cmd.Params.Count; i++)
            {
                PM3CommandParam p = cmd.Params[i];

                Control Container = CommandParamsContainer; //by default adding new controls directly to the container. Otherwise - inside the checkbox or radio button
                Control c = null;

                //if the option should be placed into a group box
                if (p.OrWithNext || (i > 0 && (cmd.Params[i - 1].OrWithNext))) 
                {
                    GroupBox RadioContainer = null;
                    int cTop = 0;
                    //this is the first option in a group -> create a groupbox container
                    if (i == 0 || (i > 0 && !(cmd.Params[i - 1].OrWithNext)))
                    {
                        RadioContainer = new System.Windows.Forms.GroupBox();
                        CommandParamsContainer.Controls.Add(RadioContainer);
                        if (p.IsOptional)
                        {
                            CheckBox EnableGroup = new System.Windows.Forms.CheckBox();
                            EnableGroup.Width = EnableGroup.Height;
                            EnableGroup.CheckedChanged += textBox1_TextChanged;
                            RadioContainer.Controls.Add(EnableGroup);
                        }
                    }
                    else //continue adding options to the group box created before
                    {
                        Control PreviousSelector = PreviousControl;
                        if (PreviousSelector.GetType() != typeof(RadioButton) && PreviousSelector.GetType() != typeof(CheckBox)) PreviousSelector = PreviousControl.Parent;

                        Control PreviousGroupBox = PreviousSelector.Parent;
                        if (PreviousGroupBox.GetType() != typeof(GroupBox)) PreviousGroupBox = PreviousGroupBox.Parent;

                        RadioContainer = (GroupBox)PreviousGroupBox;
                        cTop = PreviousSelector.Top + PreviousSelector.Height;
                    }

                    Container = new System.Windows.Forms.RadioButton();
                    Container.Top = cTop; // + Container.Height / 4;
                    if (cTop == 0) Container.Top += Container.Height;
                    Container.Width = RadioContainer.Width * 9 / 10;
                    Container.Left = RadioContainer.Width * 1 / 20;
                    RadioContainer.Controls.Add(Container);
                    RadioContainer.Height = Container.Top + Container.Height + Container.Height / 4;
                }



                if (p.IsOptional)
                {
                    if ((Container.GetType() == typeof(GroupBox))
                        || (Container.GetType() == typeof(FlowLayoutPanel))) 
                    {
                        Control CheckBoxContainer = new System.Windows.Forms.CheckBox();
                        Container.Controls.Add(CheckBoxContainer);
                        Container = CheckBoxContainer;
                    }
                }

                if ((p.ParamType == PM3CommandParam.EParamType.Fixed) &&
                    ((Container.GetType() == typeof(GroupBox))
                      || (Container.GetType() == typeof(FlowLayoutPanel)))
                    )
                {
                    c = new System.Windows.Forms.Label();
                    ((Label)c).TextAlign = ContentAlignment.MiddleLeft;
                    ((Label)c).AutoEllipsis = true;
                    Container.Controls.Add(c);
                }
                else
                if (p.AllowedValues.Count > 0)
                {
                    c = new System.Windows.Forms.ComboBox();
                    c.Enter += textBox1_Enter;                    
                    Container.Controls.Add(c);
                }
                else
                if ((p.ParamType == PM3CommandParam.EParamType.Value) || (p.Description == null))
                {
                    c = new System.Windows.Forms.TextBox();
                    c.Enter += textBox1_Enter;
                    Container.Controls.Add(c);
                }
                else
                if ((Container.GetType() != typeof(GroupBox))
                    && (Container.GetType() != typeof(FlowLayoutPanel)))
                {
                    c = Container;
                    c.Enter += textBox1_Enter;
                }

                if (Container.GetType() == typeof(RadioButton) || Container.GetType() == typeof(CheckBox))
                    if (c != Container)
                    {
                        c.Left += c.Height;
                        c.Width = Container.Width - c.Left;

                        Container.Enter += textBox1_Enter;
                        if (Container.GetType() == typeof(RadioButton))                       
                            ((RadioButton)Container).CheckedChanged += textBox1_TextChanged;
                        if (Container.GetType() == typeof(CheckBox))
                            ((CheckBox)Container).CheckedChanged += textBox1_TextChanged;
                    }

                c.Text = p.Name;
                if (p.Description != null) c.Text += " (" + p.Description + ")";
                defaultToolTip.SetToolTip(c, c.Text);
                c.Tag = i;
                Container.Tag = i;

                foreach (PM3CommandParamAllowedValue av in p.AllowedValues)
                {
                    if (c.GetType() == typeof(ComboBox))
                    ((ComboBox)c).Items.Add(av);
                }

                PreviousControl = c;

                if (c.GetType() == typeof(RadioButton))
                    ((RadioButton)c).CheckedChanged += textBox1_TextChanged;
                if (c.GetType() == typeof(CheckBox))
                    ((CheckBox)c).CheckedChanged += textBox1_TextChanged;
                if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(ComboBox))
                    c.TextChanged += textBox1_TextChanged;

            }

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!Program.PM3.IsRunning())
            {
                COMPortBox.Focus();
                return;
            }

            Program.PM3.ClientProcess.StandardInput.WriteLine(commandComboBox.Text);
            if (commandComboBox.Items.IndexOf(commandComboBox.Text) < 0)
                commandComboBox.Items.Add(commandComboBox.Text);
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void commandComboBox_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SelectLineage(Control c)
        {
            if (c.GetType() == typeof(FlowLayoutPanel)) return;

            if (c.GetType() == typeof(TextBox))
            {
                if (((TextBox)c).SelectionLength == 0)
                    ((TextBox)c).SelectAll();
            }

            if (c.Parent.GetType() == typeof(CheckBox))
            {
                ((CheckBox)(c.Parent)).Checked = true;
            }

            if (c.Parent.GetType() == typeof(RadioButton))
            {
                ((RadioButton)(c.Parent)).Checked = true;
            }

            if (c.GetType() == typeof(GroupBox))
            {
                Control c2 = c.Controls[0];
                if (c2.GetType() == typeof(CheckBox))
                {
                    if (c2.Top == 0)
                        ((CheckBox)c2).Checked = true;
                }
            }

            SelectLineage(c.Parent);
        }

        private string GenerateCommand(Control c, PM3Command cmd)
        {
            string Result = "";

            foreach (Control child in c.Controls)
            {
                if (child.GetType() == typeof(GroupBox))
                {
                    Control c2 = child.Controls[0];
                    if (c2.GetType() == typeof(CheckBox))
                    {
                        if (c2.Top == 0)
                            if (!((CheckBox)c2).Checked) continue;
                    }
                }

                if (child.GetType() == typeof(CheckBox))
                    if (!((CheckBox)child).Checked) continue;

                if (child.GetType() == typeof(RadioButton))
                    if (!((RadioButton)child).Checked) continue;

                Result += GenerateCommand(child, cmd);
            }

            if (c.Controls.Count > 0) return Result;

            if (c.GetType() == typeof(CheckBox))
                if (!((CheckBox)c).Checked) return Result;

            if (c.GetType() == typeof(RadioButton))
                if (!((RadioButton)c).Checked) return Result;

            if (c.Tag != null)
            {
                PM3CommandParam p = cmd.Params[(int)c.Tag];

                if (c.GetType() == typeof(TextBox))
                {
                    Result += " " + c.Text;
                } else
                if (c.GetType() == typeof(ComboBox))
                {
                    if (((ComboBox)c).SelectedItem != null)
                    Result += " " + ((PM3CommandParamAllowedValue)((ComboBox)c).SelectedItem).Value;
                }
                else
                {
                    Result += " " + p.Name;
                }
            }
            return Result;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            Control c = (Control)sender;

            c.Click -= textBox1_Click;

            SelectLineage(c);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Control c = (Control)sender;
                  
            c.Click += textBox1_Click;

            SelectLineage(c);

            textBox1_TextChanged(sender, e);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (PM3CommandsTree.SelectedNode == null || PM3CommandsTree.SelectedNode.Tag == null) return;
            int ItemIndex = (int)PM3CommandsTree.SelectedNode.Tag; //156
            if (ItemIndex < 0 || ItemIndex >= Program.PM3.Commands.Count) return;
            PM3Command cmd = Program.PM3.Commands[ItemIndex];

            commandComboBox.Text = cmd.Command;

            commandComboBox.Text += GenerateCommand(CommandParamsContainer, cmd);
        }

        private void commandComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) btnRun_Click(sender, e);
        }
    }
}
