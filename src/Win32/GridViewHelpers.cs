using CoreLogic.PluginBase;
using CoreLogic.PluginBase.ClosedXML;
using System.Data;
using System.Windows.Forms;

namespace Win32Desktop
{
    public class GridViewHelpers
    {
        public static void Export(DataGridView dataGridView)
        {
            DataTable data = (DataTable)dataGridView.DataSource;
            if (data == null) return;

            //var exportColumns = new List<ExportColumn>();
            //foreach (DataGridViewColumn col in dataGridView.Columns)
            //{
            //    exportColumns.Add(new ExportColumn() { Width = col.Width });
            //}
            var fileName = ExportToExcel.Export(data);
            Common.OpenWithDefaultProgram(fileName);

            //string sFile = Application.StartupPath + @"\excel.xml";
            //s.WriteXml(sFile);
            //s.WriteXmlSchema(Application.StartupPath + @"\excel.xsd");
            //System.Diagnostics.Process.Start("excel.exe", "\"" + sFile + "\"");
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1025670/how-do-you-automatically-resize-columns-in-a-datagridview-control-and-allow-the
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void AutoSizeGridView(DataGridView dataGridView, int fillColumn = -1)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (fillColumn > -1)
                dataGridView.Columns[fillColumn].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView.Refresh();
            //// Now that DataGridView has calculated it's Widths; we can now store each column Width values.
            //for (int i = 0; i <= dataGridView.Columns.Count - 1; i++)
            //{
            //    // Store Auto Sized Widths:
            //    int colw = dataGridView.Columns[i].Width;

            //    // Remove AutoSizing:
            //    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            //    // Set Width to calculated AutoSize value:
            //    dataGridView.Columns[i].Width = colw;
            //}
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1706454/c-multiline-text-in-datagridview-control
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void SupportMultipleLineCells(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
        }
    }
}
