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

using XML_Reader;
using xmlDbEditor;
using System.Threading.Tasks;

//command line "H:\web\Order online\App_Data\Paypal.xml"
namespace SmartQueryRunner
{
    public partial class MDIAdvancedQuery : Form
    {
        private const int colName = 0;
        private const int colSchema = 1;
        private const int colType = 2;
        private int QueryWindowNumber = 1;

        private string sStandardConnectionParams
        {
            get
            {
                return " ;Application Name=AdvancedQuery;Min Pool Size=1;Max Pool Size=" + txtPool.Text + ";Connection Timeout=" + txtConnectionTimeout.Text + ";" +
                    "Persist Security Info=False; MultipleActiveResultSets=True;Packet Size=" + txtPockets.Text + ";";
            }
        }
        private string DBParameter
        {
            get
            {
                return ";initial catalog=" + GetSelectedDBName();
            }
        }
        private string DBParameterMaster
        {
            get
            {
                return ";initial catalog=master";
            }
        }
        public string sConnectionString
        {
            get
            {
                return objConnections.ConnectionText + sStandardConnectionParams + DBParameter;
            }
        }

        public string sMasterConnectionString
        {
            get
            {
                return objConnections.ConnectionText + sStandardConnectionParams + DBParameterMaster;
            }
        }

        private bool bFirstTime = true;
        public frmConnections objConnections = new frmConnections();
        
        private string snippetFilePath = Application.StartupPath + "\\Snippets.json";
        private DataTable dtDatabases = new DataTable();
        private DataTable dtTablesViews = new DataTable();
        private DataTable dtProcedures = new DataTable();
        private DataTable dtSnippetsTable = new DataTable();
        private JsonReader snippetsEditor;

