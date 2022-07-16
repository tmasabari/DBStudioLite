namespace AdvancedQueryOrganizer
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
            this.txtQuery1 = new ScintillaNET.Scintilla();
            this.txtOutputText = new System.Windows.Forms.TextBox();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkShowMessages = new System.Windows.Forms.CheckBox();
            this.lblRows = new System.Windows.Forms.Label();
            this.butGraph = new System.Windows.Forms.Button();
            this.butExcel = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.Paste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnExecuteAll = new System.Windows.Forms.Button();
            this.butExecute = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCode)).BeginInit();
            this.splitCode.Panel1.SuspendLayout();
            this.splitCode.Panel2.SuspendLayout();
            this.splitCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
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
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.535142F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.46486F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1346, 840);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // splitCode
            // 
            this.splitCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCode.Location = new System.Drawing.Point(4, 58);
            this.splitCode.Margin = new System.Windows.Forms.Padding(4);
            this.splitCode.Name = "splitCode";
            this.splitCode.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitCode.Panel1
            // 
            this.splitCode.Panel1.Controls.Add(this.txtQuery1);
            // 
            // splitCode.Panel2
            // 
            this.splitCode.Panel2.Controls.Add(this.txtOutputText);
            this.splitCode.Panel2.Controls.Add(this.dataGrid1);
            this.splitCode.Size = new System.Drawing.Size(1338, 778);
            this.splitCode.SplitterDistance = 374;
            this.splitCode.SplitterWidth = 5;
            this.splitCode.TabIndex = 19;
            // 
            // txtQuery1
            // 
            this.txtQuery1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtQuery1.Location = new System.Drawing.Point(0, 0);
            this.txtQuery1.Name = "txtQuery1";
            this.txtQuery1.Size = new System.Drawing.Size(1338, 374);
            this.txtQuery1.TabIndex = 12;
            this.txtQuery1.TextChanged += new System.EventHandler(this.txtQuery1_TextChanged);
            // 
            // txtOutputText
            // 
            this.txtOutputText.AcceptsReturn = true;
            this.txtOutputText.AcceptsTab = true;
            this.txtOutputText.AllowDrop = true;
            this.txtOutputText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutputText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputText.HideSelection = false;
            this.txtOutputText.Location = new System.Drawing.Point(0, 0);
            this.txtOutputText.Margin = new System.Windows.Forms.Padding(4);
            this.txtOutputText.Multiline = true;
            this.txtOutputText.Name = "txtOutputText";
            this.txtOutputText.ReadOnly = true;
            this.txtOutputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutputText.Size = new System.Drawing.Size(1338, 399);
            this.txtOutputText.TabIndex = 13;
            this.txtOutputText.Visible = false;
            this.txtOutputText.WordWrap = false;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowUserToAddRows = false;
            this.dataGrid1.AllowUserToDeleteRows = false;
            this.dataGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGrid1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowHeadersWidth = 62;
            this.dataGrid1.RowTemplate.Height = 23;
            this.dataGrid1.Size = new System.Drawing.Size(1338, 399);
            this.dataGrid1.TabIndex = 12;
            this.dataGrid1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGrid1_DataError_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkShowMessages);
            this.panel1.Controls.Add(this.lblRows);
            this.panel1.Controls.Add(this.butGraph);
            this.panel1.Controls.Add(this.butExcel);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.Paste);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.btnExecuteAll);
            this.panel1.Controls.Add(this.butExecute);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1340, 48);
            this.panel1.TabIndex = 0;
            // 
            // checkShowMessages
            // 
            this.checkShowMessages.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkShowMessages.AutoSize = true;
            this.checkShowMessages.Location = new System.Drawing.Point(693, 6);
            this.checkShowMessages.Name = "checkShowMessages";
            this.checkShowMessages.Size = new System.Drawing.Size(117, 26);
            this.checkShowMessages.TabIndex = 24;
            this.checkShowMessages.Text = "Show Messages";
            this.checkShowMessages.UseVisualStyleBackColor = true;
            this.checkShowMessages.CheckedChanged += new System.EventHandler(this.checkShowMessages_CheckedChanged);
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(844, 11);
            this.lblRows.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(69, 16);
            this.lblRows.TabIndex = 21;
            this.lblRows.Text = "Total rows";
            // 
            // butGraph
            // 
            this.butGraph.Location = new System.Drawing.Point(974, 6);
            this.butGraph.Margin = new System.Windows.Forms.Padding(4);
            this.butGraph.Name = "butGraph";
            this.butGraph.Size = new System.Drawing.Size(105, 27);
            this.butGraph.TabIndex = 22;
            this.butGraph.Text = "Graph";
            this.butGraph.UseVisualStyleBackColor = true;
            this.butGraph.Visible = false;
            // 
            // butExcel
            // 
            this.butExcel.Location = new System.Drawing.Point(540, 5);
            this.butExcel.Margin = new System.Windows.Forms.Padding(4);
            this.butExcel.Name = "butExcel";
            this.butExcel.Size = new System.Drawing.Size(144, 28);
            this.butExcel.TabIndex = 20;
            this.butExcel.Text = "Export to Excel";
            this.butExcel.UseVisualStyleBackColor = true;
            this.butExcel.Click += new System.EventHandler(this.butExcel_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(216, 4);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 18;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Paste
            // 
            this.Paste.Location = new System.Drawing.Point(432, 4);
            this.Paste.Margin = new System.Windows.Forms.Padding(4);
            this.Paste.Name = "Paste";
            this.Paste.Size = new System.Drawing.Size(100, 28);
            this.Paste.TabIndex = 17;
            this.Paste.Text = "Paste All";
            this.Paste.UseVisualStyleBackColor = true;
            this.Paste.Click += new System.EventHandler(this.Paste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(324, 4);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 28);
            this.btnCopy.TabIndex = 16;
            this.btnCopy.Text = "Copy All";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExecuteAll
            // 
            this.btnExecuteAll.Location = new System.Drawing.Point(0, 4);
            this.btnExecuteAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecuteAll.Name = "btnExecuteAll";
            this.btnExecuteAll.Size = new System.Drawing.Size(100, 28);
            this.btnExecuteAll.TabIndex = 13;
            this.btnExecuteAll.Text = "Execute Al&l";
            this.btnExecuteAll.UseVisualStyleBackColor = true;
            this.btnExecuteAll.Click += new System.EventHandler(this.btnExecuteAll_Click);
            // 
            // butExecute
            // 
            this.butExecute.Location = new System.Drawing.Point(108, 4);
            this.butExecute.Margin = new System.Windows.Forms.Padding(4);
            this.butExecute.Name = "butExecute";
            this.butExecute.Size = new System.Drawing.Size(100, 28);
            this.butExecute.TabIndex = 14;
            this.butExecute.Text = "&Execute";
            this.butExecute.UseVisualStyleBackColor = true;
            this.butExecute.Click += new System.EventHandler(this.butExecute_Click);
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 840);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitCode.Panel1.ResumeLayout(false);
            this.splitCode.Panel2.ResumeLayout(false);
            this.splitCode.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCode)).EndInit();
            this.splitCode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
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
        private System.Windows.Forms.Button butGraph;
        public System.Windows.Forms.Button butExcel;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button Paste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnExecuteAll;
        private System.Windows.Forms.Button butExecute;
        private System.Windows.Forms.CheckBox checkShowMessages;
        private ScintillaNET.Scintilla txtQuery1;
    }
}

