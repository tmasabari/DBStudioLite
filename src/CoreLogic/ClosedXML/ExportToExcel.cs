using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace CoreLogic.PluginBase.ClosedXML
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



    }

    public class ExportColumn
    {
        public int Width { get; set; }
    }
}
