namespace DBStudioLite
{
    partial class FrmEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitCode = new System.Windows.Forms.SplitContainer();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.incrementalSearcher1 = new ScintillaNET_FindReplaceDialog.IncrementalSearcher();
            this.txtQuery = new ScintillaNET.Scintilla();
            this.tabstripResults = new System.Windows.Forms.TabControl();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.tabMessages = new System.Windows.Forms.TabPage();
            this.txtOutputText = new System.Windows.Forms.TextBox();
            this.tabFindResults = new System.Windows.Forms.TabPage();
            this.findAllResultsPanel1 = new ScintillaNET_FindReplaceDialog.FindAllResults.FindAllResultsPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRows = new System.Windows.Forms.Label();
            this.checkShowFullScreen = new System.Windows.Forms.CheckBox();
            this.butExcel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.Paste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExecuteAll = new System.Windows.Forms.Button();
            this.butExecute = new System.Windows.Forms.Button();
            this.findReplace1 = new ScintillaNET_FindReplaceDialog.FindReplace();
            this.butExecuteAllDBs = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCode)).BeginInit();
            this.splitCode.Panel1.SuspendLayout();
            this.splitCode.Panel2.SuspendLayout();
            this.splitCode.SuspendLayout();
            this.tabstripResults.SuspendLayout();
            this.tabResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tabMessages.SuspendLayout();
            this.tabFindResults.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.splitCode, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.2381F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1514, 1050);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // splitCode
            // 
            this.splitCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCode.Location = new System.Drawing.Point(4, 55);
            this.splitCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitCode.Name = "splitCode";
            this.splitCode.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCode.Panel1
            // 
            this.splitCode.Panel1.Controls.Add(this.progressBar1);
            this.splitCode.Panel1.Controls.Add(this.incrementalSearcher1);
            this.splitCode.Panel1.Controls.Add(this.txtQuery);
            // 
            // splitCode.Panel2
            // 
            this.splitCode.Panel2.Controls.Add(this.tabstripResults);
            this.splitCode.Size = new System.Drawing.Size(1506, 990);
            this.splitCode.SplitterDistance = 475;
            this.splitCode.SplitterWidth = 6;
            this.splitCode.TabIndex = 19;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBar1.Location = new System.Drawing.Point(666, 412);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(184, 34);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 25;
            this.progressBar1.Visible = false;
            // 
            // incrementalSearcher1
            // 
            this.incrementalSearcher1.AutoPosition = true;
            this.incrementalSearcher1.AutoSize = true;
            this.incrementalSearcher1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.incrementalSearcher1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.incrementalSearcher1.FindReplace = null;
            this.incrementalSearcher1.Location = new System.Drawing.Point(575, 0);
            this.incrementalSearcher1.Margin = new System.Windows.Forms.Padding(0);
            this.incrementalSearcher1.Name = "incrementalSearcher1";
            this.incrementalSearcher1.Scintilla = null;
            this.incrementalSearcher1.Size = new System.Drawing.Size(340, 28);
            this.incrementalSearcher1.TabIndex = 8;
            this.incrementalSearcher1.ToolItem = false;
            this.incrementalSearcher1.Visible = false;
            // 
            // txtQuery
            // 
            this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(1506, 475);
            this.txtQuery.TabIndex = 0;
            this.txtQuery.TextChanged += new System.EventHandler(this.txtQuery1_TextChanged);
            this.txtQuery.Enter += new System.EventHandler(this.txtQuery_Enter);
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
            // 
            // tabstripResults
            // 
            this.tabstripResults.Controls.Add(this.tabResults);
            this.tabstripResults.Controls.Add(this.tabMessages);
            this.tabstripResults.Controls.Add(this.tabFindResults);
            this.tabstripResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabstripResults.Location = new System.Drawing.Point(0, 0);
            this.tabstripResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabstripResults.Name = "tabstripResults";
            this.tabstripResults.SelectedIndex = 0;
            this.tabstripResults.Size = new System.Drawing.Size(1506, 509);
            this.tabstripResults.TabIndex = 9;
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.dataGrid1);
            this.tabResults.Location = new System.Drawing.Point(4, 29);
            this.tabResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabResults.Size = new System.Drawing.Size(1498, 476);
            this.tabResults.TabIndex = 0;
            this.tabResults.Text = "Data Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowUserToAddRows = false;
            this.dataGrid1.AllowUserToDeleteRows = false;
            this.dataGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(3, 4);
            this.dataGrid1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowHeadersWidth = 62;
            this.dataGrid1.RowTemplate.Height = 23;
            this.dataGrid1.Size = new System.Drawing.Size(1492, 468);
            this.dataGrid1.TabIndex = 10;
            this.dataGrid1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGrid1_DataError_1);
            // 
            // tabMessages
            // 
            this.tabMessages.Controls.Add(this.txtOutputText);
            this.tabMessages.Location = new System.Drawing.Point(4, 29);
            this.tabMessages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabMessages.Name = "tabMessages";
            this.tabMessages.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabMessages.Size = new System.Drawing.Size(1497, 476);
            this.tabMessages.TabIndex = 1;
            this.tabMessages.Text = "Messages";
            this.tabMessages.UseVisualStyleBackColor = true;
            // 
            // txtOutputText
            // 
            this.txtOutputText.AcceptsReturn = true;
            this.txtOutputText.AcceptsTab = true;
            this.txtOutputText.AllowDrop = true;
            this.txtOutputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutputText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputText.HideSelection = false;
            this.txtOutputText.Location = new System.Drawing.Point(3, 4);
            this.txtOutputText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOutputText.Multiline = true;
            this.txtOutputText.Name = "txtOutputText";
            this.txtOutputText.ReadOnly = true;
            this.txtOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutputText.Size = new System.Drawing.Size(1491, 468);
            this.txtOutputText.TabIndex = 11;
            this.txtOutputText.WordWrap = false;
            // 
            // tabFindResults
            // 
            this.tabFindResults.Controls.Add(this.findAllResultsPanel1);
            this.tabFindResults.Location = new System.Drawing.Point(4, 29);
            this.tabFindResults.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabFindResults.Name = "tabFindResults";
            this.tabFindResults.Size = new System.Drawing.Size(1497, 476);
            this.tabFindResults.TabIndex = 2;
            this.tabFindResults.Text = "Find Results";
            this.tabFindResults.UseVisualStyleBackColor = true;
            // 
            // findAllResultsPanel1
            // 
            this.findAllResultsPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findAllResultsPanel1.Location = new System.Drawing.Point(0, 0);
            this.findAllResultsPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.findAllResultsPanel1.Name = "findAllResultsPanel1";
            this.findAllResultsPanel1.Scintilla = this.txtQuery;
            this.findAllResultsPanel1.Size = new System.Drawing.Size(1497, 476);
            this.findAllResultsPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.butExecuteAllDBs);
            this.panel1.Controls.Add(this.lblRows);
            this.panel1.Controls.Add(this.checkShowFullScreen);
            this.panel1.Controls.Add(this.butExcel);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.Paste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExecuteAll);
            this.panel1.Controls.Add(this.butExecute);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1508, 42);
            this.panel1.TabIndex = 0;
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(970, 11);
            this.lblRows.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(81, 20);
            this.lblRows.TabIndex = 21;
            this.lblRows.Text = "Total rows";
            this.lblRows.Visible = false;
            // 
            // checkShowFullScreen
            // 
            this.checkShowFullScreen.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkShowFullScreen.AutoSize = true;
            this.checkShowFullScreen.Location = new System.Drawing.Point(832, 6);
            this.checkShowFullScreen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkShowFullScreen.Name = "checkShowFullScreen";
            this.checkShowFullScreen.Size = new System.Drawing.Size(117, 30);
            this.checkShowFullScreen.TabIndex = 7;
            this.checkShowFullScreen.Text = "&Show Results";
            this.checkShowFullScreen.UseVisualStyleBackColor = true;
            this.checkShowFullScreen.CheckedChanged += new System.EventHandler(this.checkShowMessages_CheckedChanged);
            // 
            // butExcel
            // 
            this.butExcel.Location = new System.Drawing.Point(693, 6);
            this.butExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butExcel.Name = "butExcel";
            this.butExcel.Size = new System.Drawing.Size(132, 35);
            this.butExcel.TabIndex = 6;
            this.butExcel.Text = "Export to Excel";
            this.butExcel.UseVisualStyleBackColor = true;
            this.butExcel.Click += new System.EventHandler(this.butExcel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(408, 7);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(82, 35);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Paste
            // 
            this.Paste.Location = new System.Drawing.Point(591, 7);
            this.Paste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Paste.Name = "Paste";
            this.Paste.Size = new System.Drawing.Size(94, 35);
            this.Paste.TabIndex = 5;
            this.Paste.Text = "Paste All";
            this.Paste.UseVisualStyleBackColor = true;
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(498, 7);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(85, 35);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "Copy All";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExecuteAll
            // 
            this.btnExecuteAll.Location = new System.Drawing.Point(0, 6);
            this.btnExecuteAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExecuteAll.Name = "btnExecuteAll";
            this.btnExecuteAll.Size = new System.Drawing.Size(112, 35);
            this.btnExecuteAll.TabIndex = 1;
            this.btnExecuteAll.Text = "E&xecute All";
            this.btnExecuteAll.UseVisualStyleBackColor = true;
            this.btnExecuteAll.Click += new System.EventHandler(this.btnExecuteAll_Click);
            // 
            // butExecute
            // 
            this.butExecute.Location = new System.Drawing.Point(120, 4);
            this.butExecute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butExecute.Name = "butExecute";
            this.butExecute.Size = new System.Drawing.Size(112, 35);
            this.butExecute.TabIndex = 2;
            this.butExecute.Text = "&Execute";
            this.butExecute.UseVisualStyleBackColor = true;
            this.butExecute.Click += new System.EventHandler(this.butExecute_Click);
            // 
            // findReplace1
            // 
            this.findReplace1._lastReplaceHighlight = false;
            this.findReplace1._lastReplaceLastLine = 0;
            this.findReplace1._lastReplaceMark = false;
            this.findReplace1.Scintilla = this.txtQuery;
            // 
            // butExecuteAllDBs
            // 
            this.butExecuteAllDBs.Location = new System.Drawing.Point(240, 6);
            this.butExecuteAllDBs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.butExecuteAllDBs.Name = "butExecuteAllDBs";
            this.butExecuteAllDBs.Size = new System.Drawing.Size(139, 35);
            this.butExecuteAllDBs.TabIndex = 22;
            this.butExecuteAllDBs.Text = "Execute All &DBs";
            this.butExecuteAllDBs.UseVisualStyleBackColor = true;
            this.butExecuteAllDBs.Click += new System.EventHandler(this.butExecuteAllDBs_Click);
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1514, 1050);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitCode.Panel1.ResumeLayout(false);
            this.splitCode.Panel1.PerformLayout();
            this.splitCode.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCode)).EndInit();
            this.splitCode.ResumeLayout(false);
            this.tabstripResults.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tabMessages.ResumeLayout(false);
            this.tabMessages.PerformLayout();
            this.tabFindResults.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitCode;
        public System.Windows.Forms.TextBox txtOutputText;
        public System.Windows.Forms.DataGridView dataGrid1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblRows;
        public System.Windows.Forms.Button butExcel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button Paste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExecuteAll;
        private System.Windows.Forms.Button butExecute;
        private ScintillaNET.Scintilla txtQuery;
        private ScintillaNET_FindReplaceDialog.IncrementalSearcher incrementalSearcher1;
        private ScintillaNET_FindReplaceDialog.FindAllResults.FindAllResultsPanel findAllResultsPanel1;
        private ScintillaNET_FindReplaceDialog.FindReplace findReplace1;
        private System.Windows.Forms.TabControl tabstripResults;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.TabPage tabMessages;
        private System.Windows.Forms.TabPage tabFindResults;
        public System.Windows.Forms.CheckBox checkShowFullScreen;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button butExecuteAllDBs;
    }
}

