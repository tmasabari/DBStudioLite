using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedQueryOrganizer
{
    static class XMLDAL
    {
        public static DataTable GetFirstTableFromXMLFile(string sFile)
        {
            DataSet Reports = new DataSet();
            Reports.ReadXml(sFile);
            return Reports.Tables[0];
        }

        public static void SaveToFile(string sFile, DataTable dataTable)
        {
            try
            {
                if (dataTable != null)
                {
                    dataTable.WriteXml(sFile);
                    if (sFile.LastIndexOf(".xml") > 0)
                        sFile = sFile.Replace(".xml", ".xsd");
                    else
                        sFile = sFile + ".xsd";
                    dataTable.WriteXmlSchema(sFile);
                    MessageBox.Show("The data saved to " + sFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error occurred " + Environment.NewLine + ex.ToString());
            }
        }
    }
}
