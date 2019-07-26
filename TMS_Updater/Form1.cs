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
        DataExtractor dataExtractor = new DataExtractor();
        Logger logger;

        public Form1()
        {
            InitializeComponent();
            textBoxPathToSource.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            textBoxPathToTMS.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        private void ButtonProceed_Click(object sender, EventArgs e)
        {
            DisableGui();

            try
            {
                this.logger = new Logger();
                this.logger.passMsgToDisplay += OnIncomingTextToDisplay;
                OnIncomingTextToDisplay(this, $"Log file created at \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".");
            }
            catch (Exception error)
            {
                OnIncomingTextToDisplay(this, $"Warning: Failed to create log file at \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".\r\n" +
                                              $"{error.Message}");
                return;
            }

            dataExtractor.Begin(textBoxPathToSource.Text, textBoxPathToTMS.Text, this.logger);
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
            buttonDictionary.Enabled = false;
        }

        private void OnIncomingTextToDisplay(object sender, string msg)
        {
            textBoxLCD.Invoke(new Action (() => textBoxLCD.AppendText("* " + msg +"\r\n\r\n")));
        }

        private void ButtonDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("glossary.txt");
            }
            catch
            {
                MessageBox.Show("Glossary file currently unavailable.\r\nIt will be generated automatically during program run.");
            }
        }
    }
}
