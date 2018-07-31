using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Configuration;

namespace PM3UniversalGUI
{


    class PM3CommandParamAllowedValue
    {
        public string Value, Description;

        public PM3CommandParamAllowedValue(string Value, string Description)
        {
            this.Value = Value;
            this.Description = Description;
        }

        public override string ToString()
        {
            string Result = Value;

            if (Description != null) Result += " - " + Description;

            return Result;
        }
    }

    class PM3CommandParam
    {
        public enum EParamType { Fixed, Flag, Value };

        public string Name, Description;
        public bool IsOptional = false; //e.g. [x]
        public bool OrWithNext = false; //e.g. [x|y]
        public bool GroupWithNext = false; //e.g. [x <y>]
        public EParamType ParamType = EParamType.Fixed;

        public List<PM3CommandParamAllowedValue> AllowedValues = new List<PM3CommandParamAllowedValue>();
    }

    class PM3Command
    {
        public string Command, Description, DescriptionFull;
        public string Usage, Options, Examples;

        public List<PM3CommandParam> Params = new List<PM3CommandParam>();

        public PM3Command(string Command, string Description)
        {
            this.Command = Command;
            this.Description = Description;
        }

        public void ParseUsage(string UsageString)
        {
            Params.Clear();

            bool TokenIsOptional = false;
            bool TokenIsValue = false;
            bool ReadingToken = false;
            bool FinishingToken = false;
            PM3CommandParam p = new PM3CommandParam();

            for (int i = 0; i < UsageString.Length; i++)
            {
                if (UsageString[i] == ' ' && !TokenIsValue && !TokenIsOptional) FinishingToken = true;

                if (UsageString[i] == '[' && !TokenIsValue)
                {
                    FinishingToken = true;
                    TokenIsOptional = true;
                }

                if (UsageString[i] == '<')
                {
                    if ((ReadingToken && p.IsOptional) || p.ParamType == PM3CommandParam.EParamType.Fixed) p.GroupWithNext = true;
                    FinishingToken = true;
                    TokenIsValue = true;
                }

                if (UsageString[i] == '|' && !TokenIsValue)
                {
                    p.OrWithNext = true;
                    p.GroupWithNext = true;
                    FinishingToken = true;                    
                }

                if (UsageString[i] == ']')
                {
                    TokenIsOptional = false;
                    FinishingToken = true;
                }

                if (UsageString[i] == '>')
                {
                    TokenIsValue = false;
                    FinishingToken = true;
                }

                if (FinishingToken)
                {
                    if (ReadingToken && p.Name != null)
                    {
                        p.Name = p.Name.TrimEnd();
                        if (p.Name != "") Params.Add(p);
                    }
                    ReadingToken = false;
                    FinishingToken = false;

                    continue;
                }

                if (!ReadingToken)
                {
                    p = new PM3CommandParam();

                    if (TokenIsValue) p.ParamType = PM3CommandParam.EParamType.Value;

                    if (TokenIsOptional)
                    {
                        p.IsOptional = true;
                        if (!TokenIsValue) p.ParamType = PM3CommandParam.EParamType.Flag;
                    }
                    else
                    {
                        if (!TokenIsValue) p.ParamType = PM3CommandParam.EParamType.Fixed;
                    }

                    ReadingToken = true;
                }
                if (ReadingToken)
                {
                    if (p.Name != null || UsageString[i] != ' ') p.Name += UsageString[i];
                    continue;
                }
            }

            if (ReadingToken && p.Name != null)
            {
                p.Name = p.Name.TrimEnd();
                if (p.Name != "") Params.Add(p);
            }



            foreach (PM3CommandParam pp in Params) //postprocessing to handle "<name valueA|ValueB>" and "name (valueA/ValueB/valueC)" cases
            {
                string NameClean = pp.Name.Replace("w/o", "w\\o");
                string[] Values = StringUtils.SplitNearby(NameClean, new char[] { '|', '/' });

                if (Values != null)
                    for (int i = 0; i < Values.Length; i++)
                    {
                        pp.AllowedValues.Add(new PM3CommandParamAllowedValue(Values[i], null));
                    }
            }
        }

