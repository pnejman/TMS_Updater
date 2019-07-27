using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;

namespace TMS_Updater
{
    public class DataExtractor
    {
        //public event EventHandler<string> passMsgToDisplay;
        Logger logger;

        public void Begin(string pathToSource, string pathToTMS, Logger logger) //I am happy that this is the only public method here.
        //however, you tend to call your methods 'Begin'. This is not a good name, because by convention it suggests to the user that its an asynchronous method that requires calling an 'End' method at some point
        //in other words, it suggests following the pattern like here https://stackoverflow.com/questions/11620310/do-you-have-to-call-endinvoke-or-define-a-callback-for-asynchronous-method-ca
        //but it does not, so it's confusing. If you don't have a better name (like e.g. Extract(), then just call it something generic like Work() or Do())
        {
            this.logger = logger;
            this.logger.Log($"Fixed files path set to:\r\n{pathToSource}");
            this.logger.Log($"TMS path set to:\r\n{pathToTMS}");

            if (!(ArePathsValid(pathToSource, pathToTMS)))
            {
                return;
            }

            List<string> allFiles = Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).ToList();
            this.logger.Log($"{allFiles.Count()} sdlxliff files detected.");

            ZipInjector zipInjector = new ZipInjector(pathToTMS, ExtractDataFromAll(allFiles), this.logger); //better use the Inversion of Control with Dependency Injection patterns,
                                                                                                             //i.e. pass the ZipInjector as parameter the same way you're passing the logger
                                                                                                             //this makes it clear which component relies on which
            zipInjector.Begin();
        }

        bool ArePathsValid(string pathToSource, string pathToTMS) //an MSFT convention suggests to always explicitly specify the access modifier
        {
            if (Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).Length == 0)  //when comparing strings bear in mind case sensitivity. What if the file is .SDLXLIFF?
            {
                this.logger.Log($"Error: No SDLXLIFF files found in given directory."); //as a best practice... in error message include *what* is the given directory. Otherwise it's quite frustrating to read a message and not know what exactly is wrong and where
                return false;
            }

            if (Directory.GetFiles(pathToTMS, "*.zip").Length == 0)
            {
                this.logger.Log($"Error: No archives found in given TMS directory.");
                return false;
            }

            return true;
        }

        List<ExtractedData> ExtractDataFromAll(List<string> allFiles)
        {
            List<ExtractedData> listOfExtractedData = new List<ExtractedData>();
            foreach (var file in allFiles)
            {
                try
                {
                    ExtractedData dataToAdd = ExtractDataFromOne(file);
                    listOfExtractedData.Add(dataToAdd);
                }
                catch (Exception e)
                {
                    string text = $"Error: Failed to extract data from:\r\n" +
                                  $"{file}\r\n" +
                                  $"{e.Message}";
                    this.logger.Log(text);
                }
            }
            return listOfExtractedData;
        }

        ExtractedData ExtractDataFromOne(string file)
        {
            string rawOriginal = "";
            string rawSRCLang = "";
            string rawTRGLang = "";

            XmlReader fileReader = XmlReader.Create(file);
            while (fileReader.Read())
            {
                if (fileReader.NodeType == XmlNodeType.Element)
                {
                    if (fileReader.Name == "doc-info")
                    {
                        fileReader.Skip();
                    }

                    if (fileReader.Name == "file")
                    {
                        rawOriginal = fileReader.GetAttribute("original"); //"original" is the name of attribute inside "file" element <-- well, here it's quite obvious, since you're calling a 'GetAttribute' on an element called File
                        rawSRCLang = fileReader.GetAttribute("source-language");
                        rawTRGLang = fileReader.GetAttribute("target-language");
                        break; //skip rest of the file
                    }
                }
            }

            return new ExtractedData
            {
                sourceLang = rawSRCLang,
                targetLang = rawTRGLang,
                rawOriginal = rawOriginal,
                nameAndPathOfNewFile = file,
            };
        }
    }
}

