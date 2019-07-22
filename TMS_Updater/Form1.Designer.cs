namespace TMS_Updater
{
    partial class Form1
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
            this.textBoxLCD = new System.Windows.Forms.TextBox();
            this.buttonSourceBrowse = new System.Windows.Forms.Button();
            this.buttonTMSBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxPathToSource
            // 
            this.textBoxPathToSource.Location = new System.Drawing.Point(140, 12);
            this.textBoxPathToSource.Name = "textBoxPathToSource";
            this.textBoxPathToSource.Size = new System.Drawing.Size(566, 20);
            this.textBoxPathToSource.TabIndex = 0;
            this.textBoxPathToSource.TextChanged += new System.EventHandler(this.EnableOrDisableZipButton);
            // 
            // textBoxPathToTMS
            // 
            this.textBoxPathToTMS.Location = new System.Drawing.Point(140, 43);
            this.textBoxPathToTMS.Name = "textBoxPathToTMS";
            this.textBoxPathToTMS.Size = new System.Drawing.Size(566, 20);
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
            this.labelTMSFolder.Size = new System.Drawing.Size(125, 13);
            this.labelTMSFolder.TabIndex = 3;
            this.labelTMSFolder.Text = "Folder with TMS archive:";
            // 
            // buttonProceed
            // 
            this.buttonProceed.Enabled = false;
            this.buttonProceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonProceed.Location = new System.Drawing.Point(713, 69);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 4;
            this.buttonProceed.Text = "Zip it!";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.ButtonProceed_Click);
            // 
            // textBoxLCD
            // 
            this.textBoxLCD.Location = new System.Drawing.Point(12, 98);
            this.textBoxLCD.Multiline = true;
            this.textBoxLCD.Name = "textBoxLCD";
            this.textBoxLCD.ReadOnly = true;
            this.textBoxLCD.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLCD.Size = new System.Drawing.Size(776, 537);
            this.textBoxLCD.TabIndex = 5;
            // 
            // buttonSourceBrowse
            // 
            this.buttonSourceBrowse.Location = new System.Drawing.Point(713, 10);
            this.buttonSourceBrowse.Name = "buttonSourceBrowse";
            this.buttonSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonSourceBrowse.TabIndex = 6;
            this.buttonSourceBrowse.Text = "Browse";
            this.buttonSourceBrowse.UseVisualStyleBackColor = true;
            this.buttonSourceBrowse.Click += new System.EventHandler(this.ButtonSourceBrowse_Click);
            // 
            // buttonTMSBrowse
            // 
            this.buttonTMSBrowse.Location = new System.Drawing.Point(713, 41);
            this.buttonTMSBrowse.Name = "buttonTMSBrowse";
            this.buttonTMSBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonTMSBrowse.TabIndex = 7;
            this.buttonTMSBrowse.Text = "Browse";
            this.buttonTMSBrowse.UseVisualStyleBackColor = true;
            this.buttonTMSBrowse.Click += new System.EventHandler(this.ButtonTMSBrowse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 647);
            this.Controls.Add(this.buttonTMSBrowse);
            this.Controls.Add(this.buttonSourceBrowse);
            this.Controls.Add(this.textBoxLCD);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.labelTMSFolder);
            this.Controls.Add(this.labelSourceFolder);
            this.Controls.Add(this.textBoxPathToTMS);
            this.Controls.Add(this.textBoxPathToSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
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
        private System.Windows.Forms.TextBox textBoxLCD;
        private System.Windows.Forms.Button buttonSourceBrowse;
        private System.Windows.Forms.Button buttonTMSBrowse;
    }
}

