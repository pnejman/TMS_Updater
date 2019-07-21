using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace TMS_Updater
{
    public class GeneralFilesProcessor
    {
        public event EventHandler<string> msgToLcd;

        public void Begin(string pathToSource, string pathToTMS)
        {
            List<string> allFizedXLIFFS = Directory.GetFiles(pathToSource, "*.sdlxliff", SearchOption.AllDirectories).ToList();

            if (allFizedXLIFFS.Count() == 0)
            {
                msgToLcd?.Invoke(this, $"Error: No SDLXLIFF files found in given directory.\r\n");
                return;
            }

            msgToLcd?.Invoke(this, $"{allFizedXLIFFS.Count()} SDLXLIFF files detected.\r\n");

            List<SingleFileProcessor> singleFileProcessors = new List<SingleFileProcessor>();

            Random r = new Random();

            foreach (var file in allFizedXLIFFS)
            {
                var newlyCreatedProcessor = new SingleFileProcessor(file, pathToTMS);

                
                int a = r.Next(10)+5;

                singleFileProcessors.Add(newlyCreatedProcessor);
                newlyCreatedProcessor.msgToLcd += PassMsg;
                new Thread(delegate () { newlyCreatedProcessor.Begin(a); }).Start();
            }
        }

        private void PassMsg(object sender, string msgToPass)
        {
            msgToLcd?.Invoke(this, msgToPass);
        }
    }
}
