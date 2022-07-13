using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AdvancedQuery;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace WindowsLogic
{

    public partial class JsonReader : Form
    {
        public JsonReader(string initialFile = null)
        {
            InitializeComponent();

            if(initialFile == null)
            {
                object lIndex; //lState,
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"JsonFile", "FileName", out lIndex);
                initialFile = (string)lIndex;
            }

            openFileDialog1.FileName = initialFile;
            textBox1.Text = initialFile;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;
                if (dt != null)
                {
                    string sFile;
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                        sFile = Application.StartupPath + @"\exported.json";
                    else 
                        sFile = textBox1.Text;
                    Common.SaveTableToJson(sFile, dt);
                    //System.Diagnostics.Process.Start("notepad.exe", "\"" + sFile + "\"");
                }
            }
            catch { }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string sFile = openFileDialog1.FileName;
            textBox1.Text = sFile;
            (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"JsonFile", "FileName", sFile);
            RefreshJsonTable(sFile);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshJsonTable(textBox1.Text);
        }

        public void RefreshJsonTable(string sFile, int fillColumn = -1 )
        {
            if (sFile.Length > 0)
            {
                dataGridView1.DataSource = Common.LoadJsonToTable(sFile);
                dataGridView1.Refresh();
                Common.SupportMultipleLineCells(dataGridView1);
                if (fillColumn > -1) Common.AutoSizeGridView(dataGridView1, fillColumn);
            }
        }

    }


}