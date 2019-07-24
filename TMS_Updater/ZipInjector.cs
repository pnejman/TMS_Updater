using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace TMS_Updater
{
    public class ZipInjector
    {
        public event EventHandler<string> msgToLcd;
        List<ExtractedData> fullListOfextrData;
        string pathToTMS;
        Logger logger;
        int noOfFilesProcessed = 0;

        public ZipInjector(string pathToTMS, List<ExtractedData> extractedData, Logger logger)
        {
            this.fullListOfextrData = extractedData;
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

                currentXliffData.archiveSubname = DetermineSubfolderName(currentXliffData);

                if (!(DoesFileInArchiveExist(currentXliffData)))
                {
                    continue;
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
                currentXliffData.ZipName = archivesForThisXliff[0];
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

        string DetermineSubfolderName(ExtractedData currentXliffData)
        {
            string part1;
            string part2;

            switch (currentXliffData.sourceLang)
            {
                //case "xy-XY":
                //    placeholder for exceptions
                //    break;
                case "da-DK":
                    part1 = "DA";
                    break;
                default:
                    if (Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper() == Regex.Replace(currentXliffData.sourceLang, "[A-Za-z]+-", "").ToUpper()) //source language code and flavour code are the same
                    {
                        part1 = Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper();//xy-XY will be converted to XY
                    }
                    else
                    {
                        part1 = Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper() + "-" + Regex.Replace(currentXliffData.sourceLang, "[A-Za-z]+-", "").ToUpper();//xy-XY will be converted to XY
                    }
                    break;
            }

            switch (currentXliffData.targetLang)
            {
                //case "xy-XY":
                //    placeholder for exceptions
                //    break;
                case "da-DK":
                    part2 = "DA";
                    break;
                default:
                    if (Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper() == Regex.Replace(currentXliffData.targetLang, "[A-Za-z]+-", "").ToUpper())
                    {
                        part2 = Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper();
                    }
                    else
                    {
                        part2 = Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper() + "-" + Regex.Replace(currentXliffData.targetLang, "[A-Za-z]+-", "").ToUpper();
                    }
                    break;
            }

            return part1 + "_" + part2;
        }

        bool DoesFileInArchiveExist(ExtractedData currentXliffData)
        {
            return true;
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
