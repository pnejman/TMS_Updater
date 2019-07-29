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
    public partial class GUI : Form
    {
        DataExtractor dataExtractor = new DataExtractor(); //nicely encapsulated functionality
        Logger logger;
        LanguageDictionary lng;

        public GUI(string language)
        {
            InitializeComponent();
            this.lng = new LanguageDictionary(language);
            textBoxPathToSource.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            textBoxPathToTMS.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            labelSourceFolder.Text = lng.txt["Folder with new files:"];
            labelTMSFolder.Text = lng.txt["TMS folder with ZIP files:"];
            buttonSourceBrowse.Text = lng.txt["Browse"];
            buttonTMSBrowse.Text = lng.txt["Browse"];
            buttonProceed.Text = lng.txt["Go!"];

            if (language == "arsa")
            {
                textBoxPathToSource.RightToLeft = RightToLeft.Yes;
                textBoxPathToTMS.RightToLeft = RightToLeft.Yes;
                labelSourceFolder.RightToLeft = RightToLeft.Yes;
                labelTMSFolder.RightToLeft = RightToLeft.Yes;
                buttonSourceBrowse.RightToLeft = RightToLeft.Yes;
                buttonTMSBrowse.RightToLeft = RightToLeft.Yes;
                buttonProceed.RightToLeft = RightToLeft.Yes;
                mainDisplay.RightToLeft = RightToLeft.Yes;
            }
        }

        private void ButtonProceed_Click(object sender, EventArgs e)
        {
            this.DisableGui();

            try
            {
                this.logger = new Logger(lng);
                this.logger.passMsgToDisplay += OnIncomingTextToDisplay;
                OnIncomingTextToDisplay(this, this.lng.txt["Log file created at this location:"] + $" \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".");
            }
            catch (Exception error)
            {
                OnIncomingTextToDisplay(this, this.lng.txt["Error: Failed to create log file at this location:"] + $" \"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\TMS Updater Logs\".\r\n" +
                                              $"{error.Message}");
                return;
            }

            dataExtractor.Work(textBoxPathToSource.Text, textBoxPathToTMS.Text, this.logger, this.lng);
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
                textBoxPathToTMS.Text = fbd.SelectedPath; //as far as I understand from the label in GUI, this expects the path to the jobs folder.
                //this is not good, because jobs is a subfolder of an organization, and you don't know into which organization you're dropping stuff (as there are multiple organizations)
            }
        }

        private void EnableOrDisableZipButton(object sender, EventArgs e)
        {
            if ((textBoxPathToSource.Text != "") && (textBoxPathToTMS.Text != ""))
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
            mainDisplay.Invoke(new Action(() => mainDisplay.AppendText("* " + msg + "\r\n\r\n")));
        }

        private void ButtonDictionary_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("glossary.txt");
            }
            catch
            {
                MessageBox.Show(lng.txt["Glossary file currently unavailable. It will be generated automatically during program run."]);
            }
        }
    }
}