        public void ParseOptionDescription(string OptionDescription, string RelatedTo = null)
        {
            if (OptionDescription.IndexOf(' ') < 0) return;
            string RelatedToClean = RelatedTo;

            if (RelatedToClean != null)
            {
                RelatedToClean = StringUtils.ExtractBrackets(RelatedToClean, '[', ']');
                RelatedToClean = StringUtils.ExtractBrackets(RelatedToClean, '<', '>');
                if (RelatedToClean.IndexOf(':') > 0) RelatedToClean = RelatedToClean.Substring(0, RelatedToClean.IndexOf(':') - 1).Trim();

                if (RelatedTo.StartsWith("*"))
                {
                    foreach (PM3CommandParam p in Params)
                        if (p.Name[0] == '*')
                        {
                            RelatedToClean = p.Name;
                            break;
                        }
                }
            }
            string OptionName = OptionDescription.Trim();

            if (OptionName.StartsWith("[")) OptionName = StringUtils.ExtractBrackets(OptionName, '[', ']');
            else if (OptionName.StartsWith("<")) OptionName = StringUtils.ExtractBrackets(OptionName, '<', '>');
            else OptionName = OptionName.Substring(0, OptionDescription.IndexOfAny(new char[] { ' ', ':', '-' }));

            for (int i = 0; i < Params.Count; i++)
            {
                PM3CommandParam p = Params[i];

                if (p.Name == OptionName && p.Description == null) //full match with expected parameter found -> whatever we see, it is less likely to be one of the allowed values for some other parameter
                {
                    RelatedToClean = null;
                    break;
                }
            }
            List<string[]> ValueDescriptions = new List<string[]>();


            //if there are too many '=' or '-' maybe it's not a description, but the list of allowed values instead
            if ((OptionDescription.Split(',').Length >= 2) &&
                (OptionDescription.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries).Length >= 3 
                || OptionDescription.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries).Length >= 3
                || OptionDescription.Split(new string[] { " for " }, StringSplitOptions.RemoveEmptyEntries).Length >= 3)
                )
            {
                if (RelatedToClean == null) RelatedToClean = OptionName;
                string[] ValueDescriptionTmp = OptionDescription.Replace('=', '-').Replace(" for ", "-").Split(',');

                foreach(string s in ValueDescriptionTmp)
                {
                    if (s.IndexOf('-') > 0)
                        ValueDescriptions.Add(s.Split('-'));
                }
            } else
            if ((OptionDescription.Split('|').Length > 2))
            {
                if (RelatedToClean == null) RelatedToClean = OptionName;
                string[] ValueDescriptionTmp = StringUtils.SplitNearby(OptionDescription, new char[] { '|' });

                foreach (string s in ValueDescriptionTmp)
                {
                        ValueDescriptions.Add(new string[] { s });
                }
            }
            else
            if (RelatedToClean != null && RelatedToClean != "")
            {
                int i1 = OptionDescription.IndexOf('=');
                int i2 = OptionDescription.IndexOf('-');
                if (i1 >=0 && (i2 <0 || i2 > i1))
                    ValueDescriptions.Add(OptionDescription.Split('='));
                else
                ValueDescriptions.Add(OptionDescription.Split('-'));
            }
            
            int MatchingId = -1;

            //trying to find the exact match of description with parameter name mentioned in the usage
            if (MatchingId < 0)
                for (int i = 0; i < Params.Count; i++)
                {
                    PM3CommandParam p = Params[i];

                    if (p.Name == OptionName ||
                        (RelatedToClean != null && p.Name == RelatedToClean)) //maybe it's not the parameter, but a description of allowed value of some parameter
                    {
                        MatchingId = i;
                        break;
                    }
                }




