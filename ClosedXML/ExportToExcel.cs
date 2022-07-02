using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace DBStudioLite.ClosedXML
{
    /// <summary>
    /// https://github.com/ClosedXML/ClosedXML
    /// https://github.com/closedxml/closedxml/wiki
    /// </summary>
    public class ExportToExcel
    {
        public static string Export(DataTable table, List<ExportColumn> exportColumns = null)
        {
            string excelfile = Path.GetTempPath() +
                        Guid.NewGuid().ToString() + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet = wb.Worksheets.Add(table, "ExportedResults");
                sheet.Columns().AdjustToContents();  // Adjust column width
                sheet.Rows().AdjustToContents();     // Adjust row heights

                //if (exportColumns != null)
                //{
                //    int i = 1; //1 based index
                //    foreach (var column in exportColumns)
                //    {
                //        sheet.Column(i).Width = column.Width;
                //        i++;
                //    }
                //}
                wb.SaveAs(excelfile);
            }
            return excelfile;
        }
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Sample Sheet");
        //        worksheet.Cell("A1").Value = "Hello World!";
        //        worksheet.Cell("A2").FormulaA1 = "=MID(A1, 7, 5)";
        //        workbook.SaveAs("HelloWorld.xlsx");
        //    }

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

    }

    public class ExportColumn
    {
        public int Width { get; set; }
    }
}
