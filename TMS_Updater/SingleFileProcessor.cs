using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TMS_Updater
{
    class SingleFileProcessor
    {
        public event EventHandler<string> msgToLcd;

        string filePath;
        string pathToTMS;

        public SingleFileProcessor(string filePath, string pathToTMS)
        {
            this.filePath = filePath;
            this.pathToTMS = pathToTMS;
        }

        public void Begin(int a)
        {
            while (true)
            {
                msgToLcd.Invoke(this, $"{a.ToString()} ");

                Thread.Sleep(100*a);
            }

        }
    }
}
