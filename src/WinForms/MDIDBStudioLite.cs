using CoreLogic.PluginBase;
using CoreLogic.PluginBase.PluginBase;
using DBStudioLite.Model;
using DBStudioLite.Plugins;
using DBStudioLite.WindowsLogic;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//command line "H:\web\Order online\App_Data\Paypal.xml"
namespace DBStudioLite
{
    public partial class MDIDBStudioLite : Form
    {
        private const int colName = 0;
        private const int colSchema = 1;
        private const int colType = 2;
        private int QueryWindowNumber = 1;
        private FrmOptions frmOptions = new FrmOptions();
        private const int fixedConnectionsMenu = 4;

        private string sStandardConnectionParams
        {
            get
            {
                return " ;Application Name=DBStudioLite;Min Pool Size=1;Max Pool Size=" + frmOptions.txtPool.Text
                    + ";Connection Timeout=" + frmOptions.txtConnectionTimeout.Text + ";" +
                    "Persist Security Info=False; MultipleActiveResultSets=True;Packet Size=" + frmOptions.txtPockets.Text + ";";
            }
        }
        private string DBParameter
        {
            get
            {
                return GetDBParameter(GetSelectedDBName());
            }
        }
        private string GetDBParameter(string dBName)
        {
            return ";initial catalog=" + dBName;
        }
        private string DBParameterMaster
        {
            get
            {
                return GetDBParameter("master");
            }
        }
        public string sConnectionString
        {
            get
            {
                return GetConnectionString();
            }
        }
        public string GetConnectionString(string dBName = "")
        {
            var connectionType = DataAccessFactory.GetConnectionType(objConnections.GetConnectionString());
            var text = objConnections.GetConnectionString();
            if (connectionType != "SQLite")
            {
                text += sStandardConnectionParams + DBParameter;
                if (!string.IsNullOrWhiteSpace(dBName)) text += GetDBParameter(dBName);
            }
            return text;
        }

        public string sMasterConnectionString
        {
            get
            {
                return objConnections.GetConnectionString() + sStandardConnectionParams + DBParameterMaster;
            }
        }
        public DynamicDataSourceCode GetDynamicDataSourceCode()
        {
            return new DynamicDataSourceCode(sConnectionString);
        }

        private bool bFirstTime = true;
        public frmConnections objConnections;
        private frmAboutMe objAboutMe = new frmAboutMe();

        private string snippetFilePath = Application.StartupPath + "\\Snippets.json";
        private DataTable dtTablesViews = new DataTable();
        private DataTable dtProcedures = new DataTable();
        private DataTable dtSnippetsTable = new DataTable();
        private DataTableEditor snippetsEditor;
        private string lastSessionSelectedDBName = null;
        private string lastSessionFilesPath = Application.StartupPath + "\\LastSession.txt";
        private readonly string[] Arguments;
        public MDIDBStudioLite(string[] args)
        {
            Arguments = args;
            InitializeComponent();

            splitContainerDB.SplitterDistance = 120;

            objConnections = new frmConnections();
        }
        private async void MDIParent1_Activated(object sender, EventArgs e)
        {
            if (bFirstTime)
            {
                object objConnection;
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", out objConnection);
                if (objConnection != null)
                    if (!objConnections.SetConnectionName((string)objConnection))
                    {

                    }

                object lastSessionSelectedDBNameObj = null;
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "DB", out lastSessionSelectedDBNameObj);
                if (lastSessionSelectedDBNameObj != null) lastSessionSelectedDBName = (string)lastSessionSelectedDBNameObj;

                object objLoadPreviousSession; //lState,
                (new WindowsRegistry()).ReadValue(Microsoft.Win32.Registry.CurrentUser, @"Options", "LoadPreviousSession", out objLoadPreviousSession);
                if (objLoadPreviousSession != null) frmOptions.chkLoadPreviousSession.Checked = Boolean.Parse((string)objLoadPreviousSession);

                bFirstTime = false;
                //this is  required as setting the connection from the registry is NOT automatically triggers the refresh
                await RefreshDBs();
                RefreshSnippets();

                RefreshConnectionsMenu();

                LoadFilesFromArray(Arguments); //open the files passed as command line parameters

                if (frmOptions.chkLoadPreviousSession.Checked && File.Exists(lastSessionFilesPath))
                {
                    var filePathArry = File.ReadAllLines(lastSessionFilesPath);
                    filePathArry = filePathArry.Except(Arguments).ToArray(); //remove the entries if they are already open
                    LoadFilesFromArray(filePathArry);
                }
            }
        }

