using CoreLogic.PluginBase;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32Desktop;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using Style = ScintillaNET.Style;

namespace DBStudioLite
{
    public partial class FrmEditor : Form, IOperatingForm
    {
        private FrmAllDbs frmAllDbs = new FrmAllDbs();

        public FrmEditor()
        {
            InitializeComponent();
            ConfigureMSSQLSyntax();
            ConfigureFindReplace();
            checkShowFullScreen.Checked = true;
            splitCode.SplitterDistance = splitCode.Height / 2;
            // Enable Context Menu !
            //txtQuery.EnableContextMenu();
        }

        #region FindReplaceCustomization

        private FindReplace MyFindReplace = new FindReplace();
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
            txtQuery.Focus();
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
        private void butExecuteAllDBs_Click(object sender, EventArgs e)
        {
            var mdiParent = ((MDIDBStudioLite)this.MdiParent);
            frmAllDbs.RefreshDetails(this, mdiParent.listViewDBs, txtQuery.Text.Trim());
        }

        private void butExecute_Click(object sender, EventArgs e)
        {
            //tabQuery.TabPages[1].Hide();
            string SQuery = "";
            if (txtQuery.SelectedText == "")
            {
                if (txtQuery.Text != "")
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
        public void ShowProgress()
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 100;
            progressBar1.Left = (this.ClientSize.Width - progressBar1.Width) / 2;
            progressBar1.Top = (this.ClientSize.Height - progressBar1.Height) / 2;
            progressBar1.Visible = true;
        }

        public void StopProgress()
        {
            progressBar1.Visible = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
        }
        public async Task LoadQuery(string SQuery, bool IsLoadQueryToBox = true, string dBName = "")
        {
            //clean up the data source at first. otherwise re ordering the columns will not work.
            dataGrid1.DataSource = null;
            dataGrid1.Refresh();
            ShowProgress();
            lblRows.Visible = false;

            string lsConnection = string.Empty;
            var mdiParent = ((MDIDBStudioLite)this.MdiParent);
            if (string.IsNullOrWhiteSpace(dBName))
            {
                lsConnection = mdiParent.sConnectionString;
                dBName = mdiParent.GetSelectedDBName();
            }
            else
            {
                lsConnection = mdiParent.GetConnectionString(dBName);
            }

            if (IsLoadQueryToBox) txtQuery.Text = SQuery;
            string startMessage = $"@{DateTime.Now.ToString("hh:mm:ss FFF")} The execution started for {mdiParent.GetSelectedConnection()}.{dBName}.";
            using (var DataObj = DataAccessFactory.GetDynamicDAL(lsConnection, SQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                string endMessage = $"@{DateTime.Now.ToString("hh:mm:ss FFF")} The execution was completed.";
                dataGrid1.DataSource = (ds != null && ds.Tables["MyTable"] != null) ? ds.Tables["MyTable"] : null;
                dataGrid1.Refresh();
                GridViewHelpers.EnableManualResize(dataGrid1);
                txtOutputText.Font = new System.Drawing.Font("Courier New", 8.25f, System.Drawing.FontStyle.Bold);
                txtOutputText.ForeColor = Color.Black; txtOutputText.BackColor = txtOutputText.BackColor;
                txtOutputText.Text = startMessage + Environment.NewLine;
                var executionMessages = DataObj.SQLInfoMessageBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(executionMessages))
                    txtOutputText.Text += executionMessages; //not required due to appendline + Environment.NewLine;

                //example scenario: exceptions
                if (!string.IsNullOrWhiteSpace(DataObj.ErrorText))
                {
                    txtOutputText.Text += "Error:" + DataObj.ErrorText + Environment.NewLine;
                    txtOutputText.Font = new System.Drawing.Font("Courier New", 12f, System.Drawing.FontStyle.Bold);
                    //https://stackoverflow.com/questions/20688408/how-do-you-change-the-text-color-of-a-readonly-textbox
                    txtOutputText.ForeColor = Color.Red; txtOutputText.BackColor = txtOutputText.BackColor;
                    focusMessages();
                }
                //example scenario: just declare commands DECLARE @PRINT as INT
                else if (string.IsNullOrWhiteSpace(executionMessages) && dataGrid1.DataSource == null)
                {
                    txtOutputText.Text += "There was no output." + Environment.NewLine;
                    focusMessages();
                }
                //example scenario: SELECT statements from query or from sp/modules
                else if (dataGrid1.DataSource != null)
                    FocusDataResults();
                //example scenario: PRINT 'Message'
                else
                    focusMessages();
                txtOutputText.Text += endMessage;
                StopProgress();
            }
        }
        private void focusMessages()
        {
            checkShowFullScreen.Checked = false;
            tabstripResults.SelectedTab = tabMessages;
            dataGrid1.Focus();
        }
        public void FocusDataResults()
        {
            ;
            int rowCount = ((DataTable)dataGrid1.DataSource).Rows.Count;
            if (rowCount == 0)
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
            GridViewHelpers.Export(dataGrid1);
        }

        private void butGraph_Click(object sender, EventArgs e)
        {
            //(new DBStudioLite.FrmGraph()).Show();
        }

        private void dataGrid1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
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