namespace DBStudioLite
{
    partial class FrmGraph
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
            this.btnBarchart = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPiechart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBarchart
            // 
            this.btnBarchart.Location = new System.Drawing.Point(12, 4);
            this.btnBarchart.Margin = new System.Windows.Forms.Padding(4);
            this.btnBarchart.Name = "btnBarchart";
            this.btnBarchart.Size = new System.Drawing.Size(100, 28);
            this.btnBarchart.TabIndex = 0;
            this.btnBarchart.Text = "Bar";
            this.btnBarchart.UseVisualStyleBackColor = true;
            this.btnBarchart.Click += new System.EventHandler(this.btnBarchart_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 39);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(667, 369);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // btnPiechart
            // 
            this.btnPiechart.Location = new System.Drawing.Point(120, 4);
            this.btnPiechart.Margin = new System.Windows.Forms.Padding(4);
            this.btnPiechart.Name = "btnPiechart";
            this.btnPiechart.Size = new System.Drawing.Size(100, 28);
            this.btnPiechart.TabIndex = 2;
            this.btnPiechart.Text = "Pie";
            this.btnPiechart.UseVisualStyleBackColor = true;
            this.btnPiechart.Click += new System.EventHandler(this.btnPiechart_Click);
            // 
            // FrmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 411);
            this.Controls.Add(this.btnPiechart);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnBarchart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmGraph";
            this.Text = "FrmGraph";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBarchart;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnPiechart;
    }
}