        public MDIAdvancedQuery()
        {
            InitializeComponent();
            objConnections.ConnectionChanged += ObjConnections_ConnectionChanged;
            object lIndex; //lState,
            (new MyRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", out lIndex);
            //(new MyRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "CheckState", out lState);
            if (lIndex == null) objConnections.SelectedIndex = 0; else objConnections.SelectedIndex = int.Parse(lIndex.ToString());
            //if (lState == null) chkConnection.Checked = true; else chkConnection.Checked = bool.Parse(lState.ToString());
        }

        private void ObjConnections_ConnectionChanged(object sender, EventArgs e)
        {
            currentConnectionToolStripMenuItem.Text = "Connections [" + objConnections.ConnectionName + "]";
        }

        private FrmEditor GetOperatingForm(bool CreateNew=true)
        {
            if (this.ActiveMdiChild is FrmEditor)
                return ((FrmEditor)this.ActiveMdiChild);
            else
            {
                if (CreateNew)
                    return CreateNewForm("");
                else
                    return null;
            }
        }
        private FrmEditor GetEmptyOperatingForm()
        {
            //if (this.ActiveMdiChild is FrmMain)
            //{
            //    FrmMain objChild = (FrmMain)this.ActiveMdiChild;
            //    if (objChild.isFormEmpty)
            //        return ((FrmMain)this.ActiveMdiChild);
            //}
            return CreateNewForm("");
        }

        private FrmEditor CreateNewForm(string sFileName)
        {
            // Create a new instance of the child form.
            FrmEditor childForm = new FrmEditor(sFileName);
            childForm.Show();
            // Make it a child of this MDI form before showing it.
            if (string.IsNullOrWhiteSpace( sFileName)) childForm.Text = "Untitled " + QueryWindowNumber.ToString();
            //set all properties before setting mdiparent this addes the tab to tab control.
            childForm.MdiParent = this;

            QueryWindowNumber++;

            return childForm;
        }

        private async Task RefreshDBs()
        {
            listViewDBs.Clear();
            listViewDBs.Columns.Add("Name", 200, HorizontalAlignment.Left);
            //listViewDBs.Columns.Add("Created", 200, HorizontalAlignment.Left);
            string sQuery = "SELECT name,  create_date FROM sys.databases"; //database_id,
            using (DataBaseProcedure DataObj = new DataBaseProcedure(sMasterConnectionString, sQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                //txtOutputText.Text = DataObj.SQLInfoMessageBuilder.ToString();
                if (ds != null)
                {
                    if (ds.Tables["MyTable"] != null)
                    {
                        dtDatabases = ds.Tables["MyTable"];
                        foreach (DataRow db in dtDatabases.Rows)
                        {
                            ListViewItem lvItem = new ListViewItem((string)db[0]);   // Create the ListViewItem row with the first column.
                            lvItem.SubItems.Add((string)db[0]);
                            //lvItem.SubItems.Add(db[1].ToString());
                            listViewDBs.Items.Add(lvItem);                                // Add the completed row.
                        }
                    }
                }
            }
        }


        private void RefreshSnippets()
        {
            dtSnippetsTable = Common.LoadJsonToTable(snippetFilePath);

            listViewSnippets.Clear();
            listViewSnippets.Columns.Add("Snippet", 200, HorizontalAlignment.Left);
            //listViewSnippets.Columns.Add("IsExecutable", 0, HorizontalAlignment.Left);
            foreach (DataRow snippet in dtSnippetsTable.Rows)
            {
                ListViewItem lvItem = new ListViewItem((string)snippet[0]);   // Create the ListViewItem row with the first column.
                lvItem.SubItems.Add((string)snippet[0]);
                listViewSnippets.Items.Add(lvItem);                                // Add the completed row.
            }
        }
        private void MDIParent1_Activated(object sender, EventArgs e)
        {
            if (bFirstTime)
            {
                bFirstTime = false;

                RefreshDBs();
                RefreshSnippets();

                FrmEditor childForm = CreateNewForm(Application.StartupPath + "\\Query.txt");
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();

                if (objConnections.ConnectionList.Items.Count == 0)
                    objConnections.ShowDialog(this);

                RefreshConnectionsMenu();
            }
        }

        private void RefreshConnectionsMenu()
        {
            while (currentConnectionToolStripMenuItem.DropDownItems.Count > 2)
                currentConnectionToolStripMenuItem.DropDownItems.RemoveAt(2);
            //contextConnections.Items.Clear();

            for (int i = 0; i < objConnections.sConnectionCaptions.Count; i++)
            {
                ToolStripMenuItem childmenu = new ToolStripMenuItem();
                childmenu.Text = (string)objConnections.sConnectionCaptions[i];
                childmenu.Tag = i;
                //contextConnections.Items.Add(childmenu);
                currentConnectionToolStripMenuItem.DropDownItems.Add(childmenu);
            }
            if (objConnections.SelectedIndex >= 0)
            {
                //ToolStripMenuItem childmenu1 = (ToolStripMenuItem)contextConnections.Items[objConnections.SelectedIndex];
                ToolStripMenuItem childmenu1 = (ToolStripMenuItem)
                    currentConnectionToolStripMenuItem.DropDownItems[objConnections.SelectedIndex + 2];
                childmenu1.Checked = true;
            }
            //contextConnections.Show(btnConnections, e.Location);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "All Files (*.*)|*.*|SQL Files (*.sql)|*.sql";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
                FrmEditor childForm = CreateNewForm(FileName);
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEditor FormToSave = GetOperatingForm(false);
            if (FormToSave != null) SaveAs(FormToSave);
        }

        private void SaveAs(FrmEditor FormToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                FormToSave.QueryFileName = FileName;
                FormToSave.SaveToFile();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEditor FormToSave = GetOperatingForm(false);
            if (FormToSave != null)
            {
                if (FormToSave.QueryFileName == "")
                    SaveAs(FormToSave);
                else
                    FormToSave.SaveToFile();
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEditor FormToSave = GetOperatingForm(false);
            if (FormToSave != null) FormToSave.LoadFromFile();
        }


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (objConnections.ConnectionText == "") return;
            RefreshLists();
            tabDatabase.TabPages[1].Text = "Tables & Views(" + lstTables.Items.Count + ")";
            tabDatabase.TabPages[2].Text = "Procedures & Functions(" + lstProcedures.Items.Count + ")";
        }

        private void GetMetaData()
        {
            string sQuery = "select TABLE_NAME,TABLE_SCHEMA,TABLE_TYPE from INFORMATION_SCHEMA.Tables order by table_type, table_name; "; //TABLE_SCHEMA,
            //"SELECT Name FROM sysobjects WHERE (xtype = 'V') order by Name; " + // AND (status > 0) U - tables V' - views 'S' - system tables
            sQuery = sQuery + "SELECT ROUTINE_NAME, ROUTINE_SCHEMA, ROUTINE_TYPE ,LAST_ALTERED, CREATED ";
            //if (Fastconnection) sQuery = sQuery + ", ROUTINE_DEFINITION";
            sQuery = sQuery + " FROM INFORMATION_SCHEMA.ROUTINES order by ROUTINE_TYPE desc, ROUTINE_NAME; ";
            using (DataBaseProcedure DataObj = new DataBaseProcedure(sConnectionString, sQuery, true, CommandType.Text))
            {
                if (DataObj.Execute(new DataBaseProcedure.dlgReaderOpen(ReaderEvent)) == false)
                {
                    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
            }
        }

        private void ReaderEvent(object sender, object objReader)
        {
            System.Data.SqlClient.SqlDataReader Reader = (System.Data.SqlClient.SqlDataReader)objReader;
            dtTablesViews = new DataTable();
            dtTablesViews.Load(Reader);
            //Reader.NextResult(); Load automatically moves to next result set
            dtProcedures = new DataTable();
            dtProcedures.Load(Reader);

            RefreshDBStructure();

        }

        private void RefreshDBStructure()
        {
            if(dtTablesViews.Rows.Count > 0)
            {
                // Add  to the ListView.
                lstTables.Clear();
                lstTables.Columns.Add("Table Name", 200, HorizontalAlignment.Left);
                lstTables.Columns.Add("Schema", 50, HorizontalAlignment.Left);
                lstTables.Columns.Add("Type", 80, HorizontalAlignment.Left);
                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                    dtTablesViews.DefaultView.RowFilter = "[TABLE_NAME] LIKE '*" + txtFilter.Text.Trim() + "*'";
                else
                    dtTablesViews.DefaultView.RowFilter = "";
                LoadListViewFromTable(lstTables, dtTablesViews);
            }

            if(dtProcedures.Rows.Count > 0)
            {
                //Process next resultset
                // Add the columns to the ListView.

                lstProcedures.Clear();
                lstProcedures.Columns.Add("Procedure Name", 200, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Schema", 50, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Type", 80, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Modified", 100, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Created", 100, HorizontalAlignment.Left);
                //            lstProcedures.Columns.Add("Definition", 80, HorizontalAlignment.Left);

                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                    dtProcedures.DefaultView.RowFilter = "[ROUTINE_NAME] LIKE '*" + txtFilter.Text.Trim() + "*'";
                else
                    dtProcedures.DefaultView.RowFilter = "";

                LoadListViewFromTable(lstProcedures, dtProcedures);
            }

        }

        private void LoadListViewFromTable(ListView lstView, DataTable dt)
        {
            foreach (DataRowView row in dt.DefaultView)      //while (Reader.Read())
            {
                ListViewItem lvItem = new ListViewItem((string)row[0]);   // Reader[0].ToString() Create the ListViewItem row with the first column.
                for (int i = 1; i < lstView.Columns.Count; i++)                // Reader.FieldCount Add the data for the other columns.
                {
                    lvItem.SubItems.Add(row[i].ToString());      //Reader[i].ToString()
                }
                lstView.Items.Add(lvItem);
            }
        }



        private void lstTables_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (lstTables.SelectedItems.Count > 0) contextMenuStrip1.Show(lstTables, e.Location);
        }

        private void lstConnections_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextConnection.Show(objConnections, e.Location);
        }

        private void lstTables_DoubleClick(object sender, EventArgs e)
        {
            LoadStructure();
        }
        private void viewStructureToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LoadStructure();
        }

