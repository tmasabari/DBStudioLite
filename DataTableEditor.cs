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
using SmartQueryRunner;

namespace WindowsLogic
{

    public partial class DataTableEditor : Form, IOperatingForm
    {
        private FormTypes FormType;

        public DataTableEditor(FormTypes formType, string initialFile = "")
        {
            InitializeComponent();
            FormType = formType;
            textBox1.Text = initialFile;
        }
        //private void LoadLastUsedFile()
        //{
        //    if (initialFile == "")
        //    {
        //        object lIndex; //lState,
        //        (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"XMLReport", "FileName", out lIndex);
        //        (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"JsonFile", "FileName", out lIndex);
        //        initialFile = (string)lIndex;
        //    }
        //}

        #region IOperatingFormImplementation
        private string currentFileName;
        public int FillColumn { get; set; } = -1;

        public string FileName
        {
            get { return currentFileName; }
            set
            {
                currentFileName = value;
                this.Text = value;
                var tabPage = (this.Tag as TabPage);
                if (tabPage != null) tabPage.Text = value;
            }
        }
        public void LoadFromFile()
        {
            if (FileName != "")
            {
                RefreshTable(FileName);
            }
        }
        public void RefreshTable(string sFile)
        {
            try
            {
                if (sFile.Length > 0)
                {
                    //(new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"XMLReport", "FileName", sFile);
                    //(new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"JsonFile", "FileName", sFile);

                    textBox1.Text = sFile;
                    if(FormType == FormTypes.JsonEditor)
                        dataGridView1.DataSource = Common.LoadJsonToTable(sFile);
                    else
                        dataGridView1.DataSource = XMLDAL.GetFirstTableFromXMLFile(sFile);

                    dataGridView1.Refresh();
                    Common.SupportMultipleLineCells(dataGridView1);
                    if (FillColumn > -1) Common.AutoSizeGridView(dataGridView1, FillColumn);
                }
            }
            catch (Exception ex)
            {
                ShowMessageText.Show("Unexpected error occurred " + Environment.NewLine + ex.ToString(), "Load Error");
            }
        }

        public void SaveToFile()
        {
            try
            {
                DataTable dataTable = (DataTable)dataGridView1.DataSource;
                if (dataTable != null)
                {
                    string sFile= textBox1.Text;
                    if (FormType == FormTypes.JsonEditor)
                        Common.SaveTableToJson(sFile, dataTable);
                    else
                        XMLDAL.SaveToFile(sFile, dataTable);
                    MessageBox.Show("The data saved to " + sFile);
                }
            }
            catch (Exception ex)
            {
                ShowMessageText.Show("Unexpected error occurred " + Environment.NewLine + ex.ToString(), "Save Error");
            }
        }
        #endregion
    }
}