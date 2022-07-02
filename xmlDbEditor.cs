using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace xmlDbEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class xmlEditor : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbTables;
		private System.Windows.Forms.TextBox edTableName;
		private System.Windows.Forms.Button btnAddTable;
		private System.Windows.Forms.Button btnRemoveTable;
		private System.Windows.Forms.Button btnChangeTableName;
		private System.Windows.Forms.TextBox edColumnName;
		private System.Windows.Forms.ComboBox cbColumnType;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnChangeColumn;
		private System.Windows.Forms.Button btnRemoveColumn;
		private System.Windows.Forms.Button btnAddColumn;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.DataGrid dgData;
		private System.Windows.Forms.ListView lvColumns;
		private System.Windows.Forms.ColumnHeader columnName;
		private System.Windows.Forms.ColumnHeader columnType;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mnuNew;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnuSave;
		private System.Windows.Forms.MenuItem mnuSaveAs;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem mnuExit;

		private DataTable currentTable;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.MenuItem menuAbout;
		private string fnSchema=null;
        private DataSet dataSet = new DataSet();
        private SplitContainer splitContainer1;
        private IContainer components;

		public xmlEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChangeTableName = new System.Windows.Forms.Button();
            this.btnRemoveTable = new System.Windows.Forms.Button();
            this.btnAddTable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.edTableName = new System.Windows.Forms.TextBox();
            this.lbTables = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.cbColumnType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edColumnName = new System.Windows.Forms.TextBox();
            this.btnChangeColumn = new System.Windows.Forms.Button();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnRemoveColumn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgData = new System.Windows.Forms.DataGrid();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuNew = new System.Windows.Forms.MenuItem();
            this.mnuOpen = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuSave = new System.Windows.Forms.MenuItem();
            this.mnuSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.mnuExit = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnChangeTableName);
            this.groupBox1.Controls.Add(this.btnRemoveTable);
            this.groupBox1.Controls.Add(this.btnAddTable);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.edTableName);
            this.groupBox1.Controls.Add(this.lbTables);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tables";
            // 
            // btnChangeTableName
            // 
            this.btnChangeTableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeTableName.Location = new System.Drawing.Point(106, 58);
            this.btnChangeTableName.Name = "btnChangeTableName";
            this.btnChangeTableName.Size = new System.Drawing.Size(124, 27);
            this.btnChangeTableName.TabIndex = 2;
            this.btnChangeTableName.Text = "&Change Name";
            this.btnChangeTableName.Click += new System.EventHandler(this.btnChangeTableName_Click);
            // 
            // btnRemoveTable
            // 
            this.btnRemoveTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveTable.Location = new System.Drawing.Point(20, 92);
            this.btnRemoveTable.Name = "btnRemoveTable";
            this.btnRemoveTable.Size = new System.Drawing.Size(210, 28);
            this.btnRemoveTable.TabIndex = 3;
            this.btnRemoveTable.Text = "&Remove Selected Tables";
            this.btnRemoveTable.Click += new System.EventHandler(this.btnRemoveTable_Click);
            // 
            // btnAddTable
            // 
            this.btnAddTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTable.Location = new System.Drawing.Point(20, 58);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(84, 27);
            this.btnAddTable.TabIndex = 1;
            this.btnAddTable.Text = "&Add Table";
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "Na&me:";
            // 
            // edTableName
            // 
            this.edTableName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edTableName.Location = new System.Drawing.Point(70, 28);
            this.edTableName.Name = "edTableName";
            this.edTableName.Size = new System.Drawing.Size(160, 23);
            this.edTableName.TabIndex = 0;
            // 
            // lbTables
            // 
            this.lbTables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTables.ItemHeight = 17;
            this.lbTables.Location = new System.Drawing.Point(20, 127);
            this.lbTables.Name = "lbTables";
            this.lbTables.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbTables.Size = new System.Drawing.Size(210, 55);
            this.lbTables.TabIndex = 4;
            this.lbTables.TabStop = false;
            this.lbTables.SelectedIndexChanged += new System.EventHandler(this.lbTables_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvColumns);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbColumnType);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.edColumnName);
            this.groupBox2.Controls.Add(this.btnChangeColumn);
            this.groupBox2.Controls.Add(this.btnAddColumn);
            this.groupBox2.Controls.Add(this.btnRemoveColumn);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(4, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(230, 360);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Columns";
            // 
            // lvColumns
            // 
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnType});
            this.lvColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.HideSelection = false;
            this.lvColumns.Location = new System.Drawing.Point(12, 156);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(212, 201);
            this.lvColumns.TabIndex = 5;
            this.lvColumns.TabStop = false;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.SelectedIndexChanged += new System.EventHandler(this.lvColumns_SelectedIndexChanged);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            this.columnType.Width = 90;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "&Type:";
            // 
            // cbColumnType
            // 
            this.cbColumnType.CausesValidation = false;
            this.cbColumnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColumnType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbColumnType.Items.AddRange(new object[] {
            "Boolean",
            "Byte",
            "Char",
            "DateTime",
            "Decimal",
            "Double",
            "Int16",
            "Int32",
            "Int64",
            "String"});
            this.cbColumnType.Location = new System.Drawing.Point(68, 55);
            this.cbColumnType.Name = "cbColumnType";
            this.cbColumnType.Size = new System.Drawing.Size(162, 25);
            this.cbColumnType.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nam&e:";
            // 
            // edColumnName
            // 
            this.edColumnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edColumnName.Location = new System.Drawing.Point(68, 28);
            this.edColumnName.Name = "edColumnName";
            this.edColumnName.Size = new System.Drawing.Size(162, 23);
            this.edColumnName.TabIndex = 0;
            // 
            // btnChangeColumn
            // 
            this.btnChangeColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeColumn.Location = new System.Drawing.Point(106, 87);
            this.btnChangeColumn.Name = "btnChangeColumn";
            this.btnChangeColumn.Size = new System.Drawing.Size(124, 27);
            this.btnChangeColumn.TabIndex = 3;
            this.btnChangeColumn.Text = "Change &Name";
            this.btnChangeColumn.Click += new System.EventHandler(this.btnChangeColumn_Click);
            // 
            // btnAddColumn
            // 
            this.btnAddColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddColumn.Location = new System.Drawing.Point(14, 87);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.Size = new System.Drawing.Size(90, 27);
            this.btnAddColumn.TabIndex = 2;
            this.btnAddColumn.Text = "Add C&olumn";
            this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // btnRemoveColumn
            // 
            this.btnRemoveColumn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveColumn.Location = new System.Drawing.Point(14, 121);
            this.btnRemoveColumn.Name = "btnRemoveColumn";
            this.btnRemoveColumn.Size = new System.Drawing.Size(216, 28);
            this.btnRemoveColumn.TabIndex = 4;
            this.btnRemoveColumn.Text = "Remo&ve Selected Columns";
            this.btnRemoveColumn.Click += new System.EventHandler(this.btnRemoveColumn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgData);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(772, 827);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Data";
            // 
            // dgData
            // 
            this.dgData.DataMember = "";
            this.dgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgData.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgData.Location = new System.Drawing.Point(3, 19);
            this.dgData.Name = "dgData";
            this.dgData.Size = new System.Drawing.Size(766, 805);
            this.dgData.TabIndex = 0;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuAbout});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.menuItem5,
            this.mnuSave,
            this.mnuSaveAs,
            this.menuItem8,
            this.mnuExit});
            this.menuItem1.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Index = 0;
            this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Index = 1;
            this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuOpen.Text = "Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 2;
            this.menuItem5.Text = "-";
            // 
            // mnuSave
            // 
            this.mnuSave.Index = 3;
            this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Index = 4;
            this.mnuSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mnuSaveAs.Text = "Save &As...";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 5;
            this.menuItem8.Text = "-";
            // 
            // mnuExit
            // 
            this.mnuExit.Index = 6;
            this.mnuExit.Shortcut = System.Windows.Forms.Shortcut.F10;
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 1;
            this.menuAbout.Text = "&About!";
            this.menuAbout.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "xml";
            this.saveFileDialog.Filter = "XML Files|*.xml";
            this.saveFileDialog.InitialDirectory = ".\\";
            this.saveFileDialog.Title = "Save Database As...";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "xml";
            this.openFileDialog.Filter = "\"XML Files|*.xml";
            this.openFileDialog.Title = "Open Database...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1094, 827);
            this.splitContainer1.SplitterDistance = 318;
            this.splitContainer1.TabIndex = 3;
            // 
            // xmlEditor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1094, 827);
            this.Controls.Add(this.splitContainer1);
            this.Menu = this.mainMenu1;
            this.Name = "xmlEditor";
            this.Text = "XML Database Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		// ========================== MENU COMMANDS =========================

		private void mnuNew_Click(object sender, System.EventArgs e)
		{
			fnSchema=null;
			lbTables.Items.Clear();
			lvColumns.Items.Clear();
			edTableName.Text="";
			edColumnName.Text="";
			cbColumnType.Text="";
			this.Text="XML Database Editor";
			dataSet=new DataSet();
			dgData.SetDataBinding(dataSet, null);
		}
        public void OpenFile(string fn)
        {
            lbTables.Items.Clear();
            DataSet ds = new DataSet();
            try
            {
                ds.ReadXml(fn);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            dataSet = ds;
            foreach (DataTable dt in ds.Tables)
            {
                lbTables.Items.Add(dt.TableName);
            }
            if (lbTables.Items.Count > 0) lbTables.SelectedIndex = 0;
            fnSchema = fn;
            this.Text = "XML Database Editor - " + fnSchema;
        }
		private void mnuOpen_Click(object sender, System.EventArgs e)
		{
			DialogResult res=openFileDialog.ShowDialog(this);
			if (res==DialogResult.OK)
			{
				string fn=openFileDialog.FileName;
                OpenFile(fn);
			}
		}

		private void mnuSave_Click(object sender, System.EventArgs e)
		{
			if (fnSchema==null)
			{
				mnuSaveAs_Click(sender, e);
			}
			else
			{
				SaveSchema(fnSchema);
				lbTables.SelectedIndex=lbTables.SelectedIndex;		// reselect
			}
		}

		private void mnuSaveAs_Click(object sender, System.EventArgs e)
		{
			DialogResult res=saveFileDialog.ShowDialog(this);
			if (res==DialogResult.OK)
			{
				fnSchema=saveFileDialog.FileName;
				SaveSchema(fnSchema);
				this.Text="XML Database Editor - "+fnSchema;
				lbTables.SelectedIndex=lbTables.SelectedIndex;		// reselect
			}
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("v1.00\nMarc Clifton\nwebmaster@knowledgeautomation.com\n\n" +
                            "v2.00\nSabarinathan Arthanari\ntmasabari@gmail.com"
                , "XML Schema And Data Editor");
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		// ========================= TABLE MANIPULATION ========================

		private void btnAddTable_Click(object sender, System.EventArgs e)
		{
			string tblName=edTableName.Text;
			if (!ValidateTableName(tblName))
			{
				return;
			}
			
			DataTable dt=new DataTable(tblName);
			currentTable=dt;
			lbTables.Items.Add(tblName);
			dataSet.Tables.Add(dt);
			lbTables.SelectedItem=lbTables.Items[lbTables.FindStringExact(tblName)];
		}

		private void btnChangeTableName_Click(object sender, System.EventArgs e)
		{
			string tblName=edTableName.Text;
			if ( (!ValidateTableName(tblName)) || (!ValidateSelectedTable()) )
			{
				return;
			}

			int n=lbTables.Items.IndexOf(lbTables.SelectedItem);
			string oldTableName=lbTables.Items[n].ToString();
			dataSet.Tables[oldTableName].TableName=tblName;
			lbTables.Items[n]=tblName;
			dgData.SetDataBinding(dataSet, tblName);
		}

		private void btnRemoveTable_Click(object sender, System.EventArgs e)
		{
			if (!ValidateSelectedTable())
			{
				return;
			}
            int iOldposition = lbTables.Items.IndexOf(lbTables.SelectedItems[0]);

            object[] objArray = new object[lbTables.SelectedItems.Count];
            lbTables.SelectedItems.CopyTo(objArray, 0);
            foreach (Object lvi in objArray)
            {
                int n = lbTables.Items.IndexOf(lvi);
                string tblName = lbTables.Items[n].ToString();

                // *** the data grid doesn't seem to remove the table! ***			
                DataTable dt = dataSet.Tables[tblName];
                dt.Clear();
                dt.Columns.Clear();
                dataSet.Tables.Remove(tblName);
                // *******************************************************

                lbTables.Items.Remove(lvi);
            }
            lbTables.Refresh();

            if (lbTables.Items.Count > 0)
            {
                if (iOldposition >= lbTables.Items.Count) 
                    iOldposition = lbTables.Items.Count-1;
                //else if (iOldposition > 0)
                //    iOldposition--;

                lbTables.SelectedIndex = iOldposition;
			}
			currentTable=null;
			dgData.SetDataBinding(dataSet, null);
		}

		private void lbTables_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string tblName=lbTables.Text;
            edTableName.Text = tblName;
			currentTable=dataSet.Tables[tblName];
			ShowColumns();
			dgData.SetDataBinding(dataSet, tblName);
			dgData.CaptionText="Table: "+tblName;
		}

		// ========================= COLUMN MANIPULATION ========================

		private void btnAddColumn_Click(object sender, System.EventArgs e)
		{
			string colName=edColumnName.Text;
			string colType=cbColumnType.Text;
			if ( (!ValidateColumnNameAndType(colName, colType)) || (!ValidateSelectedTable()) )
			{
				return;
			}
			ListViewItem lvi=lvColumns.Items.Add(colName);
			lvi.SubItems.Add(colType);
			currentTable.Columns.Add(colName, Type.GetType("System."+colType));
		}

		private void btnChangeColumn_Click(object sender, System.EventArgs e)
		{
			if (!ValidateSelectedColumn())
			{
				return;
			}
			ListViewItem lvi=lvColumns.SelectedItems[0];
			string prevColName=lvi.Text;
			lvi.Text=edColumnName.Text;
			currentTable.Columns[prevColName].ColumnName=edColumnName.Text;

			// can't change the data type once data exists
//			lvi.SubItems[1].Text=cbColumnType.Text;
//			currentTable.Columns[edColumnName.Text].DataType=Type.GetType("System."+cbColumnType.Text);
		}

        private void lvColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvColumns.SelectedItems.Count > 0)
            {
                edColumnName.Text = lvColumns.SelectedItems[0].Text;
                cbColumnType.Text = lvColumns.SelectedItems[0].SubItems[1].Text;
            }

        }

		private void btnRemoveColumn_Click(object sender, System.EventArgs e)
		{
			if (!ValidateSelectedColumn())
			{
				return;
			}
            foreach (ListViewItem lvi in lvColumns.SelectedItems)
            {
                currentTable.Columns.Remove(lvi.Text);
                lvColumns.Items.Remove(lvi);
            }
            lvColumns.Refresh();
            if (lvColumns.Items.Count > 0) lvColumns.Items[0].Selected = true;
		}

		// ========================= HELPERS ========================

		bool ValidateTableName(string tblName)
		{
			if (tblName=="")
			{
				MessageBox.Show("Please enter a name for the table.", "Missing Information");
				return false;
			}
			int ret=lbTables.FindStringExact(tblName);
			if (ret!=ListBox.NoMatches)
			{
				MessageBox.Show("This table is already defined.", "Duplicate Name");
				return false;
			}
			return true;
		}

		bool ValidateSelectedTable()
		{
			if (lbTables.SelectedItem==null)
			{
				MessageBox.Show("Please select a table from the table list.", "No Selection");
				return false;
			}
			return true;
		}

		bool ValidateColumnNameAndType(string colName, string colType)
		{
			if (colName=="")
			{
				MessageBox.Show("Please enter a name for the column.", "Missing Information");
				return false;
			}
			if (colType=="")
			{
				MessageBox.Show("Please select a type for the column.", "Missing Information");
				return false;
			}

			if (GetLVI(colName)!=null)
			{
				MessageBox.Show("This column is already defined.", "Duplicate Name");
				return false;
			}
			return true;
		}

		bool ValidateSelectedColumn()
		{
			if (lvColumns.SelectedItems.Count==0)
			{
				MessageBox.Show("Please select a column from the column list.", "No Selection");
				return false;
			}
			return true;
		}

		ListViewItem GetLVI(string colName)
		{
			ListView.ListViewItemCollection lvItems=lvColumns.Items;
			foreach (ListViewItem lvItem in lvItems)
			{
				if (lvItem.Text==colName)
				{
					return lvItem;
				}
			}
			return null;
		}

		void ShowColumns()
		{
			lvColumns.Items.Clear();
			if (currentTable != null)
			{
				foreach (DataColumn dc in currentTable.Columns)
				{
					ListViewItem lvi=lvColumns.Items.Add(dc.ColumnName);
					string s=dc.DataType.ToString();
					s=s.Split(new Char[] {'.'})[1];
					lvi.SubItems.Add(s);
				}
			}
		}
		
		void SaveSchema(string fn)
		{
			dataSet.WriteXml(fn, XmlWriteMode.WriteSchema);
			MessageBox.Show("Schema and data saved", "Done");
		}

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

	}
}