        private string GetSelectedTableName()
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                return lstTables.SelectedItems[0].SubItems[colSchema].Text + "." +
                       lstTables.SelectedItems[0].SubItems[colName].Text;
            }
            else
                return "";
        }

        private async Task LoadStructure()
        {
            string sTableName = GetSelectedTableName();
            if (sTableName != "")
            {
                string stablenameonly = lstTables.SelectedItems[0].SubItems[colName].Text;
                if (lstTables.SelectedItems[0].SubItems[colType].Text.ToUpper() == "BASE TABLE")
                    GetEmptyOperatingForm().SetDataGrid(await GetTableDetails(stablenameonly));
                else
                {
                    GetEmptyOperatingForm().SetProcedureText(
                        await GetProcedureDefinition(
                            sTableName,
                            lstTables.SelectedItems[0].SubItems[colType].Text)
                        );
                }
            }
        }

        private async Task GetTableRows(int Rows, bool isReverse = false)
        {
            string sTableName = GetSelectedTableName();

            if (sTableName != "") 
            {
                string sQuery = "";
                if (Rows != -1) sQuery = " top " + Rows.ToString() + " ";
                sQuery = "select " + sQuery + " * from " + sTableName;
                if (isReverse) sQuery += " order by 1 desc";
                await GetEmptyOperatingForm().LoadQuery(sQuery);
            }
        }

        private void showTop100ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            GetTableRows( 100 );
        }

        private void showTop100ReverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetTableRows(100, true);
        }

        private void showFieldHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetTableRows(0);
        }

        private void showTop1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetTableRows(1);
        }

        private void showTop10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetTableRows(10);
        }
        private void showAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            GetTableRows(-1);
        }

        private void lstProcedures_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (lstProcedures.SelectedItems.Count > 0) contextMenuStrip2.Show(lstTables, e.Location);
        }

        private string GetSelectedSPName()
        {
            if (lstProcedures.SelectedItems.Count > 0)
            {
                return lstProcedures.SelectedItems[0].SubItems[colSchema].Text + "." +
                       lstProcedures.SelectedItems[0].SubItems[colName].Text;
            }
            else
                return "";
        }
        private void TS_Procedure_Code_Click_1(object sender, EventArgs e)
        {
            getSelectedProcedureCode();
        }

            private void TS_Procedure_Run_Click_1(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingForm().SetProcedureText(
                    GetProcedureRun(sSPName) //lstProcedures.SelectedItems[0].Text)
                   );
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingForm().SetProcedureText(
                    "IF OBJECT_ID('" + sSPName + "') IS NOT NULL" +
                     Environment.NewLine + "  DROP " +
                     lstProcedures.SelectedItems[0].SubItems[colType].Text + " [" + sSPName + "]"
                 );
        }


         private void TS_Procedure_Structure_Click(object sender, EventArgs e)
         {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingForm().SetProcedureText(GetProcedureCSharpCode(sSPName, 1));
         }
         private void getCReaderToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(GetProcedureCSharpCode(sSPName, 2));
         }

         private void getCSimpleToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(GetProcedureCSharpCode(sSPName, 3));
         }

         private void getCScalarToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(GetProcedureCSharpCode(sSPName, 4));
         }

         private string GetProcedureCSharpCode(string Name, int Type)
         {
             string sCSharp = "", sParameterFunction = "", sValue = "";
             SqlParameter parameter = null;

             sCSharp = "string sConnectionString = ConfigurationManager.ConnectionStrings[\"MainConnection\"].ConnectionString;" + Environment.NewLine +
                 "using (CommonDatabase<SqlConnection, SqlCommand, SqlDataAdapter> DataObj =" + Environment.NewLine +
                 "    new CommonDatabase<SqlConnection, SqlCommand, SqlDataAdapter>" + Environment.NewLine +
                 "    (sConnectionString, \"" + Name + "\"))" + Environment.NewLine +
                 "{" + Environment.NewLine;

             try
             {
                 using (DataBaseProcedure DataObj = new DataBaseProcedure(sConnectionString, Name, true,
                     CommandType.StoredProcedure))
                 {
                     DataObj.connection.Open();
                     SqlCommand obj = DataObj.command;
                     SqlCommandBuilder.DeriveParameters(obj);
                     for (int i = 0; i < obj.Parameters.Count; i++)
                     {
                         parameter = obj.Parameters[i];
                         sValue = "";

                         if (parameter.Direction == ParameterDirection.Input)
                         {
                             sParameterFunction = "AddInputParameter";
                             sValue = ", value" + i.ToString();
                         }
                         else if (parameter.Direction == ParameterDirection.Output
                                         || parameter.Direction == ParameterDirection.InputOutput)
                             sParameterFunction = "AddOutputParameter";
                         else if (parameter.Direction == ParameterDirection.ReturnValue)
                             sParameterFunction = "AddReturnParameter";

                         if (parameter.Size > 0)
                             sCSharp += String.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1}, {2} {3});" + Environment.NewLine,
                                 parameter.ParameterName, parameter.SqlDbType.ToString("F"), parameter.Size.ToString(), sValue);
                         else
                             sCSharp += String.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1} {2});" + Environment.NewLine,
                                 parameter.ParameterName, parameter.SqlDbType.ToString("F"), sValue); //parameter.DbType.GetType().FullName

                     }
                 }
             }
             catch (Exception e)
             {
                 MessageBox.Show("Error occured" + e.ToString(), Application.ProductName, MessageBoxButtons.OK);
             }

             sCSharp += "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
                         "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
                         "   {" + Environment.NewLine +
                         "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
                         "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
                         "   }" + Environment.NewLine +
                         "}" + Environment.NewLine;

             return sCSharp + Environment.NewLine + Environment.NewLine; // +sProcedure;
         }
         //string sProcedure = ""

        //else
         //    sParameterFunction = "AddIOParameter";

         //sProcedure += "Name:" + parameter.ParameterName;
         //sProcedure += Environment.NewLine + " Type:" + parameter.SqlDbType.ToString("F");
         //sProcedure += Environment.NewLine + " Size:" + parameter.Size.ToString();
         //sProcedure += Environment.NewLine + " Direction: " + parameter.Direction.ToString() +
         //    Environment.NewLine +
         //    Environment.NewLine;


         //"else" + Environment.NewLine +
         //"{" + Environment.NewLine +
         //"    (new ErrorLog()).LogError(new Exception(DataObj.ErrorText));" + Environment.NewLine +
         //"}" + Environment.NewLine + 

         private string GetExecutionType(int type)
         {
             string sCSharp="";

             switch (type)
             {
                 case 1: //Dataset
                     sCSharp = "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
                               "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
                               "   {" + Environment.NewLine +
                               "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
                               "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
                               "   }" + Environment.NewLine;
                     break;
                 
                 case 2: //Reader
                     sCSharp = "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
                               "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
                               "   {" + Environment.NewLine +
                               "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
                               "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
                               "   }" + Environment.NewLine;
                     break;
                 
                 case 3: //simple
                     sCSharp = "   if (DataObj.Execute())" + Environment.NewLine +
                               "   {" + Environment.NewLine +
                               "   }" + Environment.NewLine;
                     break;
                 
                 case 4: //Scalar
                     sCSharp = "   Object DataobjSet = null;" + Environment.NewLine +
                               "   if (DataObj.Execute(out DataobjSet))" + Environment.NewLine +
                               "   {" + Environment.NewLine +
                               "   }" + Environment.NewLine;
                     break;
             }

             sCSharp += "}" + Environment.NewLine;
             return sCSharp;
         }
         //"else" + Environment.NewLine +
         //"{" + Environment.NewLine +
         //"    (new ErrorLog()).LogError(new Exception(DataObj.ErrorText));" + Environment.NewLine +
         //"}" + Environment.NewLine + 


        private string GetProcedureRun(string Name)
        {
            string sProcedure = Environment.NewLine + "EXECUTE " + Name + " (";
            SqlParameter parameter = null;

            try
            {
                using (DataBaseProcedure DataObj = new DataBaseProcedure(sConnectionString, Name, true,
                    CommandType.StoredProcedure))
                {
                    string sSize = "";
                    string sValue = "";

                    DataObj.connection.Open();
                    SqlCommand obj = DataObj.command;
                    SqlCommandBuilder.DeriveParameters(obj);
                    for (int i = 1; i < obj.Parameters.Count; i++)
                    {
                        parameter = obj.Parameters[i];

                        if (parameter.Size > 0) 
                                sSize = "(" + parameter.Size.ToString() + ") ";
                        else
                                sSize = "";
                        if (parameter.Value != null)
                            sValue = " = " + parameter.Value.ToString() + " ";
                        else
                            sValue = "";

                        if (parameter.Direction == ParameterDirection.Input)
                            sProcedure += Environment.NewLine +
                                ", " + parameter.ParameterName + " " + parameter.SqlDbType.ToString("F")
                                + sSize + sValue;
                        else if (parameter.Direction == ParameterDirection.Output
                                    || parameter.Direction == ParameterDirection.InputOutput )
                            sProcedure += Environment.NewLine +
                                ", " + parameter.ParameterName + " " + parameter.SqlDbType.ToString("F")
                                + sSize + " OUTPUT" + sValue;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured" + e.ToString(), Application.ProductName, MessageBoxButtons.OK);
            }

            //remove start brace and comma and dont use end brace + ")"
            return sProcedure.Replace("(" + Environment.NewLine + ",", Environment.NewLine)
                + Environment.NewLine ;
        }

        private async Task<string> GetProcedureDefinition(string Name, string sType)
        {
            //string tablename = Name.Substring(Name.LastIndexOf(".")+1);
            //string sQuery = "SELECT ROUTINE_DEFINITION FROM INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = '" + tablename + "' and ROUTINE_TYPE = '" + sType + "'";
            string sQuery = "EXEC sp_helptext N'" + Name + "';";
            //string sQuery = "SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + Name + "'));";
            //string sQuery = "SELECT definition FROM sys.sql_modules WHERE object_id = (OBJECT_ID(N'" + Name + "'));";
            //object objReturn;
            string sProcedure = "";
            using (DataBaseProcedure DataObj = new DataBaseProcedure(sConnectionString, sQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                //task.Wait();
                //var ds = task.Result;
                if (ds != null )
                {
                    for (int i = 0; i < ds.Tables["MyTable"].Rows.Count; i++)
                    {
                        sProcedure = sProcedure + (string)ds.Tables["MyTable"].Rows[i].ItemArray[0]; // +Environment.NewLine;
                    }
                    //.Text = "Total lines :" + ds.Tables["MyTable"].Rows.Count.ToString();
                }
                else
                {
                    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
                //if (DataObj.ExecuteScalar(out objReturn) == false)
                //    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                //else
                //    sProcedure = (string) objReturn;
            }
            return sProcedure;
        }

        //private void btnConnections_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button != MouseButtons.Right)
        //    else
        //}
        private void connectionsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem childmenu1;
            if (e.ClickedItem.Tag.ToString() == "Manage")
            {
                objConnections.ShowDialog(this);
                //contextConnection.Show(btnConnections, e.Location);
                RefreshConnectionsMenu();
            }
            else
            {
                if (objConnections.SelectedIndex >= 0)
                {
                    childmenu1 = (ToolStripMenuItem)
                        currentConnectionToolStripMenuItem.DropDownItems[objConnections.SelectedIndex + 2];
                    childmenu1.Checked = false;
                }
                
                objConnections.SelectedIndex = (int)e.ClickedItem.Tag;
                childmenu1 = (ToolStripMenuItem)e.ClickedItem;
                childmenu1.Checked = true;

            }
        }


        private void contextConnections_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            objConnections.SelectedIndex = (int)e.ClickedItem.Tag;
        }

        private void lstProcedures_DoubleClick(object sender, EventArgs e)
        {
            getSelectedProcedureCode();
        }
        private async Task getSelectedProcedureCode()
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
            {
                string sProcedureBody = await GetProcedureDefinition(sSPName, lstProcedures.SelectedItems[0].SubItems[colType].Text);
                //if (sProcedureBody.Substring(0,6).ToUpper() == "CREATE")
                //    sProcedureBody = "ALTER" + sProcedureBody.Substring(6);
                sProcedureBody = sProcedureBody.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
                sProcedureBody = sProcedureBody.Replace("CREATE FUNCTION", "ALTER FUNCTION");
                sProcedureBody = sProcedureBody.Replace("CREATE VIEW", "ALTER VIEW");

                GetEmptyOperatingForm().SetProcedureText( sProcedureBody );
            }
        }
        private void RefreshLists()
        {
            GetMetaData(); //chkConnection.Checked
        }

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            (new MyRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", objConnections.SelectedIndex.ToString());
            //(new MyRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "CheckState",chkConnection.Checked.ToString());
        }

        public async Task<DataTable> GetTableDetails(string TableName)
        {
            DataSet ds;
            string localColumn;
            string localDataType = "";
            int localLength;
            bool localIdentity;
            string SQuery;

            DataTable dt = new DataTable("TableInfo"); //output table
            DataRow workrow;
            string lsConnection = sConnectionString;

            SQuery = "select '" + TableName + "' as TableName, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where table_name = '" + TableName + "' ";
            using (DataBaseProcedure DataObj = new DataBaseProcedure(lsConnection, SQuery, true, CommandType.Text))
            {
                ds = await DataObj.Execute("RawTableInfo");
                //task.Wait();
                //ds = task.Result;
                if (ds != null)
                {
                    dt.Columns.Add("TableName", Type.GetType("System.String"));
                    dt.Columns.Add("ColumnName", Type.GetType("System.String"));
                    dt.Columns.Add("DataType", Type.GetType("System.String"));
                    dt.Columns.Add("Length", Type.GetType("System.Int64"));
                    dt.Columns.Add("Identity", Type.GetType("System.Boolean"));

                    foreach (DataRow rw in ds.Tables["RawTableInfo"].Rows)
                    {
                        if (!System.Convert.IsDBNull(rw["Column_Name"]))
                        {
                            localColumn = rw["Column_Name"].ToString();
                            if (!System.Convert.IsDBNull(rw["DATA_TYPE"]))
                            {
                                localDataType = rw["DATA_TYPE"].ToString();
                            }
                            if (!System.Convert.IsDBNull(rw["CHARACTER_MAXIMUM_LENGTH"]))
                            {
                                localLength = int.Parse(rw["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            }
                            else
                            {
                                localLength = 0;
                            }

                            if (localDataType.ToLower() == "int")
                            {
                                localIdentity = CheckColumIdentity(TableName, localColumn);
                            }
                            else
                            {
                                localIdentity = false;
                            }

                            workrow = dt.NewRow();
                            workrow["TableName"] = TableName;
                            workrow["ColumnName"] = localColumn;
                            workrow["Datatype"] = localDataType;
                            workrow["Length"] = localLength;
                            workrow["Identity"] = localIdentity;
                            dt.Rows.Add(workrow);
                        }
                    }
                }
            }
            return dt;

        }

        public bool CheckColumIdentity(string TableName, string ColumnName)
        {
            string SQL;

            SQL = "SELECT COLUMNPROPERTY( OBJECT_ID('" + TableName + "'),'" + ColumnName + "','IsIdentity')";
            string lsConnection = sConnectionString;
            using (DataBaseProcedure DataObj = new DataBaseProcedure(lsConnection, SQL, true, CommandType.Text))
            {
                object objreturn;
                if (DataObj.ExecuteScalar(out objreturn))
                {
                    if (!(objreturn is System.DBNull))
                    {
                        if ((int)objreturn == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshConnectionsMenu();

        }


        private void listViewSnippets_DoubleClick(object sender, EventArgs e)
        {
            ExecuteSnippet();
        }

        private async Task ExecuteSnippet()
        {
            string snippetName = GetSelectedSnippetName();
            if (snippetName != "")
            {
                var row = GetSnippetRow(snippetName);
                string snippetCode = (string)row[1];
                var operatingForm = GetEmptyOperatingForm();
                operatingForm.SetProcedureText(snippetCode);
                if ((string)row[2] == "True")
                    await operatingForm.LoadQuery(snippetCode);
            }
        }

        private DataRow GetSnippetRow(string snippetName)
        {
            DataRow[] rows = dtSnippetsTable.Select("SnippetName = '" + snippetName + "'");
            return rows[0];
        }

        private string GetSelectedSnippetName()
        {
            if (listViewSnippets.SelectedItems.Count > 0)
            {
                return listViewSnippets.SelectedItems[0].SubItems[0].Text;
            }
            else
                return "";
        }
        private string GetSelectedDBName()
        {
            if (listViewDBs.SelectedItems.Count > 0)
            {
                return listViewDBs.SelectedItems[0].SubItems[0].Text;
            }
            else
                return "";
        }

        private void toolstripEditSnippet_Click(object sender, EventArgs e)
        {
            if (snippetsEditor == null || snippetsEditor.IsDisposed)
            {
                snippetsEditor = new JsonReader();
                snippetsEditor.RefreshJsonTable(snippetFilePath, 1);
            }
            snippetsEditor.Show();
        }

        private void toolstripRefreshSnippets_Click(object sender, EventArgs e)
        {
            RefreshSnippets();
        }

        private void listViewSnippets_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var IsSnippetSelected = (listViewSnippets.SelectedItems.Count > 0);
                toolstripLoadSnippet.Visible = toolstripExecuteSnippet.Visible= IsSnippetSelected;
                ctxMenuStripSnippets.Show(listViewSnippets, e.Location);
            }
        }

        private void toolstripLoadSnippet_Click(object sender, EventArgs e)
        {
            LoadSnippet();
        }

        private void LoadSnippet()
        {
            string snippetName = GetSelectedSnippetName();
            if (snippetName != "")
            {
                string snippetCode = (string) GetSnippetRow(snippetName)[1];
                var operatingForm = GetEmptyOperatingForm();
                operatingForm.SetProcedureText(snippetCode);
            }
        }

        private void toolstripExecuteSnippet_Click(object sender, EventArgs e)
        {
            ExecuteSnippet();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            RefreshDBStructure();
        }

        #region Tabbed windows
        /// <summary>
        /// https://www.codeproject.com/Articles/17640/Tabbed-MDI-Child-Forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MDIAdvancedQuery_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == null)
                tabForms.Visible = false; // If no any child form, hide tabControl
            else
            {
                this.ActiveMdiChild.WindowState = FormWindowState.Maximized; // Child form always maximized

                // If child form is new and no has tabPage, create new tabPage
                if (this.ActiveMdiChild.Tag == null)
                {
                    // Add a tabPage to tabControl with child form caption
                    TabPage tp = new TabPage(this.ActiveMdiChild.Text);
                    tp.Tag = this.ActiveMdiChild;
                    tp.Parent = tabForms;
                    tabForms.SelectedTab = tp;

                    this.ActiveMdiChild.Tag = tp;
                    this.ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);
                }

                if (!tabForms.Visible) tabForms.Visible = true;
            }
        }

        // If child form closed, remove tabPage
        private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
        {
            ((sender as Form).Tag as TabPage).Dispose();
        }

        private void tabForms_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
                (tabForms.SelectedTab.Tag as Form).Select();
        }

        #endregion

        private void sQLEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEditor childForm = CreateNewForm("");
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
        }

        private void jsonTableEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var objJsonReader = new JsonReader();
            objJsonReader.MdiParent = this;
            objJsonReader.WindowState = FormWindowState.Maximized;
            objJsonReader.Show();

        }

        private void xMLTableEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            XMLReader objXML = new XMLReader();
            objXML.MdiParent = this;
            objXML.WindowState = FormWindowState.Maximized;
            objXML.Show();
        }

        private void xMLDBManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xmlEditor objXML = new xmlEditor();
            objXML.Show();
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnFilter_Click(sender, e);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            RefreshDBs();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by,\nSabarinathan Arthanari\ntmasabari@gmail.com" +
                "\n\nVersion: 3.00\nReleased : Sat June 20, 2009" +
                "\n\nVersion: 4.00\nReleased : Sun April 24, 2022"
                , "Smart DB Editor");

        }
    }
}
