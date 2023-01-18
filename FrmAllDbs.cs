using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Azure.Core.HttpHeader;

namespace DBStudioLite
{
    public partial class FrmAllDbs : Form
    {
        private FrmEditor frmEditor;
        private int executeAllDbsIndex = 0;
        private List<string> dbNames;
        private string Query;
        private string StartDBName;

        public FrmAllDbs()
        {
            InitializeComponent();
        }

        private void FrmAllDbs_Activated(object sender, EventArgs e)
        {

        }

        public void RefreshDetails(FrmEditor owner, ListView listViewDbs, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Query is empty. Nothing to execute", this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Query = query;
            dbNames = new List<string>();
            foreach (ListViewItem item in listViewDbs.Items)
            {
                if (item.SubItems[1].Text != "True")
                    dbNames.Add(item.SubItems[0].Text);
            }
            //if there are no dbs or query is null do nothing
            if (dbNames.Count == 0)
            {
                MessageBox.Show("The applicable database list is empty.", this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            executeAllDbsIndex = 0;
            StartDBName = dbNames[0]; //first db name
            butRestart.Text = "Begin/Restart@ " + StartDBName;
            frmEditor = owner;
            //this.Parent = Parent; //top level control cannot be added
            setButtons();
            this.ShowDialog(owner); //this sets to MDIform automatically
        }   

        private void butCancel_Click(object sender, EventArgs e)
        {
            executeAllDbsIndex = 0;
            this.Hide();
        }

        private void butRestart_Click(object sender, EventArgs e)
        {
            executeAllDbsIndex = 0;
            Execute();
        }

        private void Execute()
        {
            frmEditor.LoadQuery(Query, IsLoadQueryToBox: true, dbNames[executeAllDbsIndex]);
            CalculateNextIndex(executeAllDbsIndex);
            setButtons();
        }
        private void setButtons()
        {
            //https://stackoverflow.com/questions/11161160/c-sharp-usercontrol-visible-property-not-changing
            //you can still set Visible properties - they just won't take effect until the Form.Visible property is set to true.
            bool butVisible = (dbNames.Count > 2) && executeAllDbsIndex > 0;
            if (butVisible) butExecute.Text = "Execute@ " + dbNames[executeAllDbsIndex];
            butExecute.Visible = butVisible;
            butVisible = (dbNames.Count > 1) && (executeAllDbsIndex < dbNames.Count - 1);
            if (butVisible) butSkip.Text = "Skip && execute@ " + dbNames[executeAllDbsIndex + 1];
            butSkip.Visible = butVisible;

            //https://stackoverflow.com/questions/2022660/how-to-get-the-size-of-a-winforms-form-titlebar-height
            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;
            this.Height = titleHeight + flowLayoutPanel1.Top + flowLayoutPanel1.Height + 10;
        }

        private void CalculateNextIndex(int index)
        {
            executeAllDbsIndex= index +1;
            //if current db pointer is not valid reset to 0
            if (executeAllDbsIndex >= dbNames.Count) executeAllDbsIndex = 0;
        }

        private void butSkip_Click(object sender, EventArgs e)
        {
            CalculateNextIndex(executeAllDbsIndex);
            Execute();
        }

        private void butExecute_Click(object sender, EventArgs e)
        {
            Execute();
        }
    }
}
