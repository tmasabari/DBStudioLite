using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartQueryRunner
{
    public partial class MessageText : Form
    {
        public MessageText()
        {
            InitializeComponent();
        }

        public void Show(string Title, string Message, IWin32Window owner)
        {
            this.Text = Title;
            textBox1.Text = Message;
            this.Show(owner);
            //MessageBox.Show(owner, Message, Title);
        }
    }
}