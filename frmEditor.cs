using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Reflection;

using System.Data.SqlClient;
using AdvancedQuery;

using WindowsLogic;
using System.Threading.Tasks;
using ScintillaNET;

namespace AdvancedQueryOrganizer
{
    public partial class FrmEditor : Form, IOperatingForm
    {

        public FrmEditor()
        {
            InitializeComponent();
            ConfigureMSSQLSyntax();
            // Enable Context Menu !
            //txtQuery1.EnableContextMenu();
        }

        /// <summary>
        /// https://github.com/jacobslusser/ScintillaNET
        /// https://github.com/jacobslusser/ScintillaNET/wiki/User-Submitted-Recipes
        /// https://gist.github.com/jcouture100/f5d58df816445a1d74df10883618eab4
        /// </summary>
        private void ConfigureMSSQLSyntax()
        {
            // Reset the styles
            txtQuery1.StyleResetDefault();
            txtQuery1.Styles[Style.Default].Font = "Courier New";
            txtQuery1.Styles[Style.Default].Size = 10;
            txtQuery1.StyleClearAll();

            // Set the SQL Lexer
            txtQuery1.Lexer = Lexer.Sql;

            // Show line numbers
            txtQuery1.Margins[0].Width = 20;

            // Set the Styles
            txtQuery1.Styles[Style.LineNumber].ForeColor = Color.FromArgb(255, 128, 128, 128);  //Dark Gray
            txtQuery1.Styles[Style.LineNumber].BackColor = Color.FromArgb(255, 228, 228, 228);  //Light Gray
            txtQuery1.Styles[Style.Sql.Comment].ForeColor = Color.Green;
            txtQuery1.Styles[Style.Sql.CommentLine].ForeColor = Color.Green;
            txtQuery1.Styles[Style.Sql.CommentLineDoc].ForeColor = Color.Green;
            txtQuery1.Styles[Style.Sql.Number].ForeColor = Color.Maroon;
            txtQuery1.Styles[Style.Sql.Word].ForeColor = Color.Blue;
            txtQuery1.Styles[Style.Sql.Word2].ForeColor = Color.Fuchsia;
            txtQuery1.Styles[Style.Sql.User1].ForeColor = Color.Gray;
            txtQuery1.Styles[Style.Sql.User2].ForeColor = Color.FromArgb(255, 00, 128, 192);    //Medium Blue-Green
            txtQuery1.Styles[Style.Sql.String].ForeColor = Color.Red;
            txtQuery1.Styles[Style.Sql.Character].ForeColor = Color.Red;
            txtQuery1.Styles[Style.Sql.Operator].ForeColor = Color.Black;

            // Set keyword lists
            // Word = 0
            txtQuery1.SetKeywords(0, @"add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml go ");
            // Word2 = 1
            txtQuery1.SetKeywords(1, @"ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ");
            // User1 = 4
            txtQuery1.SetKeywords(4, @"all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
            // User2 = 5
            txtQuery1.SetKeywords(5, @"sys objects sysobjects ");
        }
        public bool isFormEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(txtQuery1.Text) && dataGrid1.DataSource == null;
            }
        }

        #region IOperatingFormImplementation
        private string currentFileName;
        public int FillColumn { get; set; } = -1;
        public string FileName
        {
            get { return currentFileName; }
            set 
            {
                currentFileName = value;
                this.Text = Path.GetFileName(value);
                var tabPage = (this.Tag as TabPage);
                if (tabPage != null) tabPage.Text = this.Text;
            }
        }
        public void LoadFromFile()
        {
            if (FileName != "")
            {
                string sFileContents = Common.ReadFile(FileName);
                txtQuery1.Text = sFileContents;
            }
        }

        public void SaveToFile()
        {
            if (FileName != "") Common.WriteFile(FileName, txtQuery1.Text);
        }
        #endregion

        //Execute Procedure

        private void btnExecuteAll_Click(object sender, EventArgs e)
        {
            if (txtQuery1.Text != "") LoadQuery(txtQuery1.Text);
        }

        private void butExecute_Click(object sender, EventArgs e)
        {
            //tabQuery.TabPages[1].Hide();
            string SQuery = "";
            if (txtQuery1.SelectedText == "")
            {
                if(txtQuery1.Text != "")
                {
                    //Start of line
                    int Pos2 = txtQuery1.Text.LastIndexOf("\n", txtQuery1.SelectionStart);
                    if (Pos2 == -1) Pos2 = 0; else Pos2++;
                    //end of line
                    int Pos1 = txtQuery1.Text.IndexOf("\r", txtQuery1.SelectionStart);
                    if (Pos1 == -1) Pos1 = txtQuery1.Text.Length - 1; else Pos1--;
                    
                    SQuery = txtQuery1.Text.Substring(Pos2, Pos1 - Pos2 + 1);
                }
            }
            else
            {
                SQuery = txtQuery1.SelectedText;
            }
            if (SQuery != "") LoadQuery(SQuery, false);
        }

