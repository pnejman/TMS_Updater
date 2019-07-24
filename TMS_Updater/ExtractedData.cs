using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TMS_Updater
{
    public class ExtractedData
    {
        public string sourceLang;
        public string targetLang;
        public string rawOriginal;
        public string file; //full path to XLIFF with file name and extension
        public string ZipName;
        public string archiveSubname;

        public string FilenameFull() //original file name with extension
        {
            return Regex.Replace(this.rawOriginal, ".*?\\\\", ""); //backslash order: #1: escape #2 from c# code, #2: escape #4 from Regex, #3: escape #4 from c# code, #4: actual backslash
        }
        public string Filename() //original file name without extension
        {
            return Regex.Replace(this.rawOriginal, ".*?\\\\|\\..*?$", "");
        }

        public string TaskID()
        {
            return Regex.Replace(this.rawOriginal, "(.*?\\\\Data\\\\TC\\\\)|(\\\\SRC\\\\.*?$)", "");
        }
    }
}
