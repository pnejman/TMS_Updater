using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TMS_Updater
{
    public class ZipInjector
    {
        public event EventHandler<string> msgToLcd;
        List<ExtractedData> fullListOfextrData;
        string pathToSrc;
        string pathToTMS;
        Logger logger;
        int noOfFilesProcessed = 0;

        public ZipInjector(string pathToSrc, string pathToTMS, List<ExtractedData> extractedData, Logger logger)
        {
            this.fullListOfextrData = extractedData;
            this.pathToSrc = pathToSrc;
            this.pathToTMS = pathToTMS;
            this.logger = logger;
        }

        public void Begin()
        {
            foreach (ExtractedData currentXliffData in this.fullListOfextrData)
            {
                if (!(DoesArchiveExist(currentXliffData)))
                {
                    continue; //skip reminder of this iteration
                }
                this.noOfFilesProcessed++;
            }

            DisplaySummary();
        }

        bool DoesArchiveExist(ExtractedData currentXliffData)
        {
            List<string> archivesForThisXliff = Directory.GetFiles(this.pathToTMS, currentXliffData.TaskID() + "_*").ToList();
            if (archivesForThisXliff.Count == 1)
            {
                return true;
            }
            else
            {
                msgToLcd?.Invoke(this, $"Error while processing file\r\n" +
                           $"{currentXliffData.file}\r\n" +
                           $"Archive for Task ID: {currentXliffData.TaskID()} not found.");
                this.logger.Log($"Error while processing file\r\n" +
                                $"{currentXliffData.file}\r\n" +
                                $"Archive for Task ID: {currentXliffData.TaskID()} not found.");
                return false;
            }
        }

        void DisplaySummary()
        {
            msgToLcd?.Invoke(this, $"Job's done.\r\n" +
                                   $"Files detected: {this.fullListOfextrData.Count}\r\n" +
                                   $"Files processed: {this.noOfFilesProcessed}\r\n");
            this.logger.Log($"Job's done.\r\n" +
                            $"Files detected: {this.fullListOfextrData.Count}\r\n" +
                            $"Files processed: {this.noOfFilesProcessed}\r\n");
        }
    }
}