        private void LoadFilesFromArray(string[] filePathArry)
        {
            foreach (var filePath in filePathArry)
            {
                if (File.Exists(filePath))
                {
                    OpenFile(filePath);
                }
            }
        }

        public void ShowProgress(bool IsSchemaUpdates = true)
        { 
            if (progressBar1.Visible) return; //if it is already visible just exit
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 100;
            if(IsSchemaUpdates)
                progressBar1.Left = (tabDatabase.Width - progressBar1.Width) / 2; //center align to Schema
            else
                progressBar1.Left = (this.ClientSize.Width - progressBar1.Width) / 2; //center align to MDI
            progressBar1.Top = (this.ClientSize.Height - progressBar1.Height) / 2;
            progressBar1.Visible = true;
        }

        public void StopProgress()
        {
            if (!progressBar1.Visible) return; //if it is NOT already visible just exit
            progressBar1.Visible = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.MarqueeAnimationSpeed = 0;
        }

        //remove not required
        //private void ObjConnections_ConnectionChanged(object sender, EventArgs e)
        //{
        //    currentConnectionToolStripMenuItem.Text = "Connections [" + objConnections.ConnectionName + "]";
        //    if (string.IsNullOrWhiteSpace(objConnections.GetConnectionString())) //don't use sMasterConnectionString or ConnectionName
        //        StopProgress(); //stop further operations.
        //    else
        //    {
        //        RefreshDBs();
        //    }
        //}

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
            return (FrmEditor)CreateNewForm("");
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
            // Make it a child of this MDI form before showing it.
            if (string.IsNullOrWhiteSpace(sFileName)) childForm.Text = "Untitled " + QueryWindowNumber.ToString();
            //set all properties before setting mdiparent this addes the tab to tab control.
            childForm.MdiParent = this;
            childForm.WindowState = FormWindowState.Maximized;
            childForm.Show();

            QueryWindowNumber++;

