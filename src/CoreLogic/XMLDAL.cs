using System.Data;

namespace DBStudioLite
{
    public static class XMLDAL
    {
        public static DataTable GetFirstTableFromXMLFile(string sFile)
        {
            DataSet Reports = new DataSet();
            Reports.ReadXml(sFile);
            return Reports.Tables[0];
        }

        public static void SaveToFile(string sFile, DataTable dataTable)
        {
            if (dataTable != null)
            {
                dataTable.WriteXml(sFile);
                if (sFile.LastIndexOf(".xml") > 0)
                    sFile = sFile.Replace(".xml", ".xsd");
                else
                    sFile = sFile + ".xsd";
                dataTable.WriteXmlSchema(sFile);
            }
        }
    }
}
