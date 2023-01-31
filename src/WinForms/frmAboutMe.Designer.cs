namespace DBStudioLite
{
    partial class frmAboutMe
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
            this.rtbContents = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbContents
            // 
            this.rtbContents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbContents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbContents.Location = new System.Drawing.Point(0, 0);
            this.rtbContents.Name = "rtbContents";
            this.rtbContents.ReadOnly = true;
            this.rtbContents.Size = new System.Drawing.Size(843, 352);
            this.rtbContents.TabIndex = 0;
            this.rtbContents.Text = "";
            this.rtbContents.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbContents_LinkClicked);
            // 
            // frmAboutMe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 352);
            this.Controls.Add(this.rtbContents);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAboutMe";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About Me";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAboutMe_Load);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.RichTextBox rtbContents;

        #endregion

        //private System.Windows.Forms.WebBrowser webBrowser1;
        //private ExtendedBrowser webBrowser1;

    }
}