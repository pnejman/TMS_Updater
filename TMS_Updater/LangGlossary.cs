using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TMS_Updater
{
    public class LangGlossary
    {
        public Dictionary<string, string> content = new Dictionary<string, string>();

        public void Prepare()
        {
            string line;
            System.IO.StreamReader file = null;

            try
            {
                file = new System.IO.StreamReader("glossary.txt");
            }
            catch
            {
                File.AppendAllText("glossary.txt", "* Xliff to TMS glossary.\r\n" +
                    "* Feel free to add new entries, but keep the correct pattern:\r\n" +
                    "* [XliffCode][space][\"means\"][space][TMSCode][full stop]\r\n" +
                    "* Invalid entries will be ignored.\r\n" +
                    "* Use asterisks for comments.\r\n\r\n" +
                    "el-GR means EL.\r\n" +
                    "da-DK means DA.\r\n");
                file = new System.IO.StreamReader("glossary.txt");
            }

            while (!(file.EndOfStream))
            {
                line = file.ReadLine();

                try
                {
                    if (line == Regex.Match(line, "[a-zA-Z0-9\\-]+ means [a-zA-Z0-9\\-]+\\.").ToString())
                    {
                        string nameInTMS = Regex.Replace(line, " means.*?\\.", "");
                        string nameInFiles = Regex.Replace(line, ".*? means |\\.", "");
                        content.Add(nameInTMS, nameInFiles);
                    }
                }
                catch
                {
                    continue;
                }
            }
            file.Close();
        }
    }
}