            return childForm;
        }

        private async Task RefreshDBs()
        {
            try
            {
                //Refresh the schema automatically
                if (string.IsNullOrWhiteSpace(objConnections.GetConnectionString())) //don't use sMasterConnectionString
                {
                    StopProgress();
                    return;
                }

                ShowProgress();
                listViewDBs.Clear();
                listViewDBs.Columns.Add("Name", 130, HorizontalAlignment.Left);
                listViewDBs.Columns.Add("IsSystem", 100, HorizontalAlignment.Left);
                listViewDBs.Columns.Add("Created", 180, HorizontalAlignment.Left);
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sMasterConnectionString))
                {
                    string sQuery = DataObj.GetAllDBsCode;
                    DataObj.SetValues(sQuery, true, CommandType.Text);
                    var ds = await DataObj.Execute("MyTable");
                    //txtOutputText.Text = DataObj.SQLInfoMessageBuilder.ToString();
                    if (ds != null)
                    {
                        if (ds.Tables["MyTable"] == null)
                        {
                            splitContainerDB.Panel1Collapsed = true;
                        }
                        else
                        {
                            splitContainerDB.Panel1Collapsed = false;

                            bool selected = false;
                            LoadListViewFromTable(listViewDBs, ds.Tables["MyTable"]);
                            if (!string.IsNullOrWhiteSpace(lastSessionSelectedDBName))
                            {
                                for (var i = 0; i < listViewDBs.Items.Count; i++)
                                {
                                    if (listViewDBs.Items[i].SubItems[0].Text == lastSessionSelectedDBName)
                                    {
                                        selected = true;
                                        listViewDBs.Items[i].Selected = true;
                                        break;
                                    }
                                }
                            }
                            if (!selected)
                            {
                                //set non system db as default db
                                for (var i = 0; i < listViewDBs.Items.Count; i++)
                                {
                                    if (listViewDBs.Items[i].SubItems[1].Text != "True")
                                    {
                                        listViewDBs.Items[i].Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        //Always refresh the schema as some DB engines does not support multiple dbs
                        await GetMetaData();
                    }
                    else
                    {
                        StopProgress();
                        var message = DataObj.ErrorText.Length > 0 ? DataObj.ErrorText : "The refresh operation failed.";
                        ShowMessageText.Show(message, "Error", this);
                    }
                }
            }
            catch (Exception)
            {
                StopProgress();
            }
        }


        private void RefreshSnippets()
        {
            dtSnippetsTable = Common.LoadJsonToTable<Snippets>(snippetFilePath);

            listViewSnippets.Clear();
            listViewSnippets.Columns.Add("Snippet", 180, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("Query", 0, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("IsExecutable", 50, HorizontalAlignment.Left);
            listViewSnippets.Columns.Add("TiedDB", 80, HorizontalAlignment.Left);
            LoadListViewFromTable(listViewSnippets, dtSnippetsTable);
        }

        private void RefreshConnectionsMenu()
        {
            while (currentConnectionToolStripMenuItem.DropDownItems.Count > fixedConnectionsMenu)
                currentConnectionToolStripMenuItem.DropDownItems.RemoveAt(fixedConnectionsMenu);
            //contextConnections.Items.Clear();

            for (int i = 0; i < objConnections.sConnectionCaptions.Count; i++)
            {
                ToolStripMenuItem childmenu = new ToolStripMenuItem();
                childmenu.Text = (string)objConnections.sConnectionCaptions[i];
                childmenu.Tag = i;
                currentConnectionToolStripMenuItem.DropDownItems.Add(childmenu);
            }
            if (objConnections.GetSelectedIndex() >= 0)
            {
                //ToolStripMenuItem childmenu1 = (ToolStripMenuItem)contextConnections.Items[objConnections.SelectedIndex];
                ToolStripMenuItem childmenu1 = (ToolStripMenuItem)
                    currentConnectionToolStripMenuItem.DropDownItems[objConnections.GetSelectedIndex() + fixedConnectionsMenu];
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
                if (confirmResult == DialogResult.Yes)
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

        private async Task GetMetaData()
        {
            ShowProgress();

            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                string sQuery = DataObj.GetAllSchemaCode + ";" + DataObj.GetAllDBModulesCode;
                DataObj.SetValues(sQuery, true, CommandType.Text);
                if (await DataObj.Execute(new dlgReaderOpen(ReaderEvent)) == false)
                {
                    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
                else
                {
                    RefreshDBStructure();
                }
            }
            StopProgress();
        }

        private void ReaderEvent(object sender, object objReader)
        {
            var Reader = (IDataReader)objReader;
            dtTablesViews = new DataTable();
            dtTablesViews.Load(Reader);
            //Reader.NextResult(); Load automatically moves to next result set
            dtProcedures = new DataTable();
            dtProcedures.Load(Reader);
        }

        private void RefreshDBStructure()
        {
            if (dtTablesViews.Rows.Count > 0)
            {
                // Add  to the ListView.
                lstTables.Clear();
                lstTables.Columns.Add("Table Name", 220, HorizontalAlignment.Left);
                lstTables.Columns.Add("Schema", 50, HorizontalAlignment.Left);
                lstTables.Columns.Add("Type", 130, HorizontalAlignment.Left);
                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                    dtTablesViews.DefaultView.RowFilter = "[TABLE_NAME] LIKE '*" + txtFilter.Text.Trim() + "*'";
                else
                    dtTablesViews.DefaultView.RowFilter = "";
                LoadListViewFromTable(lstTables, dtTablesViews);
            }

            lstProcedures.Clear();
            if (dtProcedures.Rows.Count > 0)
            {
                //Process next resultset
                // Add the columns to the ListView.

                lstProcedures.Columns.Add("Module Name", 200, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Schema", 50, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Type", 50, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Modified", 180, HorizontalAlignment.Left);
                lstProcedures.Columns.Add("Created", 180, HorizontalAlignment.Left);
                //            lstProcedures.Columns.Add("Definition", 80, HorizontalAlignment.Left);

                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                    dtProcedures.DefaultView.RowFilter = "[ModuleName] LIKE '*" + txtFilter.Text.Trim() + "*'";
                else
                    dtProcedures.DefaultView.RowFilter = "";

                //dtProcedures.DefaultView.Sort = "[ModuleName]"; order is included in query
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
                if (lstTables.SelectedItems.Count > 0) contextTablesViews.Show(lstTables, e.Location);
        }

        private void lstConnections_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextConnection.Show(objConnections, e.Location);
        }

        private void lstTables_DoubleClick(object sender, EventArgs e)
        {
            showTop100ReverseToolStripMenuItem_Click(sender, e);
            //getColumnListToolStripMenuItem_Click(sender, e);
            //LoadStructure();
        }
        private async void viewStructureToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            await LoadStructure();
        }


        private string GetSelectedSPName()
        {
            if (lstProcedures.SelectedItems.Count > 0)
            {
                return "[" + lstProcedures.SelectedItems[0].SubItems[colSchema].Text + "].[" +
                       lstProcedures.SelectedItems[0].SubItems[colName].Text + "]";
            }
            else
                return "";
        }

        private string GetSelectedCodeType()
        {
            if (lstProcedures.SelectedItems.Count > 0)
            {
                return GetDynamicDataSourceCode().GetSelectedCodeType(lstProcedures.SelectedItems[0].SubItems[colType].Text).Trim();
            }
            else
                return "";
        }
        private string GetSelectedCodeRawType()
        {
            if (lstProcedures.SelectedItems.Count > 0)
            {
                return lstProcedures.SelectedItems[0].SubItems[colType].Text.Trim();
            }
            else
                return "";
        }

        private string GetSelectedTableType()
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                return lstTables.SelectedItems[0].SubItems[colType].Text.ToUpper()
                    == GetDynamicDataSourceCode().BaseTableType
                    ? "TABLE" : "VIEW";
            }
            else
                return "TABLE";
        }
        private string GetSelectedTableNameWithSchema()
        {
            if (lstTables.SelectedItems.Count > 0)
            {
                return "[" + lstTables.SelectedItems[0].SubItems[colSchema].Text + "].[" +
                       lstTables.SelectedItems[0].SubItems[colName].Text + "]";
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
                if (lstTables.SelectedItems[0].SubItems[colType].Text.ToUpper() == GetDynamicDataSourceCode().BaseTableType)
                {
                    var dataForm = GetEmptyOperatingForm();
                    dataForm.SetDataGrid(await GetDynamicDataSourceCode().GetTableDetails(stablenameonly));
                    dataForm.FocusDataResults();
                }

                else
                {
                    await GetEmptyOperatingFormSetProcedureDefinition(sTableName);
                }
            }
        }

        private async Task GetTableRows(int Rows, bool isReverse = false, string columnList = null)
        {
            string sTableName = GetSelectedTableNameWithSchema();
            if (sTableName != "")
            {
                string sQuery = GetDynamicDataSourceCode().GetTableRowsCode(sTableName, Rows, isReverse, columnList);
                await GetEmptyOperatingForm().LoadQuery(sQuery);
            }
        }

        //Event handlers are the only place you are allowed to do async void.
        //https://stackoverflow.com/questions/25456194/winforms-call-to-async-method-hangs-up-program
        private async void getColumnListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sTableName = GetSelectedTableName();

            if (sTableName != "")
            {
                string columnList = await GetDynamicDataSourceCode().GetColumnList(sConnectionString, sTableName);
                await GetTableRows(100, false, columnList);
            }
        }

        private async void showTop100ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            await GetTableRows(100);
        }

        private async void showTop100ReverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sTableName = GetSelectedTableName();

            if (sTableName != "")
            {
                string columnList = await GetDynamicDataSourceCode().GetColumnList(sConnectionString, sTableName);
                await GetTableRows(100, true, columnList);
            }
        }

        private async void showFieldHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await GetTableRows(0);
        }

        private async void showTop1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await GetTableRows(1);
        }

        private async void showTop10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await GetTableRows(10);
        }
        private async void showAllToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            await GetTableRows(-1);
        }

        private void lstProcedures_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (lstProcedures.SelectedItems.Count > 0) contextCode.Show(lstProcedures, e.Location);
        }

        private async void TS_Procedure_Code_Click_1(object sender, EventArgs e)
        {
            await getSelectedProcedureCode();
        }

        private async void TS_Procedure_Run_Click_1(object sender, EventArgs e)
        {
            string codeType = GetSelectedCodeRawType();
            if (string.IsNullOrWhiteSpace(codeType)) return;

            //Executable code 
            if (GetDynamicDataSourceCode().executableType.Contains(codeType))
            {
                string sSPName = GetSelectedSPName();
                if (sSPName != "")
                {
                    GetEmptyOperatingFormSetProcedureRunText(sSPName, codeType);
                }
            }
            else
            {
                await getSelectedProcedureCode();
            }
        }

        private void GetEmptyOperatingFormSetProcedureRunText(string sSPName, string codeType)
        {
            var error = string.Empty;
            var code = GetDynamicDataSourceCode().GetProcedureRun(sConnectionString, sSPName, codeType, out error);
            if (!string.IsNullOrWhiteSpace(error))
                MessageBox.Show("Error occured" + error, Application.ProductName,
                    MessageBoxButtons.OK);
            else
            {
                GetEmptyOperatingForm().SetProcedureText(code);
            }
        }
        private void GetEmptyOperatingFormSetProcedureCSharpCode(string sSPName, int type)
        {
            var error = string.Empty;
            var code = CodeGeneration.GetProcedureCSharpCode(sConnectionString, sSPName, type, out error);
            //always set the code before refactoring as well. this is fixed
            if (!string.IsNullOrWhiteSpace(error))
                MessageBox.Show("Error occured" + error, Application.ProductName,
                    MessageBoxButtons.OK);
            else
            {
                GetEmptyOperatingForm().SetProcedureText(code);
            }
        }
        private async Task GetEmptyOperatingFormSetProcedureDefinition(string sTableName)
        {
            var error = string.Empty;
            var codeTuple = await GetDynamicDataSourceCode().GetProcedureDefinition(sConnectionString, sTableName);
            if (!string.IsNullOrWhiteSpace(error))
                MessageBox.Show("Error occured" + error, Application.ProductName,
                    MessageBoxButtons.OK);
            else
            {
                GetEmptyOperatingForm().SetProcedureText(codeTuple.Item1);
            }
        }

        private void TS_Procedure_Structure_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingFormSetProcedureCSharpCode(sSPName, 1);
        }
        private void getCReaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingFormSetProcedureCSharpCode(sSPName, 2);
        }

        private void getCSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingFormSetProcedureCSharpCode(sSPName, 3);
        }

        private void getCScalarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
                GetEmptyOperatingFormSetProcedureCSharpCode(sSPName, 4);
        }

        private async void connectionsToolStripMenuItem_DropDownItemClicked(object sender,
            ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag is int)
            {
                ToolStripMenuItem childmenu1;
                int previousSelectedIndex = objConnections.GetSelectedIndex();
                if (previousSelectedIndex >= 0)
                {
                    childmenu1 = (ToolStripMenuItem)currentConnectionToolStripMenuItem.
                        DropDownItems[previousSelectedIndex + fixedConnectionsMenu];
                    childmenu1.Checked = false;
                }

                if (objConnections.SetConnectionName(e.ClickedItem.Text))
                {
                    childmenu1 = (ToolStripMenuItem)e.ClickedItem;
                    childmenu1.Checked = true;
                    await RefreshDBs();
                }
                else
                {
                    MessageBox.Show($"Connection '{e.ClickedItem.Text}' is not avilable.");
                    //todo remove/refresh entries if it could not set the name?
                }
            }
        }

        private async void lstProcedures_DoubleClick(object sender, EventArgs e)
        {
            await getSelectedProcedureCode();
        }
        private async Task getSelectedProcedureCode()
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
            {
                await GetEmptyOperatingFormSetProcedureDefinition(sSPName);
            }
        }

        private void MDIParent1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                foreach (TabPage tabPage in tabForms.TabPages)
                {
                    var operatingForm = tabPage.Tag as IOperatingForm;
                    if (operatingForm != null && operatingForm != snippetsEditor)
                        sb.AppendLine(operatingForm.FileName);
                }
                File.WriteAllText(lastSessionFilesPath, sb.ToString());

                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"Options", "LoadPreviousSession", frmOptions.chkLoadPreviousSession.Checked.ToString());
                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "Connection", objConnections.GetConnectionName());
                (new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "DB", GetSelectedDBName());
                //(new WindowsRegistry()).WriteValue(Microsoft.Win32.Registry.CurrentUser, @"MyQuery", "CheckState",chkConnection.Checked.ToString());
            }
            catch (Exception ex)
            {
                ShowMessageText.Show("Unexpected error occurred " + Environment.NewLine + ex.ToString(), "Close Error", this);
            }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objConnections.ShowDialog(this);
            RefreshConnectionsMenu();

        }


        private async void listViewSnippets_DoubleClick(object sender, EventArgs e)
        {
            await ExecuteSnippet();
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
        public string GetSelectedDBName()
        {
            if (listViewDBs.SelectedItems.Count > 0)
            {
                return listViewDBs.SelectedItems[0].SubItems[0].Text;
            }
            else
                return "";
        }
        public string GetSelectedConnection()
        {
            return objConnections.GetConnectionName();
        }

        private void toolstripEditSnippet_Click(object sender, EventArgs e)
        {
            if (snippetsEditor == null || snippetsEditor.IsDisposed)
            {
                snippetsEditor = (DataTableEditor)CreateNewForm(snippetFilePath, FormTypes.JsonEditor, 1);
            }
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
                toolstripLoadSnippet.Visible = toolstripExecuteSnippet.Visible = IsSnippetSelected;
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
                string snippetCode = (string)GetSnippetRow(snippetName)[1];
                var operatingForm = GetEmptyOperatingForm();
                operatingForm.SetProcedureText(snippetCode);
            }
        }

        private async void toolstripExecuteSnippet_Click(object sender, EventArgs e)
        {
            await ExecuteSnippet();
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
        private void MDIDBStudioLite_MdiChildActivate(object sender, EventArgs e)
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
            if (index > 0)
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
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnFilter_Click(sender, e);
            }
        }

        private async void refreshDatabasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await RefreshDBs();
        }

        private async void refreshSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (objConnections.GetConnectionString() == "") return;
            await GetMetaData(); //chkConnection.Checked
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            objAboutMe.ShowDialog(this);

        }

        private void databasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            databasesToolStripMenuItem.Checked = !databasesToolStripMenuItem.Checked;
            splitContainerDB.Panel1Collapsed = !databasesToolStripMenuItem.Checked;
        }

        private void tablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tablesToolStripMenuItem.Checked = !tablesToolStripMenuItem.Checked;
            splitContainerSchema.Panel1Collapsed = !tablesToolStripMenuItem.Checked;
        }

        private void codeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeToolStripMenuItem.Checked = !codeToolStripMenuItem.Checked;
            splitContainerSchema.Panel2Collapsed = !codeToolStripMenuItem.Checked;
        }

        private void listViewDBs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lastSessionSelectedDBName = GetSelectedDBName();
        }
        private void LoadModuleDropCode()
        {
            string sSPName = GetSelectedSPName();
            if (sSPName != "")
            {
                var codeType = GetSelectedCodeType();
                var code = GetDynamicDataSourceCode().GetDropCode(sSPName, codeType);
                LoadCode(code);
            }
        }

        private void getDropCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string objectName = GetSelectedTableName();
            if (objectName != "")
            {
                var objectType = GetSelectedTableType();
                var code = GetDynamicDataSourceCode().GetDropCode(objectName, objectType);
                LoadCode(code);
            }
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadModuleDropCode();
        }

        private void getInsertCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDMLCode(SQLCommandType.Insert);
        }

        private void getUpdateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDMLCode(SQLCommandType.Update);
        }

        private void getDeleteCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDMLCode(SQLCommandType.Delete);
        }

        private void LoadDMLCode(SQLCommandType sQLCommandType)
        {
            string objectName = GetSelectedTableName();
            if (objectName != "")
            {
                var objectType = GetSelectedTableType();
                var code = GetDynamicDataSourceCode().GetDMLCode(objectName, objectType, sQLCommandType);
                LoadCode(code);
            }
        }

        private void LoadCode(string sql)
        {
            GetEmptyOperatingForm().SetProcedureText(sql);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOptions.ShowDialog();
        }

        private void splitContainerSchema_Resize(object sender, EventArgs e)
        {
            if (splitContainerSchema.Height > 0)
            {
                splitContainerSchema.SplitterDistance = splitContainerSchema.Height / 2;
            }
        }

    }
}