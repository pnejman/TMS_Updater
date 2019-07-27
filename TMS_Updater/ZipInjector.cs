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

        public void Begin() //see other comment about 'Begin' method
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
            List<string> archivesForThisXliff = Directory.GetFiles(this.pathToTMS, currentXliffData.TaskID() + "_*").ToList();//so, if you have 200000 sdlxliff files as ExtractedData,
                                                                                                                              //then you will be searching through ALL files (potentially hundreds of thousands) in a TMS archive 200000 times? 
                                                                                                                              //that's a performance problem - much bigger than what we discussed about last week in terms of concurrent execution overhead

                                                                                                                              //also - Task ID is not interesting to you here, because you need Job ID (a job has many tasks)
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
                this.logger.Log(text);
                return false;
            }
        }

        string DetermineSubfolderName(ExtractedData currentXliffData)
        {
            glossary.Prepare(); //so, building the glossary 200.000 times?:) 

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
                          $"{currentXliffData.ZipNameWithPath}" +
                          $"and subfolder\r\n" +
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
            this.logger.Log(text);
            return false;
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
