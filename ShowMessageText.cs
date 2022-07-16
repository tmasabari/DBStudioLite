using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdvancedQueryOrganizer
{
    public partial class ShowMessageText : Form
    {
        public ShowMessageText()
        {
            InitializeComponent();
        }

        public static void Show(string Message, string Title, IWin32Window owner=null)
        {
            var showMessageText = new ShowMessageText();
            showMessageText.Text = Title;
            showMessageText.textBox1.Text = Message;
            if(owner != null)
                showMessageText.ShowDialog(owner);
            else
                showMessageText.ShowDialog();
        }
    }
}