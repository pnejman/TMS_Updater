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

                UpdateZipWithNewFile(currentXliffData);
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
                              $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                              $"Archive for Task ID {currentXliffData.TaskID()} not found.";
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
                    currentXliffData.nameAndSubPathOfZippedXliff = "TGT/" + currentXliffData.archiveSubname + "/" + currentXliffData.XliffFilename();
                    if (entry.FileName == currentXliffData.nameAndSubPathOfZippedXliff)
                    {
                        return true;
                    }
                }
            }
            string text = $"Error: Couldn't find file:\r\n" +
                          $"{currentXliffData.XliffFilename()}\r\n" +
                          $"inside archive\r\n" +
                          $"{currentXliffData.ZipNameWithPath}\r\n" +
                          $"and subfolder\r\n" +
                          $"{currentXliffData.archiveSubname}";
            this.logger.Log(text);
            return false;
        }

        bool IsFileInZipOlder(ExtractedData currentXliffData)
        {
            using (ZipFile zip = ZipFile.Read(currentXliffData.ZipNameWithPath))
            {
                foreach (ZipEntry entry in zip)
                {
                    if (entry.FileName == currentXliffData.nameAndSubPathOfZippedXliff)
                    {
                        DateTime zipFileDate = entry.LastModified;
                        DateTime incomingFileDate = File.GetLastWriteTime(currentXliffData.nameAndPathOfNewFile);
                        if (DateTime.Compare(zipFileDate, incomingFileDate) < -1) //NOTE: date of last modification read from "loose" file and from "zipped" file may differ by about 1 second even if that's the same file
                        {
                            return true;
                        }
                    }
                }
            }
            string text = $"File skipped:\r\n" +
                          $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                          $"because it's older or the same as the file already present in archive.";
            this.logger.Log(text);
            return false;
        }

        void UpdateZipWithNewFile(ExtractedData currentXliffData)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(currentXliffData.ZipNameWithPath))
                {
                    for (int currentEntry = 0; currentEntry < zip.Entries.Count; currentEntry++) //"foreach" loop couses errors due to list re-indexation
                    {
                        if (zip[currentEntry].FileName == currentXliffData.nameAndSubPathOfZippedXliff)
                        {
                            zip.RemoveEntry(zip[currentEntry]);
                            zip.AddFile(currentXliffData.nameAndPathOfNewFile, "TGT\\"+currentXliffData.archiveSubname+"\\");
                            zip.Save();
                            this.noOfFilesProcessed++;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                string text = $"Error: Couldn't upload file:\r\n" +
                              $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                              $"to archive:\r\n" +
                              $"{currentXliffData.ZipNameWithPath}\r\n" +
                              $"{error.Message}";
                this.logger.Log(text);
            }
        }

        void DisplaySummary()
        {
            string text = $"Job's done.\r\n" +
                          $"Files detected: {this.fullListOfextrData.Count}\r\n" +
                          $"Files processed: {this.noOfFilesProcessed}\r\n";
            this.logger.Log(text);
        }
    }
}
