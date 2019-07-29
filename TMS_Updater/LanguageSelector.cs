using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMS_Updater
{
    public partial class LanguageSelector : Form
    {
        public LanguageSelector()
        {
            InitializeComponent();
        }

        private void En_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("en");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Pl_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("pl");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Es_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("es");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Ptbr_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("ptbr");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Th_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("th");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Zh_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("zh");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Hi_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("hi");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }

        private void Arsa_Click(object sender, EventArgs e)
        {
            GUI gui = new GUI("arsa");
            this.Hide();
            gui.ShowDialog();
            this.Close();
        }
    }
}
