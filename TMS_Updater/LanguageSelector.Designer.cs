namespace TMS_Updater
{
    partial class LanguageSelector
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
            this.en = new System.Windows.Forms.Button();
            this.pl = new System.Windows.Forms.Button();
            this.es = new System.Windows.Forms.Button();
            this.ptbr = new System.Windows.Forms.Button();
            this.th = new System.Windows.Forms.Button();
            this.zh = new System.Windows.Forms.Button();
            this.hi = new System.Windows.Forms.Button();
            this.arsa = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // en
            // 
            this.en.Image = global::TMS_Updater.Properties.Resources.en;
            this.en.Location = new System.Drawing.Point(12, 12);
            this.en.Name = "en";
            this.en.Size = new System.Drawing.Size(104, 76);
            this.en.TabIndex = 0;
            this.en.UseVisualStyleBackColor = true;
            this.en.Click += new System.EventHandler(this.En_Click);
            // 
            // pl
            // 
            this.pl.Image = global::TMS_Updater.Properties.Resources.pl;
            this.pl.Location = new System.Drawing.Point(122, 12);
            this.pl.Name = "pl";
            this.pl.Size = new System.Drawing.Size(104, 76);
            this.pl.TabIndex = 1;
            this.pl.UseVisualStyleBackColor = true;
            this.pl.Click += new System.EventHandler(this.Pl_Click);
            // 
            // es
            // 
            this.es.Image = global::TMS_Updater.Properties.Resources.es;
            this.es.Location = new System.Drawing.Point(232, 12);
            this.es.Name = "es";
            this.es.Size = new System.Drawing.Size(104, 76);
            this.es.TabIndex = 2;
            this.es.UseVisualStyleBackColor = true;
            this.es.Click += new System.EventHandler(this.Es_Click);
            // 
            // ptbr
            // 
            this.ptbr.Image = global::TMS_Updater.Properties.Resources.ptbr;
            this.ptbr.Location = new System.Drawing.Point(342, 12);
            this.ptbr.Name = "ptbr";
            this.ptbr.Size = new System.Drawing.Size(104, 76);
            this.ptbr.TabIndex = 3;
            this.ptbr.UseVisualStyleBackColor = true;
            this.ptbr.Click += new System.EventHandler(this.Ptbr_Click);
            // 
            // th
            // 
            this.th.Image = global::TMS_Updater.Properties.Resources.th;
            this.th.Location = new System.Drawing.Point(12, 94);
            this.th.Name = "th";
            this.th.Size = new System.Drawing.Size(104, 76);
            this.th.TabIndex = 4;
            this.th.UseVisualStyleBackColor = true;
            this.th.Click += new System.EventHandler(this.Th_Click);
            // 
            // zh
            // 
            this.zh.Image = global::TMS_Updater.Properties.Resources.zh;
            this.zh.Location = new System.Drawing.Point(122, 94);
            this.zh.Name = "zh";
            this.zh.Size = new System.Drawing.Size(104, 76);
            this.zh.TabIndex = 5;
            this.zh.UseVisualStyleBackColor = true;
            this.zh.Click += new System.EventHandler(this.Zh_Click);
            // 
            // hi
            // 
            this.hi.Image = global::TMS_Updater.Properties.Resources.hi;
            this.hi.Location = new System.Drawing.Point(232, 94);
            this.hi.Name = "hi";
            this.hi.Size = new System.Drawing.Size(104, 76);
            this.hi.TabIndex = 6;
            this.hi.UseVisualStyleBackColor = true;
            this.hi.Click += new System.EventHandler(this.Hi_Click);
            // 
            // arsa
            // 
            this.arsa.Image = global::TMS_Updater.Properties.Resources.arsa;
            this.arsa.Location = new System.Drawing.Point(342, 94);
            this.arsa.Name = "arsa";
            this.arsa.Size = new System.Drawing.Size(104, 76);
            this.arsa.TabIndex = 7;
            this.arsa.UseVisualStyleBackColor = true;
            this.arsa.Click += new System.EventHandler(this.Arsa_Click);
            // 
            // LanguageSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 181);
            this.Controls.Add(this.arsa);
            this.Controls.Add(this.hi);
            this.Controls.Add(this.zh);
            this.Controls.Add(this.th);
            this.Controls.Add(this.ptbr);
            this.Controls.Add(this.es);
            this.Controls.Add(this.pl);
            this.Controls.Add(this.en);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "LanguageSelector";
            this.Text = "TMS Updater - Please select your language";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button en;
        private System.Windows.Forms.Button pl;
        private System.Windows.Forms.Button es;
        private System.Windows.Forms.Button ptbr;
        private System.Windows.Forms.Button th;
        private System.Windows.Forms.Button zh;
        private System.Windows.Forms.Button hi;
        private System.Windows.Forms.Button arsa;
    }
}