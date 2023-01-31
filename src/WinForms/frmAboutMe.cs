using System;
using System.IO;
using System.Windows.Forms;

namespace DBStudioLite
{
    public partial class frmAboutMe : Form
    {
        public frmAboutMe()
        {
            InitializeComponent();
        }

        private void frmAboutMe_Load(object sender, EventArgs e)
        {
            rtbContents.LoadFile(Path.Combine(Application.StartupPath, "DBStudioLite.rtf"));
            rtbContents.Rtf = rtbContents.Rtf.Replace("<Year/>", DateTime.Now.Year.ToString());
        }

        //https://stackoverflow.com/questions/435607/how-can-i-make-a-hyperlink-work-in-a-richtextbox
        private void rtbContents_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
    }


}