using System.Data;

namespace CoreLogic.PluginBase
{
    public static class DataAccessFactory
    {
        public static IDynamicDAL GetDynamicDAL(string sConnection,
            string sProcedure, bool bLogError, CommandType type)
        {
            IDynamicDAL dynamicDAL = GetDynamicDAL(sConnection);
            dynamicDAL.SetValues(sProcedure, bLogError, type);
            return dynamicDAL;
        }

        public static IDynamicDAL GetDynamicDAL(string sConnection)
        {
            switch (GetConnectionType(sConnection))
            {
                case "SQLite":
                    return new DynamicDALSQLite(sConnection);
                default:
                    return new DynamicDALSqlServer(sConnection);
            }
        }

        public static string GetConnectionType(string sConnection)
        {
            if (sConnection.ToLower().Contains("version"))
                return "SQLite";
            else
                return "SqlServer";
        }
    }
}
