using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedQueryOrganizer.ClosedXML
{
    /// <summary>
    /// https://github.com/ClosedXML/ClosedXML
    /// </summary>
    internal class ExportToExcel
    {
        public static string Export(DataTable table)
        {
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Sample Sheet");
        //        worksheet.Cell("A1").Value = "Hello World!";
        //        worksheet.Cell("A2").FormulaA1 = "=MID(A1, 7, 5)";
        //        workbook.SaveAs("HelloWorld.xlsx");
        //    }

            string excelfile = Path.GetTempPath() +
                        Guid.NewGuid().ToString() + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(table, "ExportedResults");
                wb.SaveAs(excelfile);
            }
            return excelfile;
        }


    }
}
