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

        public void Begin(string pathToSource, string pathToTMS, Logger logger)
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

            ZipInjector zipInjector = new ZipInjector(pathToTMS, ExtractDataFromAll(allFiles), this.logger);
            zipInjector.Begin();
        }

        bool ArePathsValid(string pathToSource, string pathToTMS)
        {
            if (Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).Length == 0)
            {
                this.logger.Log($"Error: No SDLXLIFF files found in given directory.");
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
                        rawOriginal = fileReader.GetAttribute("original"); //"original" is the name of attribute inside "file" element
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

