namespace DBStudioLite
{
    partial class FrmAllDbs
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
            this.butExecute = new System.Windows.Forms.Button();
            this.butRestart = new System.Windows.Forms.Button();
            this.butSkip = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butExecute
            // 
            this.butExecute.Location = new System.Drawing.Point(3, 48);
            this.butExecute.Name = "butExecute";
            this.butExecute.Size = new System.Drawing.Size(348, 39);
            this.butExecute.TabIndex = 1;
            this.butExecute.Text = "Execute@ ";
            this.butExecute.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butExecute.UseVisualStyleBackColor = true;
            this.butExecute.Click += new System.EventHandler(this.butExecute_Click);
            // 
            // butRestart
            // 
            this.butRestart.Location = new System.Drawing.Point(3, 3);
            this.butRestart.Name = "butRestart";
            this.butRestart.Size = new System.Drawing.Size(348, 39);
            this.butRestart.TabIndex = 2;
            this.butRestart.Text = "Begin/Restart@ ";
            this.butRestart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butRestart.UseVisualStyleBackColor = true;
            this.butRestart.Click += new System.EventHandler(this.butRestart_Click);
            // 
            // butSkip
            // 
            this.butSkip.Location = new System.Drawing.Point(3, 93);
            this.butSkip.Name = "butSkip";
            this.butSkip.Size = new System.Drawing.Size(348, 39);
            this.butSkip.TabIndex = 3;
            this.butSkip.Text = "&Skip ";
            this.butSkip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butSkip.UseVisualStyleBackColor = true;
            this.butSkip.Click += new System.EventHandler(this.butSkip_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(3, 138);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(108, 39);
            this.butCancel.TabIndex = 4;
            this.butCancel.Text = "&Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.butRestart);
            this.flowLayoutPanel1.Controls.Add(this.butExecute);
            this.flowLayoutPanel1.Controls.Add(this.butSkip);
            this.flowLayoutPanel1.Controls.Add(this.butCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(368, 180);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // FrmAllDbs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butCancel;
            this.ClientSize = new System.Drawing.Size(368, 202);
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmAllDbs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Excuting in All DBs...";
            this.Activated += new System.EventHandler(this.FrmAllDbs_Activated);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butExecute;
        private System.Windows.Forms.Button butRestart;
        private System.Windows.Forms.Button butSkip;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}