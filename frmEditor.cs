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
using ScintillaNET_FindReplaceDialog;
using ScintillaNET_FindReplaceDialog.FindAllResults;
using AdvancedQueryOrganizer.ClosedXML;

namespace AdvancedQueryOrganizer
{
    public partial class FrmEditor : Form, IOperatingForm
    {

        public FrmEditor()
        {
            InitializeComponent();
            ConfigureMSSQLSyntax();
            ConfigureFindReplace();
            checkShowFullScreen.Checked = true;
            // Enable Context Menu !
            //txtQuery.EnableContextMenu();
        }

        #region FindReplaceCustomization

        private FindReplace MyFindReplace= new FindReplace();
        private void ConfigureFindReplace()
        {
            findAllResultsPanel1.Scintilla = txtQuery;
            MyFindReplace.Scintilla = txtQuery;
            MyFindReplace.FindAllResults += MyFindReplace_FindAllResults;
            MyFindReplace.KeyPressed += MyFindReplace_KeyPressed;
            
            incrementalSearcher1.FindReplace = MyFindReplace;
        }
        private void MyFindReplace_KeyPressed(object sender, KeyEventArgs e)
        {
            txtQuery_KeyDown(sender, e);
        }

        private void MyFindReplace_FindAllResults(object sender, FindResultsEventArgs FindAllResults)
        {
            // Pass on find results
            findAllResultsPanel1.UpdateFindAllResults(FindAllResults.FindReplace, FindAllResults.FindAllResults);
            focusFindResults();
        }

        private void GotoButton_Click(object sender, EventArgs e)
        {
            // Use the FindReplace Scintilla as this will change based on focus
            GoTo MyGoTo = new GoTo(MyFindReplace.Scintilla);
            MyGoTo.ShowGoToDialog();
        }
        /// <summary>
        /// Enter event tied to each Scintilla that will share a FindReplace dialog.
        /// Tie each Scintilla to this event.
        /// </summary>
        /// <param name="sender">The Scintilla receiving focus</param>
        /// <param name="e"></param>
        private void txtQuery_Enter(object sender, EventArgs e)
        {
            MyFindReplace.Scintilla = (Scintilla)sender;
        }

