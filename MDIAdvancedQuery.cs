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
//using WinFormsLogic;

//command line "H:\web\Order online\App_Data\Paypal.xml"
namespace AdvancedQueryOrganizer
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
        public frmConnections objConnections;
        
        private string snippetFilePath = Application.StartupPath + "\\Snippets.json";
        private DataTable dtDatabases = new DataTable();
        private DataTable dtTablesViews = new DataTable();
        private DataTable dtProcedures = new DataTable();
        private DataTable dtSnippetsTable = new DataTable();
        private DataTableEditor snippetsEditor;
        private string lastSessionSelectedDBName = null;
        private string lastSessionFilesPath = Application.StartupPath + "\\LastSession.txt";
        public MDIAdvancedQuery()
        {
            InitializeComponent();
            splitDatabase.SplitterDistance = 140;
            splitDatabase.FixedPanel = FixedPanel.Panel1;
            splitContainerDB.SplitterDistance = 140;
            //splitContainerDB.FixedPanel = FixedPanel.Panel1;
            objConnections = new frmConnections();
            objConnections.ConnectionChanged += ObjConnections_ConnectionChanged;
        }
        private void MDIParent1_Activated(object sender, EventArgs e)
        {
            if (bFirstTime)
            {
                ShowProgress();
                object lIndex; //lState,
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", out lIndex);
                object lastSessionSelectedDBNameObj = null;
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "DB", out lastSessionSelectedDBNameObj);
                if (lastSessionSelectedDBNameObj != null) lastSessionSelectedDBName = (string)lastSessionSelectedDBNameObj;
                //(new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "CheckState", out lState);
                if (lIndex == null) objConnections.SelectedIndex = 0; else objConnections.SelectedIndex = int.Parse(lIndex.ToString());
                //if (lState == null) chkConnection.Checked = true; else chkConnection.Checked = bool.Parse(lState.ToString());

                object objLoadPreviousSession; //lState,
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"Options", "LoadPreviousSession", out objLoadPreviousSession);
                if(objLoadPreviousSession != null) chkLoadPreviousSession.Checked = Boolean.Parse((string)objLoadPreviousSession);

                bFirstTime = false;
                //this is not required as setting the connection from the registry automatically triggers the refresh
                //RefreshDBs();
                RefreshSnippets();

                if (objConnections.ConnectionList.Items.Count == 0)
                    objConnections.ShowDialog(this);

                RefreshConnectionsMenu();

                if(chkLoadPreviousSession.Checked && File.Exists(lastSessionFilesPath))
                {
                    var filePathArry = File.ReadAllLines(lastSessionFilesPath);
                    foreach (var filePath in filePathArry)
                    {
                        if (File.Exists(filePath))
                        {
                            OpenFile(filePath);
                        }
                    }
                }
            }
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
        private void ObjConnections_ConnectionChanged(object sender, EventArgs e)
        {
            currentConnectionToolStripMenuItem.Text = "Connections [" + objConnections.ConnectionName + "]";
            RefreshDBs();
        }

        private IOperatingForm GetOperatingForm()  //bool CreateNew=true
        {
            if (this.ActiveMdiChild is IOperatingForm)
                return ((IOperatingForm)this.ActiveMdiChild);
            return null;
            //else
            //{
            //    if (CreateNew)
            //        return CreateNewForm("");
            //    else
            //        return null;
            //}
        }
        private FrmEditor GetEmptyOperatingForm()
        {
            //if (this.ActiveMdiChild is FrmMain)
            //{
            //    FrmMain objChild = (FrmMain)this.ActiveMdiChild;
            //    if (objChild.isFormEmpty)
            //        return ((FrmMain)this.ActiveMdiChild);
            //}
            return (FrmEditor) CreateNewForm("");
        }

        private Form CreateNewForm(string sFileName, FormTypes formType = FormTypes.SQLEditor, int fillColumn = -1)
        {
            // Create a new instance of the child form.
            IOperatingForm childOperatingForm = null;
            switch (formType)
            {
                case FormTypes.SQLEditor:
                    childOperatingForm = new FrmEditor();
                    break;
                case FormTypes.JsonEditor:
                case FormTypes.XMLEditor:
                    childOperatingForm = new DataTableEditor(formType);
                    break;
                default:
                    throw new ApplicationException("Invalid form type");
            }
            childOperatingForm.FillColumn = fillColumn;
            childOperatingForm.FileName = sFileName;
            childOperatingForm.LoadFromFile();
            Form childForm = (Form)childOperatingForm;
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
            listViewDBs.Columns.Add("Name", 130, HorizontalAlignment.Left);
            listViewDBs.Columns.Add("IsSystemDB", 50, HorizontalAlignment.Left);
            listViewDBs.Columns.Add("Created", 130, HorizontalAlignment.Left);
            string sQuery = DynamicDataSourceCode.GetAllDBsCode;
            using (DynamicDAL DataObj = new DynamicDAL(sMasterConnectionString, sQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                //txtOutputText.Text = DataObj.SQLInfoMessageBuilder.ToString();
                if (ds != null)
                {
                    if (ds.Tables["MyTable"] != null)
                    {
                        LoadListViewFromTable(listViewDBs, ds.Tables["MyTable"]);
                        if(!string.IsNullOrWhiteSpace(lastSessionSelectedDBName))
                        {
                            for (var i = 0; i<listViewDBs.Items.Count; i++) 
                            {
                                if (listViewDBs.Items[i].SubItems[0].Text == lastSessionSelectedDBName)
                                {
                                    listViewDBs.Items[i].Selected = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //set non system db as default db
                            for (var i = 0; i < listViewDBs.Items.Count; i++)
                            {
                                if (listViewDBs.Items[i].SubItems[1].Text == "0")
                                {
                                    listViewDBs.Items[i].Selected = true;
                                    break;
                                }
                            }
                        }
                        //Refresh the schema automatically
                        if (objConnections.ConnectionText == "") return;
                        GetMetaData();  

                    }
                }
            }
        }


        private void RefreshSnippets()
        {
            dtSnippetsTable = Common.LoadJsonToTable(snippetFilePath);

            listViewSnippets.Clear();
            listViewSnippets.Columns.Add("Snippet", 180, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("Query", 0, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("IsExecutable", 50, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("TiedDB", 80, HorizontalAlignment.Left);
            LoadListViewFromTable(listViewSnippets, dtSnippetsTable);
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
            openFileDialog.Filter = "All Files (*.*)|*.*" + "|SQL Files (*.sql)|*.sql"
                 + "|Json Files (*.json)|*.json" + "|XML Files (*.xml)|*.xml";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
            }
        }
        private void OpenFile(string sFileName)
        {
            string FileName = sFileName.Trim();
            var formType = FormTypes.SQLEditor;
            if (FileName.ToLower().EndsWith(".xml")) formType = FormTypes.XMLEditor;
            else if (FileName.ToLower().EndsWith(".json")) formType = FormTypes.JsonEditor;

            var childForm = CreateNewForm(FileName, formType);
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOperatingForm FormToSave = GetOperatingForm();
            if (FormToSave != null)
            {
                if (FormToSave != snippetsEditor) //do not invoke save as for snippets editor
                    SaveAs(FormToSave);
                else
                    MessageBox.Show("'Save As' option is not applicable for snippets.");
            }
        }

        private void SaveAs(IOperatingForm FormToSave)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
                FormToSave.FileName = FileName;
                FormToSave.SaveToFile();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOperatingForm FormToSave = GetOperatingForm();
            if (FormToSave != null)
            {
                if (FormToSave.FileName == "")
                    SaveAs(FormToSave);
                else
                {
                    FormToSave.SaveToFile();
                    if (FormToSave == snippetsEditor) //refresh snippets automatically if snippets are saved
                    {
                        RefreshSnippets();
                    }
                }
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IOperatingForm FormToSave = GetOperatingForm();
            if (FormToSave != null)
            {
                var confirmResult = MessageBox.Show("You will loose any unsaved data. Would you like to Continue?", 
                    "Confirm reload", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(confirmResult == DialogResult.Yes)
                    FormToSave.LoadFromFile();
            }
        }


        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            GetMetaData(); //chkConnection.Checked
        }

        private void GetMetaData()
        {
            string sQuery = DynamicDataSourceCode.GetAllSchemaCode + ";" + DynamicDataSourceCode.GetAllDBModulesCode;
            using (DynamicDAL DataObj = new DynamicDAL(sConnectionString, sQuery, true, CommandType.Text))
            {
                if (DataObj.Execute(new DynamicDAL.dlgReaderOpen(ReaderEvent)) == false)
                {
                    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
            }
            //todo
            //tabDatabase.TabPages[1].Text = "Tables & Views(" + lstTables.Items.Count + ")";
            //tabDatabase.TabPages[2].Text = "Procedures & Functions(" + lstProcedures.Items.Count + ")";
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
                lstProcedures.Columns.Add("Module Name", 200, HorizontalAlignment.Left);
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

            if (progressBar1.Visible) StopProgress();

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
            getColumnListToolStripMenuItem_Click(sender, e);
            //LoadStructure();
        }
        private void viewStructureToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LoadStructure();
        }

        private string GetSelectedTableNameWithSchema()
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                return lstTables.SelectedItems[0].SubItems[colSchema].Text + "." +
                       lstTables.SelectedItems[0].SubItems[colName].Text;
            }
            else
                return "";
        }

        private string GetSelectedTableName()
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                return lstTables.SelectedItems[0].SubItems[colName].Text;
            }
            else
                return "";
        }

        private async Task LoadStructure()
        {
            string sTableName = GetSelectedTableNameWithSchema();
            if (sTableName != "")
            {
                string stablenameonly = lstTables.SelectedItems[0].SubItems[colName].Text;
                if (lstTables.SelectedItems[0].SubItems[colType].Text.ToUpper() == DynamicDataSourceCode.BaseTableType)
                    GetEmptyOperatingForm().SetDataGrid(await GetTableDetails(stablenameonly));
                else
                {
                    GetEmptyOperatingForm().SetProcedureText(
                        await DynamicDataSourceCode.GetProcedureDefinition(sConnectionString,
                            sTableName,
                            lstTables.SelectedItems[0].SubItems[colType].Text)
                        );
                }
            }
        }

        private async Task GetTableRows(int Rows, bool isReverse = false, string columnList = null)
        {
            string sTableName = GetSelectedTableNameWithSchema();
            if (sTableName != "") 
            {
                string sQuery = DynamicDataSourceCode.GetTableRowsCode(sTableName, Rows, isReverse, columnList);
                await GetEmptyOperatingForm().LoadQuery(sQuery);
            }
        }

        private void getColumnListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sTableName = GetSelectedTableName();

            if (sTableName != "")
            {
                string columnList = DynamicDAL.GetColumnList(sConnectionString, sTableName);
                GetTableRows(100, false, columnList);
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
                if (lstProcedures.SelectedItems.Count > 0) contextMenuStrip2.Show(lstProcedures, e.Location);
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
                    DynamicDataSourceCode.GetProcedureRun(sConnectionString, sSPName) //lstProcedures.SelectedItems[0].Text)
                   );
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingForm().SetProcedureText(DynamicDataSourceCode.GetDropCode(sSPName,
                    lstProcedures.SelectedItems[0].SubItems[colType].Text));
        }


         private void TS_Procedure_Structure_Click(object sender, EventArgs e)
         {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingForm().SetProcedureText(CodeGeneration.GetProcedureCSharpCode(sConnectionString, sSPName, 1));
         }
         private void getCReaderToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(CodeGeneration.GetProcedureCSharpCode(sConnectionString, sSPName, 2));
         }

         private void getCSimpleToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(CodeGeneration.GetProcedureCSharpCode(sConnectionString, sSPName, 3));
         }

         private void getCScalarToolStripMenuItem_Click(object sender, EventArgs e)
         {
             string sSPName = GetSelectedSPName();
             if (sSPName != "")
                 GetEmptyOperatingForm().SetProcedureText(CodeGeneration.GetProcedureCSharpCode(sConnectionString, sSPName, 4));
         }
         
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
                string sProcedureBody = await DynamicDataSourceCode.GetProcedureDefinition(sConnectionString, sSPName, lstProcedures.SelectedItems[0].SubItems[colType].Text);
                //if (sProcedureBody.Substring(0,6).ToUpper() == "CREATE")
                //    sProcedureBody = "ALTER" + sProcedureBody.Substring(6);

                //do not do this it may create a problem in dynamic query
                //sProcedureBody = sProcedureBody.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
                //sProcedureBody = sProcedureBody.Replace("CREATE FUNCTION", "ALTER FUNCTION");
                //sProcedureBody = sProcedureBody.Replace("CREATE VIEW", "ALTER VIEW");

                GetEmptyOperatingForm().SetProcedureText( sProcedureBody );
            }
        }

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                //var sessions = new List<SessionVM>();
                foreach (TabPage tabPage in tabForms.TabPages)
                {
                    var operatingForm = tabPage.Tag as IOperatingForm;
                    if (operatingForm != null && operatingForm != snippetsEditor)
                        //sessions.Add(new SessionVM { FilePath = tabPage.Text, Contents = "" });
                        sb.AppendLine(operatingForm.FileName);
                }
                File.WriteAllText(lastSessionFilesPath, sb.ToString());

                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"Options", "LoadPreviousSession", chkLoadPreviousSession.Checked.ToString());
                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", objConnections.SelectedIndex.ToString());
                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "DB", GetSelectedDBName());
                //(new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "CheckState",chkConnection.Checked.ToString());
            }
            catch (Exception ex)
            {
                ShowMessageText.Show("Unexpected error occurred " + Environment.NewLine + ex.ToString(), "Close Error");
            }
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

            SQuery = DynamicDataSourceCode.GetColumnsCode( TableName );
            using (DynamicDAL DataObj = new DynamicDAL(lsConnection, SQuery, true, CommandType.Text))
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

                    var identityColumn = DynamicDAL.GetIdentityColumn(sConnectionString, TableName);

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
                            localIdentity = localColumn == identityColumn;

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
                snippetsEditor = (DataTableEditor) CreateNewForm(snippetFilePath, FormTypes.JsonEditor,1);
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
            var tabPage = ((sender as Form).Tag as TabPage);
            var index = tabForms.TabPages.IndexOf(tabPage);
            if(index > 0)
                tabForms.SelectedIndex = index - 1;
            tabPage.Dispose();
        }

        private void tabForms_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
                (tabForms.SelectedTab.Tag as Form).Select();
        }

        #endregion

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var childForm = CreateNewForm("");
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();
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
                "\n\nVersion: 3.0.0\nReleased : Sat June 20, 2009" +
                "\n\nVersion: 4.1.0\nReleased : Thu July 14, 2022"
                , "Advanced Query Organizer");

        }

        private void chkShowDB_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerDB.Panel1Collapsed = ! chkShowDB.Checked;
        }

        private void chkShowTables_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerSchema.Panel1Collapsed = ! chkShowTables.Checked;
        }

        private void chkShowCode_CheckedChanged(object sender, EventArgs e)
        {
            splitContainerSchema.Panel2Collapsed = !chkShowCode.Checked;
        }

        private void listViewDBs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastSessionSelectedDBName = GetSelectedDBName();
        }

    }
}
//this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
//            this.sQLEditorToolStripMenuItem,
//            this.jsonTableEditorToolStripMenuItem1,
//            this.xMLTableEditorToolStripMenuItem1});

