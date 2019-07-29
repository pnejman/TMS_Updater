namespace TMS_Updater
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxPathToSource = new System.Windows.Forms.TextBox();
            this.textBoxPathToTMS = new System.Windows.Forms.TextBox();
            this.labelSourceFolder = new System.Windows.Forms.Label();
            this.labelTMSFolder = new System.Windows.Forms.Label();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.mainDisplay = new System.Windows.Forms.TextBox();
            this.buttonSourceBrowse = new System.Windows.Forms.Button();
            this.buttonTMSBrowse = new System.Windows.Forms.Button();
            this.buttonDictionary = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxPathToSource
            // 
            this.textBoxPathToSource.Location = new System.Drawing.Point(175, 12);
            this.textBoxPathToSource.Name = "textBoxPathToSource";
            this.textBoxPathToSource.Size = new System.Drawing.Size(502, 20);
            this.textBoxPathToSource.TabIndex = 0;
            this.textBoxPathToSource.TextChanged += new System.EventHandler(this.EnableOrDisableZipButton);
            // 
            // textBoxPathToTMS
            // 
            this.textBoxPathToTMS.Location = new System.Drawing.Point(175, 43);
            this.textBoxPathToTMS.Name = "textBoxPathToTMS";
            this.textBoxPathToTMS.Size = new System.Drawing.Size(502, 20);
            this.textBoxPathToTMS.TabIndex = 1;
            this.textBoxPathToTMS.TextChanged += new System.EventHandler(this.EnableOrDisableZipButton);
            // 
            // labelSourceFolder
            // 
            this.labelSourceFolder.AutoSize = true;
            this.labelSourceFolder.Location = new System.Drawing.Point(12, 16);
            this.labelSourceFolder.Name = "labelSourceFolder";
            this.labelSourceFolder.Size = new System.Drawing.Size(107, 13);
            this.labelSourceFolder.TabIndex = 2;
            this.labelSourceFolder.Text = "Folder with fixed files:";
            // 
            // labelTMSFolder
            // 
            this.labelTMSFolder.AutoSize = true;
            this.labelTMSFolder.Location = new System.Drawing.Point(12, 46);
            this.labelTMSFolder.Name = "labelTMSFolder";
            this.labelTMSFolder.Size = new System.Drawing.Size(94, 13);
            this.labelTMSFolder.TabIndex = 3;
            this.labelTMSFolder.Text = "TMS \"jobs\" folder:";
            // 
            // buttonProceed
            // 
            this.buttonProceed.Enabled = false;
            this.buttonProceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonProceed.Location = new System.Drawing.Point(683, 69);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(105, 23);
            this.buttonProceed.TabIndex = 4;
            this.buttonProceed.Text = "Go!";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.ButtonProceed_Click);
            // 
            // mainDisplay
            // 
            this.mainDisplay.Location = new System.Drawing.Point(12, 98);
            this.mainDisplay.Multiline = true;
            this.mainDisplay.Name = "mainDisplay";
            this.mainDisplay.ReadOnly = true;
            this.mainDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mainDisplay.Size = new System.Drawing.Size(776, 537);
            this.mainDisplay.TabIndex = 5;
            // 
            // buttonSourceBrowse
            // 
            this.buttonSourceBrowse.Location = new System.Drawing.Point(683, 10);
            this.buttonSourceBrowse.Name = "buttonSourceBrowse";
            this.buttonSourceBrowse.Size = new System.Drawing.Size(105, 23);
            this.buttonSourceBrowse.TabIndex = 6;
            this.buttonSourceBrowse.Text = "Browse";
            this.buttonSourceBrowse.UseVisualStyleBackColor = true;
            this.buttonSourceBrowse.Click += new System.EventHandler(this.ButtonSourceBrowse_Click);
            // 
            // buttonTMSBrowse
            // 
            this.buttonTMSBrowse.Location = new System.Drawing.Point(683, 41);
            this.buttonTMSBrowse.Name = "buttonTMSBrowse";
            this.buttonTMSBrowse.Size = new System.Drawing.Size(105, 23);
            this.buttonTMSBrowse.TabIndex = 7;
            this.buttonTMSBrowse.Text = "Browse";
            this.buttonTMSBrowse.UseVisualStyleBackColor = true;
            this.buttonTMSBrowse.Click += new System.EventHandler(this.ButtonTMSBrowse_Click);
            // 
            // buttonDictionary
            // 
            this.buttonDictionary.Location = new System.Drawing.Point(585, 69);
            this.buttonDictionary.Name = "buttonDictionary";
            this.buttonDictionary.Size = new System.Drawing.Size(92, 23);
            this.buttonDictionary.TabIndex = 8;
            this.buttonDictionary.Text = "Check Glossary";
            this.buttonDictionary.UseVisualStyleBackColor = true;
            this.buttonDictionary.Click += new System.EventHandler(this.ButtonDictionary_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 647);
            this.Controls.Add(this.buttonDictionary);
            this.Controls.Add(this.buttonTMSBrowse);
            this.Controls.Add(this.buttonSourceBrowse);
            this.Controls.Add(this.mainDisplay);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.labelTMSFolder);
            this.Controls.Add(this.labelSourceFolder);
            this.Controls.Add(this.textBoxPathToTMS);
            this.Controls.Add(this.textBoxPathToSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GUI";
            this.Text = "TMS Uploader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPathToSource;
        private System.Windows.Forms.TextBox textBoxPathToTMS;
        private System.Windows.Forms.Label labelSourceFolder;
        private System.Windows.Forms.Label labelTMSFolder;
        private System.Windows.Forms.Button buttonProceed;
        private System.Windows.Forms.TextBox mainDisplay;
        private System.Windows.Forms.Button buttonSourceBrowse;
        private System.Windows.Forms.Button buttonTMSBrowse;
        private System.Windows.Forms.Button buttonDictionary;
    }
}

