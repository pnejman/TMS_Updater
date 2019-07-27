using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace TMS_Updater
{
    public class ExtractedData //i think this class should have a constructor
    {
        public string sourceLang; //never expose public writeable fields on a class
        public string targetLang;
        public string rawOriginal; //raw original what?
        
        //I spent 20 minutes explaining why you should use FileInfo class rather than strings because then you don't have the problem of 'is the variable 'file' a file path or a name? if it's a name, does it contain extension? or maybe it's file content?'...
        //also, if you are using a string for such a thing, better call it 'fullXliffPath' or 'xliffFileNameWithoutExtension' etc, rather than call it something misleading and add a COMMENT in a class...
        public string file; //full path to XLIFF with file name and extension

        public string ZipNameWithPath; //yeah, better like that... (but should begin with lowercase)
        public string archiveSubname;

        public string XliffFilename() //again, what is this? method names should describe it's ACTIVITY, not a piece of data.
        {
            //that's something that would probably benefit from a quick unit test. and I bet your keyboard that it would have been faster to develop & debug in a test method rather than how you've done when working on it
            return Regex.Replace(this.file, ".*?\\\\", ""); //backslash order: #1: escape #2 from c# code, #2: escape #4 from Regex, #3: escape #4 from c# code, #4: actual backslash
        }

        public string TaskID()
        {
            //that's something that would probably benefit from a quick unit test. and I bet your keyboard that it would have been faster to develop & debug in a test method rather than how you've done when working on it

            return Regex.Replace(this.rawOriginal, "(.*?\\\\Data\\\\TC\\\\)|(\\\\SRC\\\\.*?$)", "");
        }

        //public string FilenameFull() //original file name with extension
        //{
        //    return Regex.Replace(this.rawOriginal, ".*?\\\\", ""); 
        //}
        //public string Filename() //original file name without extension
        //{
        //    return Regex.Replace(this.rawOriginal, ".*?\\\\|\\..*?$", "");
        //}
    }
}
