using System;
using System.Data;

namespace CoreLogic.PluginBase
{
    public static class DataAccessFactory
    {
        public static Func<string, string, IDynamicDAL> FuncGetDynamicDAL = null;

        public static IDynamicDAL GetDynamicDAL(string sConnection,
            string sProcedure, bool bLogError, CommandType type)
        {
            IDynamicDAL dynamicDAL = GetDynamicDAL(sConnection);
            dynamicDAL.SetValues(sProcedure, bLogError, type);
            return dynamicDAL;
        }

        public static IDynamicDAL GetDynamicDAL(string sConnection)
        {
            var sDBIdentifier = GetConnectionType(sConnection);
            return FuncGetDynamicDAL(sDBIdentifier, sConnection);
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
