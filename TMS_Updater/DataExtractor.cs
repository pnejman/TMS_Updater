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
        int noOfFilesDetected;
        int noOfFilesProcessed = 0;
        Logger logger;

        public void Begin(string pathToSource, string pathToTMS)
        {
            try
            {
                this.logger = new Logger();
                msgToLcd?.Invoke(this, $"Log file created at \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".");
            }
            catch (Exception e)
            {
                msgToLcd?.Invoke(this, $"Warning: Failed to create log file at \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".\r\n" +
                                       $"{e.Message}");
            }

            List<string> allFiles = Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).ToList();
            if (allFiles.Count() == 0)
            {
                msgToLcd?.Invoke(this, $"Error: No SDLXLIFF files found in given directory.");
                this.logger.Log($"No SDLXLIFF files were found in given directory.");
                return;
            }

            this.noOfFilesDetected = allFiles.Count();
            msgToLcd?.Invoke(this, $"{noOfFilesDetected} SDLXLIFF files detected.");
            this.logger.Log($"{noOfFilesDetected} SDLXLIFF files detected.");
            ExtractDataFromAll(allFiles);
            End();
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
                    this.noOfFilesProcessed++;
                }
                catch (Exception e)
                {
                    msgToLcd?.Invoke(this, $"Error: Failed to extract data from:\r\n" +
                                           $"{file}\r\n" +
                                           $"{e.Message}");
                    this.logger.Log($"Error: Failed to extract data from:\r\n" +
                                    $"{file}\r\n" +
                                    $"{e.Message}");
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
            };
        }

        void End()
        {
            msgToLcd?.Invoke(this, $"__________\r\n" +
                                   $"Job's done.\r\n" +
                                   $"Files detected: {this.noOfFilesDetected}\r\n" +
                                   $"Files processed: {this.noOfFilesProcessed}\r\n");
            this.logger.Log($"__________\r\n" +
                            $"Job's done.\r\n" +
                            $"Files detected: {this.noOfFilesDetected}\r\n" +
                            $"Files processed: {this.noOfFilesProcessed}\r\n");
        }
    }
}

