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
        Logger logger;
        LanguageDictionary lng;

        public void Work(string pathToSource, string pathToTMS, Logger logger, LanguageDictionary lng)
        {
            this.lng = lng;
            this.logger = logger;
            this.logger.Log(lng.txt["Path to new files set to:"] + $"\r\n{pathToSource}");
            this.logger.Log(lng.txt["TMS path set to:"] + $"\r\n{pathToTMS}");

            if (!(ArePathsValid(pathToSource, pathToTMS)))
            {
                return;
            }

            List<string> allFiles = Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).ToList();
            this.logger.Log(lng.txt["Number of sdlxliff files detected:"] + $" {allFiles.Count()}");

            ZipInjector zipInjector = new ZipInjector(pathToTMS, ExtractDataFromAll(allFiles), this.logger, this.lng); //better use the Inversion of Control with Dependency Injection patterns,
                                                                                                             //i.e. pass the ZipInjector as parameter the same way you're passing the logger
                                                                                                             //this makes it clear which component relies on which
            zipInjector.Work();
        }

        bool ArePathsValid(string pathToSource, string pathToTMS) //an MSFT convention suggests to always explicitly specify the access modifier
        {
            if (Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).Length == 0)  //when comparing strings bear in mind case sensitivity. What if the file is .SDLXLIFF?
            {
                this.logger.Log(lng.txt["Error: No sdlxliff files found in given directory."] + "\r\n" +
                                       $"{pathToSource}");
                return false;
            }

            if (Directory.GetFiles(pathToTMS, "*.zip").Length == 0)
            {
                this.logger.Log(lng.txt["Error: No archives found in given TMS directory."] + "\r\n" +
                                       $"{pathToTMS}");
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
                    string text = lng.txt["Error: Failed to extract data from:"]+"\r\n" +
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