        /// <summary>
        /// Key down event for each Scintilla. Tie each Scintilla to this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                MyFindReplace.ShowFind();
                e.SuppressKeyPress = true;
            }
            else if (e.Shift && e.KeyCode == Keys.F3)
            {
                MyFindReplace.Window.FindPrevious();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                MyFindReplace.Window.FindNext();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.H)
            {
                MyFindReplace.ShowReplace();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                MyFindReplace.ShowIncrementalSearch();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                GoTo MyGoTo = new GoTo((Scintilla)sender);
                MyGoTo.ShowGoToDialog();
                e.SuppressKeyPress = true;
            }
        }
        #endregion


        /// <summary>
        /// https://github.com/jacobslusser/ScintillaNET
        /// https://github.com/jacobslusser/ScintillaNET/wiki/User-Submitted-Recipes
        /// https://gist.github.com/jcouture100/f5d58df816445a1d74df10883618eab4
        /// https://www.scintilla.org/ScintillaDoc.html#LineWrapping
        /// </summary>
        private void ConfigureMSSQLSyntax()
        {
            txtQuery.WrapMode = WrapMode.Word;
            txtQuery.WrapIndentMode = WrapIndentMode.Indent;
            txtQuery.WrapVisualFlags = WrapVisualFlags.End;
            txtQuery.WrapVisualFlagLocation = WrapVisualFlagLocation.Default;

            // Reset the styles
            txtQuery.StyleResetDefault();
            txtQuery.Styles[Style.Default].Font = "Courier New";
            txtQuery.Styles[Style.Default].Size = 10;
            txtQuery.StyleClearAll();

            // Set the SQL Lexer
            txtQuery.Lexer = Lexer.Sql;

            // Show line numbers
            txtQuery.Margins[0].Width = 20;

            // Set the Styles
            txtQuery.Styles[Style.LineNumber].ForeColor = Color.FromArgb(255, 128, 128, 128);  //Dark Gray
            txtQuery.Styles[Style.LineNumber].BackColor = Color.FromArgb(255, 228, 228, 228);  //Light Gray
            txtQuery.Styles[Style.Sql.Comment].ForeColor = Color.Green;
            txtQuery.Styles[Style.Sql.CommentLine].ForeColor = Color.Green;
            txtQuery.Styles[Style.Sql.CommentLineDoc].ForeColor = Color.Green;
            txtQuery.Styles[Style.Sql.Number].ForeColor = Color.Maroon;
            txtQuery.Styles[Style.Sql.Word].ForeColor = Color.Blue;
            txtQuery.Styles[Style.Sql.Word2].ForeColor = Color.Fuchsia;
            txtQuery.Styles[Style.Sql.User1].ForeColor = Color.Gray;
            txtQuery.Styles[Style.Sql.User2].ForeColor = Color.FromArgb(255, 00, 128, 192);    //Medium Blue-Green
            txtQuery.Styles[Style.Sql.String].ForeColor = Color.Red;
            txtQuery.Styles[Style.Sql.Character].ForeColor = Color.Red;
            txtQuery.Styles[Style.Sql.Operator].ForeColor = Color.Black;

            // Set keyword lists
            // Word = 0
            txtQuery.SetKeywords(0, @"add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml go ");
            // Word2 = 1
            txtQuery.SetKeywords(1, @"ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ");
            // User1 = 4
            txtQuery.SetKeywords(4, @"all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ");
            // User2 = 5
            txtQuery.SetKeywords(5, @"sys objects sysobjects ");
        }
        public bool isFormEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(txtQuery.Text) && dataGrid1.DataSource == null;
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
                txtQuery.Text = sFileContents;
            }
        }

        public void SaveToFile()
        {
            if (FileName != "") Common.WriteFile(FileName, txtQuery.Text);
        }
        #endregion

        //Execute Procedure

        private void btnExecuteAll_Click(object sender, EventArgs e)
        {
            if (txtQuery.Text != "") LoadQuery(txtQuery.Text);
        }

        private void butExecute_Click(object sender, EventArgs e)
        {
            //tabQuery.TabPages[1].Hide();
            string SQuery = "";
            if (txtQuery.SelectedText == "")
            {
                if(txtQuery.Text != "")
                {
                    //Start of line
                    int Pos2 = txtQuery.Text.LastIndexOf("\n", txtQuery.SelectionStart);
                    if (Pos2 == -1) Pos2 = 0; else Pos2++;
                    //end of line
                    int Pos1 = txtQuery.Text.IndexOf("\r", txtQuery.SelectionStart);
                    if (Pos1 == -1) Pos1 = txtQuery.Text.Length - 1; else Pos1--;
                    
                    SQuery = txtQuery.Text.Substring(Pos2, Pos1 - Pos2 + 1);
                }
            }
            else
            {
                SQuery = txtQuery.SelectedText;
            }
            if (SQuery != "") LoadQuery(SQuery, false);
        }

        public async Task LoadQuery(string SQuery, bool IsLoadQueryToBox = true)
        {
            ((MDIAdvancedQuery)MdiParent).ShowProgress();
            lblRows.Visible = false;

            string lsConnection =( (MDIAdvancedQuery) this.MdiParent).sConnectionString;
            if(IsLoadQueryToBox) txtQuery.Text = SQuery;
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
                        dataGrid1.Refresh();
                        focusDataResults(ds.Tables["MyTable"].Rows.Count);
                    }
                    else if(txtOutputText.Text.Length > 0)
                    {
                        focusMessages();
                    }

                    //TabPage objPage = (TabPage)CloneControl(tabQuery.TabPages[1], ExecuteSequence);
                    ////TabPage objPage = new TabPage("Query" + ExecuteSequence.ToString());
                    //tabQuery.TabPages.Add(objPage);
                    //tabQuery.SelectedIndex = 1;
                }
                else
                {
                    txtOutputText.Text = DataObj.ErrorText;
                    focusMessages();
                    //MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                    //(new AdvancedQueryOrganizer.MessageText()).Show(Application.ProductName,
                    //    "Error occured" + DataObj.ErrorText, this);
                }
                ((MDIAdvancedQuery)MdiParent).StopProgress();
            }
        }
        private void focusMessages()
        {
            checkShowFullScreen.Checked = false;
            tabstripResults.SelectedTab = tabMessages;
            dataGrid1.Focus();
        }
        private void focusDataResults(int rowCount)
        {
            if(rowCount == 0)
                lblRows.Text = "There are no rows returned.";
            else
                lblRows.Text = "Total Rows :" + rowCount.ToString();
            lblRows.Visible = true;
            checkShowFullScreen.Checked = false;
            tabstripResults.SelectedTab = tabResults;
            txtOutputText.Focus();
        }
        private void focusFindResults()
        {
            checkShowFullScreen.Checked = false;
            tabstripResults.SelectedTab = tabFindResults;
            findAllResultsPanel1.Focus();
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
            DataTable data = (DataTable)dataGrid1.DataSource;
            if (data != null)
            {
                var fileName = ExportToExcel.Export(data);
                //string sFile = Application.StartupPath + @"\excel.xml";
                //s.WriteXml(sFile);
                //s.WriteXmlSchema(Application.StartupPath + @"\excel.xsd");
                Common.OpenWithDefaultProgram(fileName);
                //System.Diagnostics.Process.Start("excel.exe", "\"" + sFile + "\"");
            }
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
            txtQuery.Text = sText;
            //tabQuery.SelectedIndex = 0;
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            txtQuery.SelectAll();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (txtQuery.Text != "") Clipboard.SetText(txtQuery.Text);
        }

        private void Paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                txtQuery.SelectAll();   //support undo
                txtQuery.Paste();
            }
            //txtQuery.Text =Clipboard.GetText();
            //txtQuery.Text = txtQuery.Text.Insert(txtQuery.SelectionStart, Clipboard.GetText());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtQuery.SelectAll();   //support undo
            txtQuery.Focus();
            SendKeys.Send("{DEL}");
        }

        private void checkShowMessages_CheckedChanged(object sender, EventArgs e)
        {
            splitCode.Panel2Collapsed = checkShowFullScreen.Checked;
            checkShowFullScreen.Text = checkShowFullScreen.Checked ? "&Show Results" : "&Show Full Screen";
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
            var maxLineNumberCharLength = txtQuery.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            txtQuery.Margins[0].Width = txtQuery.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }
    }
}