            //second chance by looking up partial match
            if (MatchingId < 0 && OptionName.IndexOf(' ') > 0) 
            {
                string OptionNamePartial = OptionName.Substring(0, OptionName.IndexOf(' '));

                for (int i = 0; i < Params.Count; i++)
                {
                    if (Params[i].Name == OptionNamePartial || (RelatedToClean != null && Params[i].Name == RelatedToClean))
                    {
                        MatchingId = i;

                        string[] Values = StringUtils.SplitNearby(OptionName, new char[] { '|', '/' });

                        if (Values != null)
                        {
                            for (int j = 0; j < Values.Length; j++)
                            {
                                ValueDescriptions.Add(new string[] { Values[j] });
                            }

                            if (RelatedToClean == null) RelatedToClean = OptionNamePartial;
                        }

                        break;
                    }
                }
            }


            if (MatchingId >= 0)
            {
                /* to handle case:
                * usage: cmd p <v>  
                * options:
                *  p : 1 = a
                *      2 = b
                *  (should have been "<v>", not "p"!!!)
                */
                if (ValueDescriptions.Count > 0)
                if (Params[MatchingId].ParamType != PM3CommandParam.EParamType.Value)
                {
                    if (MatchingId + 1 < Params.Count)
                        if (Params[MatchingId + 1].ParamType == PM3CommandParam.EParamType.Value)
                                if (Params[MatchingId].Name[0] == Params[MatchingId + 1].Name[0])
                            MatchingId++;
                }

                if (RelatedToClean != null && RelatedToClean != "" && ValueDescriptions.Count > 0)
                {
                    foreach (string[] ValueDescription in ValueDescriptions)
                    {
                        string Value = ValueDescription[0].Trim();

                        int i = Value.IndexOf(':');

                        if (i > 0)
                        {
                            if (i < Value.Length - 1) Value = Value.Substring(Value.IndexOf(':') + 1).Trim();
                            else Value = Value.Substring(0, i - 1);
                        }
                        Value = StringUtils.ExtractBrackets(Value, '\'', '\'');
                        Value = StringUtils.ExtractBrackets(Value, '[', ']');
                        Value = StringUtils.ExtractBrackets(Value, '<', '>');
                        string Description = null;
                        if (ValueDescription.Length > 1) Description = ValueDescription[1].Trim();
                        PM3CommandParamAllowedValue v = new PM3CommandParamAllowedValue(Value, Description);
                        if (Params[MatchingId].Name[0] == '*') v.Value = '*' + v.Value;

                        Params[MatchingId].AllowedValues.Add(v);
                    }
                }
                else
                {
                    Params[MatchingId].Description = OptionDescription.Remove(0, OptionDescription.IndexOf(' ')).Trim();

                    if (Params[MatchingId].Description.Length > 1)
                        if (Params[MatchingId].Description[0] == '-')
                            Params[MatchingId].Description = Params[MatchingId].Description.Substring(1).TrimStart();

                    if (Params[MatchingId].Description.Length > 1)
                        if (Params[MatchingId].Description[0] == ':')
                            Params[MatchingId].Description = Params[MatchingId].Description.Substring(1).TrimStart();

                    if (Params[MatchingId].Description.IndexOf("Optional", StringComparison.InvariantCultureIgnoreCase) > 0)
                    {
                        Params[MatchingId].IsOptional = true;

                        if (Params[MatchingId].Description != null)
                            if (Params[MatchingId].ParamType == PM3CommandParam.EParamType.Fixed)
                                Params[MatchingId].ParamType = PM3CommandParam.EParamType.Flag;
                    }


                }
            }
        }
    }

    class PM3Client
    {
        public Process ClientProcess = new Process();
        public List<PM3Command> Commands = new List<PM3Command>();
        public string PM3FileName;

        public PM3Client(string PM3FileName)
        {
            this.PM3FileName = PM3FileName;
        }

        public bool IsRunning()
        {
            try { Process.GetProcessById(ClientProcess.Id); }
            catch (InvalidOperationException) { return false; }
            catch (ArgumentException) { return false; }
            return true;
        }

        public void InitClient(string PortName)
        {
            ClientProcess.StartInfo.FileName = PM3FileName; // Specify exe name.
            ClientProcess.StartInfo.Arguments = PortName +" -f";
            ClientProcess.StartInfo.UseShellExecute = false;
            ClientProcess.StartInfo.RedirectStandardOutput = true;
            ClientProcess.StartInfo.RedirectStandardInput = true;
            ClientProcess.StartInfo.CreateNoWindow = true;
        }

        public void StartClient()
        {
            ClientProcess.Start();

            ClientProcess.BeginOutputReadLine();
        }

        //try to parse detailed help output of specific command
        public void ExtractCommandParams(PM3Command cmd)
        {
            Process TmpProcess = new Process();
            TmpProcess.StartInfo.FileName = PM3FileName;
            TmpProcess.StartInfo.Arguments = "-c \"" + cmd.Command + " h\"";
            TmpProcess.StartInfo.UseShellExecute = false;            
            TmpProcess.StartInfo.RedirectStandardOutput = true;
            TmpProcess.StartInfo.CreateNoWindow = true;

            TmpProcess.Start();

            Task<string> t = TmpProcess.StandardOutput.ReadToEndAsync();
            Task t2 = Task.WhenAny(t, Task.Delay(Int32.Parse(ConfigurationManager.AppSettings["ConsoleDumpTimeout"]))).GetAwaiter().GetResult();
            if (t2.Id == t.Id)
            {
                // task completed within timeout
            }
            else
            {
                try
                {
                    TmpProcess.Kill();
                } catch (Exception e)
                {

                }
            }
            string Output = t.Result;

            cmd.Usage = null;
            cmd.Examples = null;
            cmd.DescriptionFull = null;
            bool OptionsBlockExpected = Output.IndexOf("Options:") > 0;

            using (StringReader sr = new StringReader(Output))
            {
                int Block = -1;
                string RelatedTo = null;
                int RelatedToIndent = 0;
                bool UsageParsed = false;
                string PrevDescription = null;
                string PrevRelatedTo = null;
                int PrevIndent = 0;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("[!]") == 0) continue;
                    if (Block == -1 && line.TrimEnd().EndsWith("> " + cmd.Command + " h"))
                    {
                        Block = 3;
                        continue;
                    }
                    if (line.TrimStart().IndexOf("Usage:", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        Block = 0;
                    }

                    if (line.TrimStart().IndexOf("Options:", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        Block = 1;
                        continue;
                    }

                    if (line.TrimStart().IndexOf("Example", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        Block = 2;
                    }

                    if (Block == 0) //usage
                    {
                        string UsageString = line.Replace("Usage:", "").Replace("\t", "").Trim();
                        if (UsageString.Length > 0)
                        {
                            cmd.Usage += UsageString + "\r\n";
                            if (!OptionsBlockExpected) Block = 1; //if there is no explicit options block we do not expect Usage to be longer than 1 line
                        }
                    } else
                    if (Block == 1) //options
                    {
                        cmd.Options += line + "\r\n";
                        if (cmd.Usage != null && !UsageParsed)
                        {
                            string[] UsageParts = cmd.Usage.Trim().Replace("\r\n", " ").Split(new string[] { cmd.Command }, StringSplitOptions.RemoveEmptyEntries);
                            string UsageString = null;
                            if (UsageParts.Length > 0) UsageString = UsageParts[0].Trim(); else UsageString = cmd.Usage.Trim();

                            cmd.ParseUsage(UsageString);
                            UsageParsed = true;
                        }

                        string OptionDescription = line.Trim();
                        int Indent = line.IndexOf(OptionDescription);

                        if (Indent > PrevIndent && PrevDescription != null && PrevDescription != "" && RelatedTo == null)
                        {
                            RelatedTo = PrevDescription;
                            RelatedToIndent = PrevIndent;
                        }

                        if (RelatedTo != null)
                        {
                            if (false//(line.IndexOf("\t\t\t") < 0)
                                || (OptionDescription.Replace("\t", "") == "" || !line.StartsWith(" "))
                                || (RelatedToIndent == Indent)
                                || (Indent < PrevIndent))
                                RelatedTo = null;
                        }

                        if (PrevRelatedTo == null && RelatedTo != null) //if we've missed the option in the first line
                        {
                            if (RelatedTo.Split('=').Length == 2 || RelatedTo.Split('-').Length == 2)
                                cmd.ParseOptionDescription(RelatedTo, RelatedTo);
                        }

                        cmd.ParseOptionDescription(OptionDescription, RelatedTo);

                        if (OptionDescription.Length > 1)
                        if ((OptionDescription.IndexOf('*') == 0)
                            || (OptionDescription.IndexOf(':') == OptionDescription.Length - 1)
                            || (OptionDescription.IndexOf(" see ") > 0))
                        {
                            RelatedTo = OptionDescription;
                            RelatedToIndent = Indent;
                        }

                        PrevDescription = OptionDescription;
                        PrevIndent = Indent;
                        PrevRelatedTo = RelatedTo;
                    } else
                    if (Block == 2) //examples
                    {
                        cmd.Examples += line.Trim() + "\r\n";
                    }
                    else
                    if (Block == 3) //description full
                    {
                        cmd.DescriptionFull += line.Trim() + "\r\n";
                    }
                }

                if (!UsageParsed && cmd.Usage != null)
                {
                    string UsageString2 = cmd.Usage.Trim().Replace("\r\n", " ").Split(new string[] { cmd.Command }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    cmd.ParseUsage(UsageString2);

                    UsageParsed = true;
                }
            }

            //remove most likely wrongly detected fixed keywords in the beginning of the parameters block
            for (int i = 0; i < cmd.Params.Count; i++)
            {
                if (cmd.Params[i].ParamType != PM3CommandParam.EParamType.Fixed) break;
                if (cmd.Params[i].Description != null) break; //highly likely that's a real one
                cmd.Params.RemoveAt(i);
                i--;
            }
            //remove most likely wrongly detected fixed keywords in the end of the parameters block
            for (int i = cmd.Params.Count-1; i >= 0; i--)
            {
                if (cmd.Params[i].ParamType != PM3CommandParam.EParamType.Fixed) break;
                cmd.Params.RemoveAt(i);
            }
        }

        
        //load the commands list with basic descriptions
        public async void LoadCommands()
        {
            Process TmpProcess = new Process();
            TmpProcess.StartInfo.FileName = PM3FileName; // Specify exe name.
            TmpProcess.StartInfo.Arguments = "-h";
            TmpProcess.StartInfo.UseShellExecute = false;
            TmpProcess.StartInfo.RedirectStandardOutput = true;


            TmpProcess.Start();


            string CommandsList = TmpProcess.StandardOutput.ReadToEnd();

            Commands.Clear();

            using (StringReader sr = new StringReader(CommandsList))
            {
                bool StartParse = false;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.IndexOf("command") == 0) StartParse = true;

                    if (!StartParse) continue;

                    if (line.IndexOf("command") == 0 || line.IndexOf("-------") >= 0)
                    {
                        continue;
                    }

                    if (line.IndexOf("###") == 0)
                    {
                        Commands.Add(new PM3Command(line.Replace("###", "").Trim(), ""));
                    }
                    else if (line.IndexOf('|') > 0)
                    {
                        PM3Command cmd = new PM3Command((line.Split('|')[0]).Trim(), (line.Split('|')[2]).Trim());

                        Commands.Add(cmd);
                    }
                    else if (line.IndexOf('{') > 0 && line.IndexOf('}') > 0)
                    {
                        Commands.Last().Description = line.Substring(line.IndexOf('{') + 1, line.IndexOf('}') - line.IndexOf('{') - 2).Trim();
                    }
                }
            }

        }
    }
}
