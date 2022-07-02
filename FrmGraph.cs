using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace AdvancedQuery
{
    public partial class FrmGraph : Form
    {
        public FrmGraph()
        {
            InitializeComponent();
            // Initialise graphics
            initialiseGraphics();
        }

#region " Variables "
        //   Variables declaration
        private Bitmap myImage;
        private Graphics g;
        private int[] p = { 1000000, 600000, 2500000, 80000 };
        private string[] towns = { "A", "B", "C", "D" };
        private Brush[] myBrushes = new Brush[5];
#endregion

#region " Methods "
        //   Initialises the bitmap, graphics context and pens for drawing
        private void initialiseGraphics()
        {
            try
            {
                // Create an in-memory bitmap where you will draw the image. 
                // The Bitmap is 300 pixels wide and 200 pixels high.
                myImage = new Bitmap(500, 300, PixelFormat.Format32bppRgb);

                // Get the graphics context for the bitmap.
                g = Graphics.FromImage(myImage);

                //   Create the brushes for drawing
                createBrushes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //   Method to create brushes of specific colours
        private void createBrushes()
        {
            try
            {
                myBrushes[0] = new SolidBrush(Color.Red);
                myBrushes[1] = new SolidBrush(Color.Blue);
                myBrushes[2] = new SolidBrush(Color.Yellow);
                myBrushes[3] = new SolidBrush(Color.Green);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //   Draws the barchart
        private void drawBarchart(Graphics g)
        {
            try
            {
                //   Variables declaration
                int i;
                int xInterval = 100;
                int width = 90;
                int height;
                SolidBrush blackBrush = new SolidBrush(Color.Black);

                for (i = 0; i <= p.Length - 1; i++)
                {
                    height = (p[i] / 10000);
                    //   divide by 10000 to adjust barchart to height of Bitmap

                    //   Draws the bar chart using specific colours
                    g.FillRectangle(myBrushes[i], xInterval * i + 50, 280 - height, width, height);

                    //   label the barcharts
                    g.DrawString(towns[i], new Font("Verdana", 12, FontStyle.Bold), Brushes.Black, xInterval * i + 50 + (width / 3), 280 - height - 25);

                    //   Draw the scale
                    g.DrawString(height.ToString(), new Font("Verdana", 8, FontStyle.Bold), Brushes.Black, 0, 280 - height);

                    //   Draw the axes
                    g.DrawLine(Pens.Brown, 40, 10, 40, 290);
                    //   y-axis
                    g.DrawLine(Pens.Brown, 20, 280, 490, 280);
                    //   x-axis
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //   Draws the piechart
        private void drawPieChart(Graphics g)
        {
            try
            {
                //   Variables declaration
                int i;
                int total=0;
                double percentage;
                double angleSoFar = 0.0;

                //   Caculates the total
                for (i = 0; i <= p.Length - 1; i++)
                {
                    total += p[i];
                }

                //   Draws the pie chart
                for (i = 0; i <= p.Length - 1; i++)
                {
                    percentage = (double)p[i] * 360 / total;

                    g.FillPie(myBrushes[i], 25, 25, 250, 250, (int)angleSoFar, (int)percentage);

                    angleSoFar += percentage;

                    //   Draws the lengend
                    g.FillRectangle(myBrushes[i], 350, 25 + (i * 50), 25, 25);

                    //   Label the towns
                    g.DrawString("Town " + towns[i], new Font("Verdana", 8, FontStyle.Bold), Brushes.Brown, 390, 25 + (i * 50) + 10);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion

#region "Events"
        //   Draws the barchart when this button is clicked
        protected void btnBarchart_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Set the background color and rendering quality.
                g.Clear(Color.WhiteSmoke);

                //   draws the barchart
                drawBarchart(g);

                // Render the image to the HTML output stream.
                //myImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //this.CreateGraphics().DrawImage(myImage,10,10);
                pictureBox1.Image = myImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPiechart_Click(object sender, System.EventArgs e)
        {
            try
            {
                // Set the background color and rendering quality.
                g.Clear(Color.WhiteSmoke);

                //   draws the barchart
                drawPieChart(g);

                // Render the image to the HTML output stream.
               // myImage.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                pictureBox1.Image = myImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion
    }

}