//this.sQLEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
//this.jsonTableEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
//this.xMLTableEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
//// 
//// sQLEditorToolStripMenuItem
//// 
//this.sQLEditorToolStripMenuItem.Name = "sQLEditorToolStripMenuItem";
//this.sQLEditorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
//this.sQLEditorToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
//this.sQLEditorToolStripMenuItem.Text = "SQL Editor";
//this.sQLEditorToolStripMenuItem.Click += new System.EventHandler(this.sQLEditorToolStripMenuItem_Click);
//// 
//// jsonTableEditorToolStripMenuItem1
//// 
//this.jsonTableEditorToolStripMenuItem1.Name = "jsonTableEditorToolStripMenuItem1";
//this.jsonTableEditorToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
//this.jsonTableEditorToolStripMenuItem1.Text = "Json Table Editor";
//this.jsonTableEditorToolStripMenuItem1.Click += new System.EventHandler(this.jsonTableEditorToolStripMenuItem1_Click);
//// 
//// xMLTableEditorToolStripMenuItem1
//// 
//this.xMLTableEditorToolStripMenuItem1.Name = "xMLTableEditorToolStripMenuItem1";
//this.xMLTableEditorToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
//this.xMLTableEditorToolStripMenuItem1.Text = "XML Table Editor";
//this.xMLTableEditorToolStripMenuItem1.Click += new System.EventHandler(this.xMLTableEditorToolStripMenuItem1_Click);

//private void sQLEditorToolStripMenuItem_Click(object sender, EventArgs e)
//{
//}

//private void jsonTableEditorToolStripMenuItem1_Click(object sender, EventArgs e)
//{
//    var objJsonReader = new JsonReader();
//    objJsonReader.MdiParent = this;
//    objJsonReader.WindowState = FormWindowState.Maximized;
//    objJsonReader.Show();

//}

//private void xMLTableEditorToolStripMenuItem1_Click(object sender, EventArgs e)
//{
//    XMLReader objXML = new XMLReader();
//    objXML.MdiParent = this;
//    objXML.WindowState = FormWindowState.Maximized;
//    objXML.Show();
//}