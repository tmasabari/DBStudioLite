namespace DBStudioLite
{
    partial class FrmOptions
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
            this.chkLoadPreviousSession = new System.Windows.Forms.CheckBox();
            this.lblPoolSize = new System.Windows.Forms.Label();
            this.txtPool = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPockets = new System.Windows.Forms.TextBox();
            this.lblConnectionTime = new System.Windows.Forms.Label();
            this.txtConnectionTimeout = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkLoadPreviousSession
            // 
            this.chkLoadPreviousSession.AutoSize = true;
            this.chkLoadPreviousSession.Checked = true;
            this.chkLoadPreviousSession.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoadPreviousSession.Location = new System.Drawing.Point(32, 163);
            this.chkLoadPreviousSession.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLoadPreviousSession.Name = "chkLoadPreviousSession";
            this.chkLoadPreviousSession.Size = new System.Drawing.Size(359, 24);
            this.chkLoadPreviousSession.TabIndex = 21;
            this.chkLoadPreviousSession.Text = "During startup load files from previous session";
            this.chkLoadPreviousSession.UseVisualStyleBackColor = true;
            // 
            // lblPoolSize
            // 
            this.lblPoolSize.AutoSize = true;
            this.lblPoolSize.Location = new System.Drawing.Point(10, 69);
            this.lblPoolSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPoolSize.Name = "lblPoolSize";
            this.lblPoolSize.Size = new System.Drawing.Size(108, 20);
            this.lblPoolSize.TabIndex = 17;
            this.lblPoolSize.Text = "Max Pool Size";
            this.lblPoolSize.Click += new System.EventHandler(this.lblPoolSize_Click);
            // 
            // txtPool
            // 
            this.txtPool.Location = new System.Drawing.Point(305, 16);
            this.txtPool.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPool.MaxLength = 3;
            this.txtPool.Name = "txtPool";
            this.txtPool.Size = new System.Drawing.Size(61, 26);
            this.txtPool.TabIndex = 18;
            this.txtPool.Text = "20";
            this.txtPool.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "Packet Size";
            // 
            // txtPockets
            // 
            this.txtPockets.Location = new System.Drawing.Point(305, 99);
            this.txtPockets.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPockets.MaxLength = 5;
            this.txtPockets.Name = "txtPockets";
            this.txtPockets.Size = new System.Drawing.Size(61, 26);
            this.txtPockets.TabIndex = 20;
            this.txtPockets.Text = "4096";
            this.txtPockets.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblConnectionTime
            // 
            this.lblConnectionTime.AutoSize = true;
            this.lblConnectionTime.Location = new System.Drawing.Point(10, 28);
            this.lblConnectionTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnectionTime.Name = "lblConnectionTime";
            this.lblConnectionTime.Size = new System.Drawing.Size(151, 20);
            this.lblConnectionTime.TabIndex = 15;
            this.lblConnectionTime.Text = "Connection Timeout";
            // 
            // txtConnectionTimeout
            // 
            this.txtConnectionTimeout.Location = new System.Drawing.Point(305, 60);
            this.txtConnectionTimeout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnectionTimeout.MaxLength = 3;
            this.txtConnectionTimeout.Name = "txtConnectionTimeout";
            this.txtConnectionTimeout.Size = new System.Drawing.Size(61, 26);
            this.txtConnectionTimeout.TabIndex = 16;
            this.txtConnectionTimeout.Text = "20";
            this.txtConnectionTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPoolSize);
            this.groupBox1.Controls.Add(this.txtConnectionTimeout);
            this.groupBox1.Controls.Add(this.txtPockets);
            this.groupBox1.Controls.Add(this.txtPool);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblConnectionTime);
            this.groupBox1.Location = new System.Drawing.Point(18, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 137);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection pool options";
            // 
            // FrmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 200);
            this.Controls.Add(this.chkLoadPreviousSession);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOptions";
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOptions_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblPoolSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblConnectionTime;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.CheckBox chkLoadPreviousSession;
        internal System.Windows.Forms.TextBox txtPool;
        internal System.Windows.Forms.TextBox txtPockets;
        internal System.Windows.Forms.TextBox txtConnectionTimeout;
    }
}