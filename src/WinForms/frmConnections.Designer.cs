﻿namespace DBStudioLite
{
    partial class frmConnections
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
            this.splitConnectionString = new System.Windows.Forms.SplitContainer();
            this.splitButtons = new System.Windows.Forms.SplitContainer();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.ConnectionsList = new System.Windows.Forms.ListBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.lblConnectionCaption = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitConnectionString)).BeginInit();
            this.splitConnectionString.Panel1.SuspendLayout();
            this.splitConnectionString.Panel2.SuspendLayout();
            this.splitConnectionString.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitButtons)).BeginInit();
            this.splitButtons.Panel1.SuspendLayout();
            this.splitButtons.Panel2.SuspendLayout();
            this.splitButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitConnectionString
            // 
            this.splitConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitConnectionString.Location = new System.Drawing.Point(0, 0);
            this.splitConnectionString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitConnectionString.Name = "splitConnectionString";
            this.splitConnectionString.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitConnectionString.Panel1
            // 
            this.splitConnectionString.Panel1.Controls.Add(this.splitButtons);
            // 
            // splitConnectionString.Panel2
            // 
            this.splitConnectionString.Panel2.Controls.Add(this.splitContainer3);
            this.splitConnectionString.Size = new System.Drawing.Size(1009, 594);
            this.splitConnectionString.SplitterDistance = 376;
            this.splitConnectionString.SplitterWidth = 6;
            this.splitConnectionString.TabIndex = 0;
            // 
            // splitButtons
            // 
            this.splitButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitButtons.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitButtons.IsSplitterFixed = true;
            this.splitButtons.Location = new System.Drawing.Point(0, 0);
            this.splitButtons.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitButtons.Name = "splitButtons";
            // 
            // splitButtons.Panel1
            // 
            this.splitButtons.Panel1.Controls.Add(this.btnSave);
            this.splitButtons.Panel1.Controls.Add(this.btnRemove);
            this.splitButtons.Panel1.Controls.Add(this.btnAdd);
            // 
            // splitButtons.Panel2
            // 
            this.splitButtons.Panel2.Controls.Add(this.ConnectionsList);
            this.splitButtons.Size = new System.Drawing.Size(1009, 376);
            this.splitButtons.SplitterDistance = 134;
            this.splitButtons.SplitterWidth = 6;
            this.splitButtons.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(13, 96);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 35);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(13, 51);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(112, 35);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(13, 6);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ConnectionsList
            // 
            this.ConnectionsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectionsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionsList.FormattingEnabled = true;
            this.ConnectionsList.ItemHeight = 29;
            this.ConnectionsList.Location = new System.Drawing.Point(0, 0);
            this.ConnectionsList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ConnectionsList.Name = "ConnectionsList";
            this.ConnectionsList.Size = new System.Drawing.Size(869, 376);
            this.ConnectionsList.TabIndex = 1;
            this.ConnectionsList.SelectedIndexChanged += new System.EventHandler(this.ConnectionsList_SelectedIndexChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.lblConnectionCaption);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.txtConnectionString);
            this.splitContainer3.Size = new System.Drawing.Size(1009, 212);
            this.splitContainer3.SplitterDistance = 32;
            this.splitContainer3.SplitterWidth = 6;
            this.splitContainer3.TabIndex = 1;
            // 
            // lblConnectionCaption
            // 
            this.lblConnectionCaption.AutoSize = true;
            this.lblConnectionCaption.Location = new System.Drawing.Point(-4, 13);
            this.lblConnectionCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionCaption.Name = "lblConnectionCaption";
            this.lblConnectionCaption.Size = new System.Drawing.Size(247, 20);
            this.lblConnectionCaption.TabIndex = 0;
            this.lblConnectionCaption.Text = "Enter your connection string here ";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.AcceptsReturn = true;
            this.txtConnectionString.AcceptsTab = true;
            this.txtConnectionString.AllowDrop = true;
            this.txtConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnectionString.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.HideSelection = false;
            this.txtConnectionString.Location = new System.Drawing.Point(0, 0);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(1009, 174);
            this.txtConnectionString.TabIndex = 0;
            // 
            // frmConnections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 594);
            this.Controls.Add(this.splitConnectionString);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnections";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connections";
            this.Activated += new System.EventHandler(this.frmConnections_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConnections_FormClosing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmConnections_KeyUp);
            this.splitConnectionString.Panel1.ResumeLayout(false);
            this.splitConnectionString.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConnectionString)).EndInit();
            this.splitConnectionString.ResumeLayout(false);
            this.splitButtons.Panel1.ResumeLayout(false);
            this.splitButtons.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitButtons)).EndInit();
            this.splitButtons.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitConnectionString;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.SplitContainer splitButtons;
        private System.Windows.Forms.ListBox ConnectionsList;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Label lblConnectionCaption;
    }
}