using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TMS_Updater
{
    public class LanguageCodesGlossary
    {
        public Dictionary<string, string> content = new Dictionary<string, string>(); //again, public mutable fields are bad idea

        public void Prepare()
        {
            string line;
            System.IO.StreamReader file = null;

            try //the thing you're doing here is very weird and ugly
            {
                file = new System.IO.StreamReader("glossary.txt"); //for streams always prefer the 'using' pattern . If anything goes wrong, you're not disposing your reader and stream, which can cause issues
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
                    if (line == Regex.Match(line, "[a-zA-Z0-9\\-]+ means [a-zA-Z0-9\\-]+\\.").ToString()) //avoid plain text files for structured data... there's lots of formats like XML and JSON or CSV which are better
                    {
                        string nameInTMS = Regex.Replace(line, " means.*?\\.", "");
                        string nameInFiles = Regex.Replace(line, ".*? means |\\.", "");
                        content.Add(nameInTMS, nameInFiles);
                    }
                }
                catch //so, you can create a glossary, and make a typo and miss some lang pair mapping and never realize that something went wrong...
                {
                    continue;
                }
            }
            file.Close(); //as I said, use 'using' pattern
        }
    }
}
