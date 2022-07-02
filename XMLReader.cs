using AdvancedQuery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XML_Reader
{

    public partial class XMLReader : Form
    {
        public XMLReader()
        {
            InitializeComponent();

            object lIndex; //lState,
            (new MyRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"XMLReport", "FileName", out lIndex);

            openFileDialog1.FileName = (string)lIndex;
            textBox1.Text = (string)lIndex;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable s = (DataTable)dataGridView1.DataSource;
                if (s != null)
                {
                    string sFile = Application.StartupPath + @"\excel.xml";
                    s.WriteXml(sFile);
                    s.WriteXmlSchema(Application.StartupPath + @"\excel.xsd");
                    System.Diagnostics.Process.Start("excel.exe", "\"" + sFile + "\"");
                }
            }
            catch { }

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string sFile = openFileDialog1.FileName;
            Refresh(sFile);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh(textBox1.Text);
        }

        private void Refresh(string sFile)
        {
            if (sFile.Length > 0)
            {
                (new MyRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"XMLReport", "FileName", sFile);

                textBox1.Text = sFile;
                DataSet Reports = new DataSet();
                Reports.ReadXml(sFile);
                dataGridView1.DataSource = Reports.Tables[0];

                dataGridView1.Refresh();
                Common.SupportMultipleLineCells(dataGridView1);
                Common.AutoSizeGridView(dataGridView1);

            }
        }
    }
}