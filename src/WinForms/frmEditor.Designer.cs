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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            splitCode = new System.Windows.Forms.SplitContainer();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            incrementalSearcher1 = new ScintillaNET_FindReplaceDialog.IncrementalSearcher();
            txtQuery = new ScintillaNET.Scintilla();
            tabstripResults = new System.Windows.Forms.TabControl();
            tabResults = new System.Windows.Forms.TabPage();
            dataGrid1 = new System.Windows.Forms.DataGridView();
            contextDataGrid = new System.Windows.Forms.ContextMenuStrip(components);
            copyWithHeadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tabMessages = new System.Windows.Forms.TabPage();
            txtOutputText = new System.Windows.Forms.TextBox();
            tabFindResults = new System.Windows.Forms.TabPage();
            findAllResultsPanel1 = new ScintillaNET_FindReplaceDialog.FindAllResults.FindAllResultsPanel();
            panel1 = new System.Windows.Forms.Panel();
            butExecuteAllDBs = new System.Windows.Forms.Button();
            lblRows = new System.Windows.Forms.Label();
            checkShowFullScreen = new System.Windows.Forms.CheckBox();
            butExcel = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            Paste = new System.Windows.Forms.Button();
            btnCopy = new System.Windows.Forms.Button();
            btnExecuteAll = new System.Windows.Forms.Button();
            butExecute = new System.Windows.Forms.Button();
            findReplace1 = new ScintillaNET_FindReplaceDialog.FindReplace();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitCode).BeginInit();
            splitCode.Panel1.SuspendLayout();
            splitCode.Panel2.SuspendLayout();
            splitCode.SuspendLayout();
            tabstripResults.SuspendLayout();
            tabResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGrid1).BeginInit();
            contextDataGrid.SuspendLayout();
            tabMessages.SuspendLayout();
            tabFindResults.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(splitCode, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.761905F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 95.2381F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1682, 1050);
            tableLayoutPanel1.TabIndex = 24;
            // 
            // splitCode
            // 
            splitCode.Dock = System.Windows.Forms.DockStyle.Fill;
            splitCode.Location = new System.Drawing.Point(4, 56);
            splitCode.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            splitCode.Name = "splitCode";
            splitCode.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCode.Panel1
            // 
            splitCode.Panel1.Controls.Add(progressBar1);
            splitCode.Panel1.Controls.Add(incrementalSearcher1);
            splitCode.Panel1.Controls.Add(txtQuery);
            // 
            // splitCode.Panel2
            // 
            splitCode.Panel2.Controls.Add(tabstripResults);
            splitCode.Size = new System.Drawing.Size(1674, 988);
            splitCode.SplitterDistance = 474;
            splitCode.SplitterWidth = 8;
            splitCode.TabIndex = 19;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            progressBar1.Location = new System.Drawing.Point(741, 455);
            progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(204, 42);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 25;
            progressBar1.Visible = false;
            // 
            // incrementalSearcher1
            // 
            incrementalSearcher1.AutoPosition = true;
            incrementalSearcher1.AutoSize = true;
            incrementalSearcher1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            incrementalSearcher1.BackColor = System.Drawing.Color.LightSteelBlue;
            incrementalSearcher1.FindReplace = null;
            incrementalSearcher1.Location = new System.Drawing.Point(639, 0);
            incrementalSearcher1.Margin = new System.Windows.Forms.Padding(0);
            incrementalSearcher1.Name = "incrementalSearcher1";
            incrementalSearcher1.Scintilla = null;
            incrementalSearcher1.Size = new System.Drawing.Size(371, 33);
            incrementalSearcher1.TabIndex = 8;
            incrementalSearcher1.ToolItem = false;
            incrementalSearcher1.Visible = false;
            // 
            // txtQuery
            // 
            txtQuery.AutoCMaxHeight = 9;
            txtQuery.BiDirectionality = ScintillaNET.BiDirectionalDisplayType.Disabled;
            txtQuery.CaretLineBackColor = System.Drawing.Color.OldLace;
            txtQuery.CaretLineVisible = true;
            txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            txtQuery.LexerName = null;
            txtQuery.Location = new System.Drawing.Point(0, 0);
            txtQuery.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            txtQuery.Name = "txtQuery";
            txtQuery.ScrollWidth = 1;
            txtQuery.Size = new System.Drawing.Size(1674, 474);
            txtQuery.TabIndents = true;
            txtQuery.TabIndex = 0;
            txtQuery.UseRightToLeftReadingLayout = false;
            txtQuery.WrapMode = ScintillaNET.WrapMode.None;
            txtQuery.TextChanged += txtQuery1_TextChanged;
            txtQuery.Enter += txtQuery_Enter;
            txtQuery.KeyDown += txtQuery_KeyDown;
            // 
            // tabstripResults
            // 
            tabstripResults.Controls.Add(tabResults);
            tabstripResults.Controls.Add(tabMessages);
            tabstripResults.Controls.Add(tabFindResults);
            tabstripResults.Dock = System.Windows.Forms.DockStyle.Fill;
            tabstripResults.Location = new System.Drawing.Point(0, 0);
            tabstripResults.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabstripResults.Name = "tabstripResults";
            tabstripResults.SelectedIndex = 0;
            tabstripResults.Size = new System.Drawing.Size(1674, 506);
            tabstripResults.TabIndex = 9;
            // 
            // tabResults
            // 
            tabResults.Controls.Add(dataGrid1);
            tabResults.Location = new System.Drawing.Point(4, 34);
            tabResults.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabResults.Name = "tabResults";
            tabResults.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabResults.Size = new System.Drawing.Size(1666, 468);
            tabResults.TabIndex = 0;
            tabResults.Text = "Data Results";
            tabResults.UseVisualStyleBackColor = true;
            // 
            // dataGrid1
            // 
            dataGrid1.AllowUserToAddRows = false;
            dataGrid1.AllowUserToDeleteRows = false;
            dataGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid1.ContextMenuStrip = contextDataGrid;
            dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGrid1.Location = new System.Drawing.Point(3, 5);
            dataGrid1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            dataGrid1.Name = "dataGrid1";
            dataGrid1.ReadOnly = true;
            dataGrid1.RowHeadersWidth = 62;
            dataGrid1.RowTemplate.Height = 23;
            dataGrid1.Size = new System.Drawing.Size(1660, 458);
            dataGrid1.TabIndex = 10;
            dataGrid1.DataError += dataGrid1_DataError_1;
            // 
            // contextDataGrid
            // 
            contextDataGrid.ImageScalingSize = new System.Drawing.Size(24, 24);
            contextDataGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copyWithHeadersToolStripMenuItem, copyToolStripMenuItem });
            contextDataGrid.Name = "contextDataGrid";
            contextDataGrid.Size = new System.Drawing.Size(239, 68);
            // 
            // copyWithHeadersToolStripMenuItem
            // 
            copyWithHeadersToolStripMenuItem.Name = "copyWithHeadersToolStripMenuItem";
            copyWithHeadersToolStripMenuItem.Size = new System.Drawing.Size(238, 32);
            copyWithHeadersToolStripMenuItem.Text = "Copy With Headers";
            copyWithHeadersToolStripMenuItem.Click += copyWithHeadersToolStripMenuItem_Click;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(238, 32);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // tabMessages
            // 
            tabMessages.Controls.Add(txtOutputText);
            tabMessages.Location = new System.Drawing.Point(4, 34);
            tabMessages.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabMessages.Name = "tabMessages";
            tabMessages.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabMessages.Size = new System.Drawing.Size(1666, 468);
            tabMessages.TabIndex = 1;
            tabMessages.Text = "Messages";
            tabMessages.UseVisualStyleBackColor = true;
            // 
            // txtOutputText
            // 
            txtOutputText.AcceptsReturn = true;
            txtOutputText.AcceptsTab = true;
            txtOutputText.AllowDrop = true;
            txtOutputText.Dock = System.Windows.Forms.DockStyle.Fill;
            txtOutputText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            txtOutputText.HideSelection = false;
            txtOutputText.Location = new System.Drawing.Point(3, 5);
            txtOutputText.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            txtOutputText.Multiline = true;
            txtOutputText.Name = "txtOutputText";
            txtOutputText.ReadOnly = true;
            txtOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtOutputText.Size = new System.Drawing.Size(1660, 458);
            txtOutputText.TabIndex = 11;
            txtOutputText.WordWrap = false;
            // 
            // tabFindResults
            // 
            tabFindResults.Controls.Add(findAllResultsPanel1);
            tabFindResults.Location = new System.Drawing.Point(4, 34);
            tabFindResults.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            tabFindResults.Name = "tabFindResults";
            tabFindResults.Size = new System.Drawing.Size(1666, 468);
            tabFindResults.TabIndex = 2;
            tabFindResults.Text = "Find Results";
            tabFindResults.UseVisualStyleBackColor = true;
            // 
            // findAllResultsPanel1
            // 
            findAllResultsPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            findAllResultsPanel1.Location = new System.Drawing.Point(0, 0);
            findAllResultsPanel1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            findAllResultsPanel1.Name = "findAllResultsPanel1";
            findAllResultsPanel1.Scintilla = txtQuery;
            findAllResultsPanel1.Size = new System.Drawing.Size(1666, 468);
            findAllResultsPanel1.TabIndex = 12;
            // 
            // panel1
            // 
            panel1.Controls.Add(butExecuteAllDBs);
            panel1.Controls.Add(lblRows);
            panel1.Controls.Add(checkShowFullScreen);
            panel1.Controls.Add(butExcel);
            panel1.Controls.Add(btnClear);
            panel1.Controls.Add(Paste);
            panel1.Controls.Add(btnCopy);
            panel1.Controls.Add(btnExecuteAll);
            panel1.Controls.Add(butExecute);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(3, 5);
            panel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(1676, 40);
            panel1.TabIndex = 0;
            // 
            // butExecuteAllDBs
            // 
            butExecuteAllDBs.Location = new System.Drawing.Point(267, 8);
            butExecuteAllDBs.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            butExecuteAllDBs.Name = "butExecuteAllDBs";
            butExecuteAllDBs.Size = new System.Drawing.Size(154, 44);
            butExecuteAllDBs.TabIndex = 22;
            butExecuteAllDBs.Text = "Execute All &DBs";
            butExecuteAllDBs.UseVisualStyleBackColor = true;
            butExecuteAllDBs.Click += butExecuteAllDBs_Click;
            // 
            // lblRows
            // 
            lblRows.AutoSize = true;
            lblRows.Location = new System.Drawing.Point(1078, 14);
            lblRows.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRows.Name = "lblRows";
            lblRows.Size = new System.Drawing.Size(92, 25);
            lblRows.TabIndex = 21;
            lblRows.Text = "Total rows";
            lblRows.Visible = false;
            // 
            // checkShowFullScreen
            // 
            checkShowFullScreen.Appearance = System.Windows.Forms.Appearance.Button;
            checkShowFullScreen.AutoSize = true;
            checkShowFullScreen.Location = new System.Drawing.Point(924, 8);
            checkShowFullScreen.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            checkShowFullScreen.Name = "checkShowFullScreen";
            checkShowFullScreen.Size = new System.Drawing.Size(126, 35);
            checkShowFullScreen.TabIndex = 7;
            checkShowFullScreen.Text = "&Show Results";
            checkShowFullScreen.UseVisualStyleBackColor = true;
            checkShowFullScreen.CheckedChanged += checkShowMessages_CheckedChanged;
            // 
            // butExcel
            // 
            butExcel.Location = new System.Drawing.Point(770, 8);
            butExcel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            butExcel.Name = "butExcel";
            butExcel.Size = new System.Drawing.Size(147, 44);
            butExcel.TabIndex = 6;
            butExcel.Text = "Export to Excel";
            butExcel.UseVisualStyleBackColor = true;
            butExcel.Click += butExcel_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(453, 9);
            btnClear.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(91, 44);
            btnClear.TabIndex = 3;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // Paste
            // 
            Paste.Location = new System.Drawing.Point(657, 9);
            Paste.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            Paste.Name = "Paste";
            Paste.Size = new System.Drawing.Size(104, 44);
            Paste.TabIndex = 5;
            Paste.Text = "Paste All";
            Paste.UseVisualStyleBackColor = true;
            Paste.Click += Paste_Click;
            // 
            // btnCopy
            // 
            btnCopy.Location = new System.Drawing.Point(553, 9);
            btnCopy.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new System.Drawing.Size(94, 44);
            btnCopy.TabIndex = 4;
            btnCopy.Text = "Copy All";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnExecuteAll
            // 
            btnExecuteAll.Location = new System.Drawing.Point(0, 8);
            btnExecuteAll.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            btnExecuteAll.Name = "btnExecuteAll";
            btnExecuteAll.Size = new System.Drawing.Size(124, 44);
            btnExecuteAll.TabIndex = 1;
            btnExecuteAll.Text = "E&xecute All";
            btnExecuteAll.UseVisualStyleBackColor = true;
            btnExecuteAll.Click += btnExecuteAll_Click;
            // 
            // butExecute
            // 
            butExecute.Location = new System.Drawing.Point(133, 5);
            butExecute.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            butExecute.Name = "butExecute";
            butExecute.Size = new System.Drawing.Size(124, 44);
            butExecute.TabIndex = 2;
            butExecute.Text = "&Execute";
            butExecute.UseVisualStyleBackColor = true;
            butExecute.Click += butExecute_Click;
            // 
            // findReplace1
            // 
            findReplace1._lastReplaceHighlight = false;
            findReplace1._lastReplaceLastLine = 0;
            findReplace1._lastReplaceMark = false;
            findReplace1.Scintilla = txtQuery;
            // 
            // FrmEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1682, 1050);
            Controls.Add(tableLayoutPanel1);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            Name = "FrmEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            FormClosing += FrmMain_FormClosing;
            tableLayoutPanel1.ResumeLayout(false);
            splitCode.Panel1.ResumeLayout(false);
            splitCode.Panel1.PerformLayout();
            splitCode.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitCode).EndInit();
            splitCode.ResumeLayout(false);
            tabstripResults.ResumeLayout(false);
            tabResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGrid1).EndInit();
            contextDataGrid.ResumeLayout(false);
            tabMessages.ResumeLayout(false);
            tabMessages.PerformLayout();
            tabFindResults.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextDataGrid;
        private System.Windows.Forms.ToolStripMenuItem copyWithHeadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    }
}

