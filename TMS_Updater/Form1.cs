using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TMS_Updater
{
    public partial class Form1 : Form
    {
        DataExtractor filesProcessor = new DataExtractor();
        
        public Form1()
        {
            InitializeComponent();
            filesProcessor.msgToLcd += OnLCDDisplay;
            textBoxPathToSource.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            textBoxPathToTMS.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private void ButtonProceed_Click(object sender, EventArgs e)
        {
            DisableGui();
            filesProcessor.Begin(textBoxPathToSource.Text, textBoxPathToTMS.Text);
        }

        private void ButtonSourceBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxPathToSource.Text = fbd.SelectedPath;
            }

        }

        private void ButtonTMSBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBoxPathToTMS.Text = fbd.SelectedPath;
            }
        }

        private void EnableOrDisableZipButton(object sender, EventArgs e)
        {
            if ((textBoxPathToSource.Text != "")&&(textBoxPathToTMS.Text != ""))
            {
                buttonProceed.Enabled = true;
            }
            else
            {
                buttonProceed.Enabled = false;
            }
        }

        private void DisableGui()
        {
            buttonProceed.Enabled = false;
            textBoxPathToSource.Enabled = false;
            textBoxPathToTMS.Enabled = false;
            buttonSourceBrowse.Enabled = false;
            buttonTMSBrowse.Enabled = false;
        }

        private void OnLCDDisplay(object sender, string msg)
        {
            textBoxLCD.Invoke(new Action (() => textBoxLCD.AppendText(msg+"\r\n")));
        }
    }
}