        public async Task LoadQuery(string SQuery, bool IsLoadQueryToBox = true)
        {
            ((MDIAdvancedQuery)MdiParent).ShowProgress();

            string lsConnection =( (MDIAdvancedQuery) this.MdiParent).sConnectionString;
            if(IsLoadQueryToBox) txtQuery1.Text = SQuery;
            using (DynamicDAL DataObj = new DynamicDAL(lsConnection, SQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                txtOutputText.Text = DataObj.SQLInfoMessageBuilder.ToString();
                //task.Wait(); 
                //var ds = task.Result;
                if (ds != null) 
                {
                    dataGrid1.DataSource = null;
                    dataGrid1.Refresh();
                    if (ds.Tables["MyTable"] != null)
                    {
                        dataGrid1.DataSource = ds.Tables["MyTable"];
                        lblRows.Text = "Total Rows :" + ds.Tables["MyTable"].Rows.Count.ToString();
                        dataGrid1.Refresh();
                        checkShowMessages.Checked = false;
                    }
                    else if(txtOutputText.Text.Length > 0)
                    {
                        checkShowMessages.Checked = true;
                    }

                    //TabPage objPage = (TabPage)CloneControl(tabQuery.TabPages[1], ExecuteSequence);
                    ////TabPage objPage = new TabPage("Query" + ExecuteSequence.ToString());
                    //tabQuery.TabPages.Add(objPage);
                    //tabQuery.SelectedIndex = 1;
                }
                else
                {
                    txtOutputText.Text = DataObj.ErrorText;
                    checkShowMessages.Checked = true;
                    //MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                    //(new AdvancedQueryOrganizer.MessageText()).Show(Application.ProductName,
                    //    "Error occured" + DataObj.ErrorText, this);
                }
                ((MDIAdvancedQuery)MdiParent).StopProgress();
            }
        }
        private object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);

            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(p, pi.GetValue(o, null), null);
                }
            }
            return p;
        }
        private Control CloneControl(Control o, int ExecuteSequence)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();

            try
            {
                Control p = (Control)t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);

                foreach (PropertyInfo pi in properties)
                {
                    if (pi.CanWrite)
                    {
                        try { pi.SetValue(p, pi.GetValue(o, null), null); }
                        catch { }
                    }
                }

                foreach (Control child in o.Controls)
                {
                    p.Controls.Add(CloneControl(child, ExecuteSequence));
                }
                return p;
            }
            catch { }
           return null;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (sQueryFileName != "") Common.WriteFile(sQueryFileName, txtQuery.Text);
        }


     private void butLoad_Click(object sender, EventArgs e)
     {
         LoadFromFile();
     }

 



        private void butExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable s = (DataTable)dataGrid1.DataSource;
                if (s != null)
                {
                    string sFile = Application.StartupPath + @"\excel.xml";
                    s.WriteXml(sFile);
                    s.WriteXmlSchema(Application.StartupPath + @"\excel.xsd");
                    Common.OpenWithDefaultProgram(sFile);
                    //System.Diagnostics.Process.Start("excel.exe", "\"" + sFile + "\"");
                }
            }
            catch { }
        }

        private void butGraph_Click(object sender, EventArgs e)
        {
            (new AdvancedQuery.FrmGraph()).Show();
        }

        private void dataGrid1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }


        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void SetDataGrid(object Data)
        {
            dataGrid1.DataSource = null;
            dataGrid1.Refresh();
            dataGrid1.DataSource = Data;
            dataGrid1.Refresh();
            //tabQuery.SelectedIndex = 1;
        }

        public void SetProcedureText(string sText)
        {
            //txtDefinition.Text = sText;
            //tabQuery.SelectedIndex = 2;
            txtQuery1.Text = sText;
            //tabQuery.SelectedIndex = 0;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            txtQuery1.SelectAll();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtQuery1.Text != "") Clipboard.SetText(txtQuery1.Text);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                txtQuery1.SelectAll();   //support undo
                txtQuery1.Paste();
            }
            //txtQuery1.Text =Clipboard.GetText();
            //txtQuery.Text = txtQuery.Text.Insert(txtQuery.SelectionStart, Clipboard.GetText());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtQuery1.SelectAll();   //support undo
            txtQuery1.Focus();
            SendKeys.Send("{DEL}");
        }

        private void checkShowMessages_CheckedChanged(object sender, EventArgs e)
        {
            txtOutputText.Visible = checkShowMessages.Checked;
            checkShowMessages.Text = checkShowMessages.Checked ? "Show Data" : "Show Messages";
        }

        private void dataGrid1_DataError_1(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        //https://github-wiki-see.page/m/jacobslusser/ScintillaNET/wiki/Displaying-Line-Numbers
        private int maxLineNumberCharLength;
        private void txtQuery1_TextChanged(object sender, EventArgs e)
        {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = txtQuery1.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            txtQuery1.Margins[0].Width = txtQuery1.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }
    }
}