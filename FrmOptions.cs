using System;
using System.Windows.Forms;

namespace DBStudioLite
{
    public partial class FrmOptions : Form
    {
        public FrmOptions()
        {
            InitializeComponent();
        }

        private void lblPoolSize_Click(object sender, EventArgs e)
        {

        }

        private void FrmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
