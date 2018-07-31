namespace PM3UniversalGUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.defaultToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnRun = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnReloadCommands = new System.Windows.Forms.Button();
            this.PM3CommandsTree = new System.Windows.Forms.TreeView();
            this.COMPortBox = new System.Windows.Forms.ComboBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.CommandDescriptionTextBox = new System.Windows.Forms.RichTextBox();
            this.CommandParamsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.commandComboBox = new System.Windows.Forms.ComboBox();
            this.ConsoleTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.CommandParamsContainer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(738, 2);
            this.btnRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 28);
            this.btnRun.TabIndex = 17;
            this.btnRun.Text = "Run";
            this.defaultToolTip.SetToolTip(this.btnRun, "No COM Port selected");
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(24, 10);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnReloadCommands);
            this.splitContainer1.Panel1.Controls.Add(this.PM3CommandsTree);
            this.splitContainer1.Panel1.Controls.Add(this.COMPortBox);
            this.splitContainer1.Size = new System.Drawing.Size(340, 584);
            this.splitContainer1.SplitterDistance = 307;
            this.splitContainer1.TabIndex = 11;
            // 
            // btnReloadCommands
            // 
            this.btnReloadCommands.Location = new System.Drawing.Point(135, 311);
            this.btnReloadCommands.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnReloadCommands.Name = "btnReloadCommands";
            this.btnReloadCommands.Size = new System.Drawing.Size(130, 29);
            this.btnReloadCommands.TabIndex = 11;
            this.btnReloadCommands.Text = "Reload Commands";
            this.btnReloadCommands.UseVisualStyleBackColor = true;
            this.btnReloadCommands.Visible = false;
            this.btnReloadCommands.Click += new System.EventHandler(this.btnReloadCommands_Click);
            // 
            // PM3CommandsTree
            // 
            this.PM3CommandsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PM3CommandsTree.HideSelection = false;
            this.PM3CommandsTree.Location = new System.Drawing.Point(10, 34);
            this.PM3CommandsTree.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PM3CommandsTree.Name = "PM3CommandsTree";
            this.PM3CommandsTree.ShowNodeToolTips = true;
            this.PM3CommandsTree.Size = new System.Drawing.Size(286, 539);
            this.PM3CommandsTree.TabIndex = 4;
            this.PM3CommandsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PM3CommandsTree_AfterSelect);
            this.PM3CommandsTree.DoubleClick += new System.EventHandler(this.btnRun_Click);
            // 
            // COMPortBox
            // 
            this.COMPortBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.COMPortBox.FormattingEnabled = true;
            this.COMPortBox.Location = new System.Drawing.Point(10, 2);
            this.COMPortBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.COMPortBox.Name = "COMPortBox";
            this.COMPortBox.Size = new System.Drawing.Size(286, 28);
            this.COMPortBox.TabIndex = 3;
            this.COMPortBox.Text = "Select COM Port...";
            this.COMPortBox.DropDown += new System.EventHandler(this.COMPortBox_DropDown);
            this.COMPortBox.SelectedIndexChanged += new System.EventHandler(this.COMPortBox_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Location = new System.Drawing.Point(385, 12);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.AutoScroll = true;
            this.splitContainer2.Panel1.Controls.Add(this.CommandDescriptionTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.CommandParamsContainer);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnRun);
            this.splitContainer2.Panel2.Controls.Add(this.btnCopy);
            this.splitContainer2.Panel2.Controls.Add(this.commandComboBox);
            this.splitContainer2.Panel2.Controls.Add(this.ConsoleTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(825, 582);
            this.splitContainer2.SplitterDistance = 259;
            this.splitContainer2.TabIndex = 16;
            // 
            // CommandDescriptionTextBox
            // 
            this.CommandDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandDescriptionTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.CommandDescriptionTextBox.Font = new System.Drawing.Font("Trebuchet MS", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommandDescriptionTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CommandDescriptionTextBox.Location = new System.Drawing.Point(0, 10);
            this.CommandDescriptionTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CommandDescriptionTextBox.Name = "CommandDescriptionTextBox";
            this.CommandDescriptionTextBox.Size = new System.Drawing.Size(796, 193);
            this.CommandDescriptionTextBox.TabIndex = 10;
            this.CommandDescriptionTextBox.Text = "";
            // 
            // CommandParamsContainer
            // 
            this.CommandParamsContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandParamsContainer.AutoScroll = true;
            this.CommandParamsContainer.AutoScrollMinSize = new System.Drawing.Size(100, 100);
            this.CommandParamsContainer.AutoSize = true;
            this.CommandParamsContainer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.CommandParamsContainer.Controls.Add(this.groupBox1);
            this.CommandParamsContainer.Location = new System.Drawing.Point(0, 209);
            this.CommandParamsContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CommandParamsContainer.Name = "CommandParamsContainer";
            this.CommandParamsContainer.Size = new System.Drawing.Size(796, 200);
            this.CommandParamsContainer.TabIndex = 9;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(267, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.groupBox1.Visible = false;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(16, 58);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(126, 24);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 88);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "text";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 0);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(113, 24);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(8, 2);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 28);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // commandComboBox
            // 
            this.commandComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandComboBox.FormattingEnabled = true;
            this.commandComboBox.Location = new System.Drawing.Point(89, 2);
            this.commandComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.commandComboBox.Name = "commandComboBox";
            this.commandComboBox.Size = new System.Drawing.Size(643, 28);
            this.commandComboBox.TabIndex = 15;
            this.commandComboBox.Enter += new System.EventHandler(this.commandComboBox_Enter);
            this.commandComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.commandComboBox_KeyPress);
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleTextBox.BackColor = System.Drawing.Color.Black;
            this.ConsoleTextBox.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ConsoleTextBox.Location = new System.Drawing.Point(0, 36);
            this.ConsoleTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.Size = new System.Drawing.Size(813, 274);
            this.ConsoleTextBox.TabIndex = 11;
            this.ConsoleTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 650);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "PM3 Universal GUI";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.CommandParamsContainer.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip defaultToolTip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView PM3CommandsTree;
        private System.Windows.Forms.ComboBox COMPortBox;
        private System.Windows.Forms.Button btnReloadCommands;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox CommandDescriptionTextBox;
        private System.Windows.Forms.FlowLayoutPanel CommandParamsContainer;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ComboBox commandComboBox;
        private System.Windows.Forms.RichTextBox ConsoleTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

