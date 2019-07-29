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
        LanguageCodesGlossary glossary = new LanguageCodesGlossary();
        LanguageDictionary lng;

        public ZipInjector(string pathToTMS, List<ExtractedData> extractedData, Logger logger, LanguageDictionary lng)
        {
            this.fullListOfextrData = extractedData;
            this.pathToTMS = pathToTMS;
            this.logger = logger;
            this.lng = lng;
        }

        public void Work()
        {
            glossary.Prepare();

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
            List<string> archivesForThisXliff = Directory.GetFiles(this.pathToTMS, currentXliffData.GetJobID() + "_*").ToList();//so, if you have 200000 sdlxliff files as ExtractedData,
                                                                                                                                //then you will be searching through ALL files (potentially hundreds of thousands) in a TMS archive 200000 times? 
                                                                                                                                //that's a performance problem - much bigger than what we discussed about last week in terms of concurrent execution overhead
            if (archivesForThisXliff.Count == 1)
            {
                currentXliffData.ZipNameWithPath = archivesForThisXliff[0];
                return true;
            }
            else
            {
                string text = lng.txt["Error while processing file:"] + "\r\n" +
                                     $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                              lng.txt["Archive for given Job ID could not be found:"] + $" {currentXliffData.GetJobID()}";
                this.logger.Log(text);
                return false;
            }
        }

        string DetermineSubfolderName(ExtractedData currentXliffData)
        {
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
                    currentXliffData.nameAndSubPathOfZippedXliff = "TGT/" + currentXliffData.archiveSubname + "/" + currentXliffData.GetXliffFilename();
                    if (entry.FileName == currentXliffData.nameAndSubPathOfZippedXliff)
                    {
                        return true;
                    }
                }
            }
            string text = lng.txt["Error: It was not possible to find file:"] + "\r\n" +
                          $"{currentXliffData.GetXliffFilename()}\r\n" +
                          lng.txt["inside archive:"] + "\r\n" +
                          $"{currentXliffData.ZipNameWithPath}\r\n" +
                          lng.txt["inside sub-folder:"] + "\r\n" +
                          $"{currentXliffData.archiveSubname}";
            this.logger.Log(text);
            return false;
        }

        bool IsFileInZipOlder(ExtractedData currentXliffData)
        {
            using (ZipFile zip = ZipFile.Read(currentXliffData.ZipNameWithPath)) //so, reading the same zip again, 200000 times? :)
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
            string text = lng.txt["File skipped:"]+"\r\n" +
                          $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                          lng.txt["Incoming file it's older or the same as the file already present in archive."];
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
                            zip.AddFile(currentXliffData.nameAndPathOfNewFile, "TGT\\" + currentXliffData.archiveSubname + "\\");
                            zip.Save();
                            this.noOfFilesProcessed++;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                string text = lng.txt["Error: It was not possible to upload file:"] + "\r\n" +
                              $"{currentXliffData.nameAndPathOfNewFile}\r\n" +
                              lng.txt["into this archive:"] + "\r\n" +
                              $"{currentXliffData.ZipNameWithPath}\r\n" +
                              $"{error.Message}";
                this.logger.Log(text);
            }
        }

        void DisplaySummary()
        {
            string text = lng.txt["Job's done."] +"\r\n" +
                          lng.txt["Files detected:"] + $" {this.fullListOfextrData.Count}\r\n" +
                          lng.txt["Files processed:"] + $" {this.noOfFilesProcessed}\r\n";
            this.logger.Log(text);
        }
    }
}
