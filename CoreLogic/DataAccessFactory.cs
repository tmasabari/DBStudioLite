using System.Data;

namespace CoreLogic
{
    public static class DataAccessFactory
    {
        public static IDynamicDAL GetDynamicDAL(string sConnection,
            string sProcedure, bool bLogError, CommandType type)
        {
            return new DynamicDALSqlServer(sConnection, sProcedure, bLogError, type);
        }

        public static IDynamicDAL GetDynamicDAL(string sConnection)
        {
            return new DynamicDALSqlServer(sConnection);
        }
    }
}
