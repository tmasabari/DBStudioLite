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
using xmlDbEditor;
using System.Threading.Tasks;
using ScintillaNET;

namespace SmartQueryRunner
{
    public partial class FrmEditor : Form
    {
        private string psQueryFileName;

        public FrmEditor(string FileName =  "")
        {
            InitializeComponent();
            QueryFileName = FileName; 
            LoadFromFile();

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

        public string QueryFileName
        {
            get { return psQueryFileName; }
            set 
            {
                psQueryFileName = value;
                this.Text = value;
            }
        }

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
                    //(new SmartQueryRunner.MessageText()).Show(Application.ProductName,
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

     public void LoadFromFile()
     {
         if (QueryFileName != "")
         {
             string sFileContents = Common.ReadFile(QueryFileName);
             txtQuery1.Text = sFileContents;
         }
     }

     public void SaveToFile()
     {
         if (QueryFileName != "") Common.WriteFile(QueryFileName, txtQuery1.Text);
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

        //Now, let's turn to another routine that I'll need to complete the search: GetTableDetails (see Figure 4). This routine uses the SQL query shown here to retrieve the details of a particular table, including field names, datatype, and maximum length: 
        // SQL = "select '" & TableName & "' as TableName, " _
        //    SQL &= "COLUMN_NAME, DATA_TYPE, " _
        //    SQL &= "CHARACTER_MAXIMUM_LENGTH  from " _
        //    SQL &= "INFORMATION_SCHEMA.COLUMNS where " _
        //    SQL &= "table_name = '" & TableName & "' "

        //GetTableDetails is called when the user clicks the Search button (called cmdSearch). More on this soon.

        //One more function that I need to look at is CheckColumnIdentity. 
        //Function CheckColumnIdentity(ByVal TableName As String, _
        //    ByVal ColumnName As String) As Boolean
        //    Dim SQL As String
        //    SQL = "SELECT COLUMNPROPERTY( OBJECT_ID('" & " _
        //    SQL &= "TableName & "'),'" & ColumnName & _
        //    SQL &= "','IsIdentity')"
        //    If CInt(RunSQLScalar(SQL)) = 1 Then
        //        Return True
        //    Else
        //        Return False
        //    End If
        //End Function
        //This function takes a table name and column name as parameters and returns a Boolean value, indicating whether the column is an identity column. It uses COLUMNPROPERTY to determine if the column is an identity column. You can also use this property to determine if a column allows nulls, and so on.

        //Now, let's put this together by looking at the cmdSearch Click event code. The first few lines of code define the variables I'll need: 
        //Dim dt, dttemp As DataTable
        //Dim ds As New DataSet
        //Dim tabletosearch As String
        //Dim rw As DataRow
        //Dim IsMatch As Boolean = False

        //The following two lines set tabletosearch to the name of the table to search and clears the output textbox: 
        //tabletosearch = cboTables.Text
        //txtOutput.ResetText()

        //The next few lines control how the search is handled. If chkAllTables is True, then all of the details for each table are loaded into the dt table. If chkAllTables is False, then dt is loaded with the details for only the selected table: 
        //If chkAllTables.Checked Then
        //    For Each rw In dsTables.Tables(0).Rows
        //        dttemp = GetTableDetails(rw("Name").ToString)
        //        ds.Merge(dttemp)
        //    Next
        //    dt = ds.Tables(0)
        //Else
        //    dt = GetTableDetails(tabletosearch)
        //End If

        //Now that the tables are loaded, I can perform the search. The search is handled inside the For Each loop, which moves through all rows in the dt table: 
        //For Each rw In dt.Rows
        //The next line sets IsMatch to False as the default for each field. IsMatch is a flag that determines whether to output the field: 
        //IsMatch = False

        //Next, if chkContains is True, then InStr is used to determine if a match exists. If chkContains is False, then the Else clause checks for an exact match. If a match is found, IsMatch is set to True: 
        //If chkContains.Checked Then
        //    If InStr(UCase(rw("ColumnName").ToString), _
        //        UCase(txtSearchField.Text)) > 0 Then
        //        IsMatch = True
        //    End If
        //Else
        //    If UCase(rw("ColumnName").ToString) = UCase(txtSearchField.Text) Then
        //        IsMatch = True
        //    End If
        //End If
        //Now that the test is complete, I can output the data if IsMatch is True. The values in the various fields in the datatable are output to txtOutput, as shown here: 
        //If IsMatch Then
        //    txtOutput.Text &= rw("TableName").ToString _
        //        & " : " & rw("ColumnName").ToString & " - "
        //    txtOutput.Text &= rw("DataType").ToString _
        //        & " (" & rw("Length").ToString & ")"

        //    If CBool(rw("Identity").ToString) Then
        //        txtOutput.Text &= " Identity"
        //End If
        //        txtOutput.Text &= vbCrLf
        //    End If
        //Next
        //That's it. There is not a lot of code to this tool, but it sure is handy.

    }
}