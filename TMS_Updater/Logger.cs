using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TMS_Updater
{
    public class Logger
    {
        string logFileName;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TMS Updater Logs\\";

        public Logger()
        {
            this.logFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_log.txt";
            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path + this.logFileName,
                               $"This is TMS Updater log.\r\n" +
                               $"(Tool for batch updating SDLXLIFF files on TMS.)\r\n" +
                               $"Created {DateTime.Now.ToString("yyyy-MM-dd, HH:mm:ss")}.\r\n\r\n");
        }

        public void Log(string textToLog)
        {
            File.AppendAllText(path + this.logFileName, DateTime.Now.ToString("HH:mm:ss") + ": " + textToLog + "\r\n");
        }
    }
}
