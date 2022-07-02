using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartQueryRunner
{
    public partial class frmAboutMe : Form
    {
        public frmAboutMe()
        {
            InitializeComponent();
            //webBrowser1.Navigate("http://soft.sabarinathanarthanari.com/Query/update");
            webBrowser1.Navigate("http://localhost:10005/soft/Query/update");
        }

        private void frmAboutMe_Load(object sender, EventArgs e)
        {
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string[] sep = { "||" };
            string sWindowValue = webBrowser1.FormField("WindowData");
            string[] sWindowValues = sWindowValue.Split(sep,StringSplitOptions.None);

            this.SetBounds(int.Parse(sWindowValues[0]), int.Parse(sWindowValues[1]),
                    int.Parse(sWindowValues[2]), int.Parse(sWindowValues[3]), BoundsSpecified.All);
            this.Show();
        }
    }


}