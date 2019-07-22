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
        public event EventHandler<string> msgToLcd;

        public void Begin(string pathToSource, string pathToTMS)
        {
            List<string> allFiles = Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).ToList();
            if (allFiles.Count() == 0)
            {
                msgToLcd?.Invoke(this, $"Error: No SDLXLIFF files found in given directory.\r\n");
                return;
            }

            msgToLcd?.Invoke(this, $"{allFiles.Count()} SDLXLIFF files detected.\r\n");
            ExtractDataFromAll(allFiles);
        }

        List<ExtractedData> ExtractDataFromAll(List<string> allFiles)
        {
            List<ExtractedData> tempList = new List<ExtractedData>();
            foreach (var file in allFiles)
            {
                ExtractedData dataToAdd = ExtractDataFromOne(file);
                tempList.Add(dataToAdd);
                msgToLcd?.Invoke(this, $"{dataToAdd.Zipname()}\r\n"); //debug
            }
            return tempList;
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
                        rawOriginal = fileReader.GetAttribute("original");
                        rawSRCLang = fileReader.GetAttribute("source-language");
                        rawTRGLang = fileReader.GetAttribute("target-language");
                        break;
                    }
                }
            }

            return new ExtractedData
            {
                sourceLang = rawSRCLang,
                targetLang = rawTRGLang,
                rawOriginal = rawOriginal,
            };
        }
    }
}

