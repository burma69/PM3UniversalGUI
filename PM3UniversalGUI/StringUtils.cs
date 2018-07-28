using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM3UniversalGUI
{
    class StringUtils
    {
        public static string ExtractBrackets(string Text, char OpeningBracket, char ClosingBracket)
        {
            string Result = Text;
            int i1, i2;

            i1 = Text.IndexOf(OpeningBracket);
            if (i1 >= 0)
            {
                i1++;
                i2 = Text.Substring(i1).IndexOf(ClosingBracket) + i1;
                if (i2 > i1)
                {
                    Result = Text.Substring(i1, i2 - i1).Trim();
                }
            }

            return Result;
        }

        public static string[] SplitNearby(string Text, char[] SplitChars)
        {
            foreach (char SplitChar in SplitChars)
            {
                int i1 = Text.IndexOf(SplitChar);
                if (i1 < 0) continue;

                string[] Values = Text.Split(SplitChar);

                for (int i = 0; i < Values.Length; i++)
                    Values[i] = Values[i].Trim();

                int i2 = Values[0].LastIndexOf(' ');
                if (Values[0].LastIndexOf('(') > i2) i2 = Values[0].LastIndexOf('(');
                if (Values[0].LastIndexOf('<') > i2) i2 = Values[0].LastIndexOf('<');

                int i3 = Values[Values.Length - 1].IndexOf(' ');
                int i4 = Values[Values.Length - 1].IndexOf(')');
                if (i4 > 0 && (i4 < i3 || i3 < 0)) i3 = i4;
                i4 = Values[Values.Length - 1].IndexOf('>');
                if (i4 > 0 && (i4 < i3 || i3 < 0)) i3 = i4;

                if (i2 >= 0) Values[0] = Values[0].Substring(i2 + 1);
                if (i3 >= 0 && Values.Length > 1) Values[Values.Length - 1] = Values[Values.Length - 1].Substring(0, i3);

                return Values;
            }

            return null;
        }
    }
}
