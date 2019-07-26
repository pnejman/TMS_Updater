using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace TMS_Updater
{
    public class ZipInjector
    {
        public event EventHandler<string> msgToLcd;
        List<ExtractedData> fullListOfextrData;
        string pathToTMS;
        Logger logger;
        int noOfFilesProcessed = 0;
        LangGlossary glossary = new LangGlossary();

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

                if (!(IsFileInZipOlder(currentXliffData)))
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
                currentXliffData.ZipNameWithPath = archivesForThisXliff[0];
                return true;
            }
            else
            {
                string text = $"Error while processing file\r\n" +
                              $"{currentXliffData.file}\r\n" +
                              $"Archive for Task ID {currentXliffData.TaskID()} not found.";
                msgToLcd?.Invoke(this, text);
                this.logger.Log(text);
                return false;
            }
        }

        string DetermineSubfolderName(ExtractedData currentXliffData)
        {
            glossary.Prepare();

            string part1;
            string part2;
            string value;

            if (glossary.content.TryGetValue(currentXliffData.sourceLang, out value))
            {
                part1 = value;
            }
            else
            {
                if (Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper() == Regex.Replace(currentXliffData.sourceLang, "[A-Za-z]+-", "").ToUpper()) //source language code and flavour code are the same
                {
                    part1 = Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper();//xy-XY will be converted to XY
                }
                else
                {
                    part1 = Regex.Replace(currentXliffData.sourceLang, "-[A-Za-z]+", "").ToUpper() + "-" + Regex.Replace(currentXliffData.sourceLang, "[A-Za-z]+-", "").ToUpper();//xy-XY will be converted to XY
                }
            }

            if (glossary.content.TryGetValue(currentXliffData.targetLang, out value))
            {
                part2 = value;
            }
            else
            {

                if (Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper() == Regex.Replace(currentXliffData.targetLang, "[A-Za-z]+-", "").ToUpper())
                {
                    part2 = Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper();
                }
                else
                {
                    part2 = Regex.Replace(currentXliffData.targetLang, "-[A-Za-z]+", "").ToUpper() + "-" + Regex.Replace(currentXliffData.targetLang, "[A-Za-z]+-", "").ToUpper();
                }
            }
            return part1 + "_" + part2;
        }

        bool DoesFileInArchiveExist(ExtractedData currentXliffData)
        {
            using (ZipFile zip = ZipFile.Read(currentXliffData.ZipNameWithPath))
            {
                foreach (ZipEntry entry in zip.Entries)
                {
                    string expectedFilename = "TGT/" + currentXliffData.archiveSubname + "/" + currentXliffData.XliffFilename();
                    if (entry.FileName == expectedFilename)
                    {
                        return true;
                    }
                }
            }
            string text = $"Error: Couldn't find file:\r\n" +
                          $"{currentXliffData.XliffFilename()}\r\n" +
                          $"inside archive\r\n" +
                          $"{currentXliffData.ZipNameWithPath}";
            msgToLcd?.Invoke(this, text);
            this.logger.Log(text);
            return false;
        }

        bool IsFileInZipOlder(ExtractedData currentXliffData)
        {
            using (ZipFile zip = ZipFile.Read(currentXliffData.ZipNameWithPath))
            {
                foreach (ZipEntry entry in zip)
                {
                    string expectedFilename = "TGT/" + currentXliffData.archiveSubname + "/" + currentXliffData.XliffFilename();
                    if (entry.FileName == expectedFilename)
                    {
                        DateTime zipFileDate = entry.LastModified;
                        DateTime incomingFileDate = File.GetLastWriteTime(currentXliffData.file);
                        if (zipFileDate < incomingFileDate)
                        {
                            return true;
                        }
                    }
                }
            }
            string text = $"File skipped:\r\n" +
                          $"{currentXliffData.XliffFilename()}\r\n" +
                          $"because it's older or the same as the file already present in archive.";
            msgToLcd?.Invoke(this, text);
            this.logger.Log(text);
            return false;
        }

        void DisplaySummary()
        {
            string text = $"Job's done.\r\n" +
                          $"Files detected: {this.fullListOfextrData.Count}\r\n" +
                          $"Files processed: {this.noOfFilesProcessed}\r\n";
            msgToLcd?.Invoke(this, text);
            this.logger.Log(text);
        }
    }
}
