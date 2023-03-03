using CoreLogic.PluginBase.PluginBase;
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.PluginBase
{
    public delegate void dlgReaderOpen(object sender, object Reader);
    public enum SQLCommandType { Insert, Update, Delete }


    public interface IDynamicDAL : IDisposable
    {
        #region "DB Terms"
        string BaseTableType { get; set; }
        string[] executableType { get; set; }
        string[] codeTypeCodes { get; set; }
        string[] codeTypeDescriptions { get; set; }
        string[] codeTypeKeywordss { get; set; }

        string GetAllDBModulesCode { get; set; }
        string GetAllDBsCode { get; set; }
        string GetAllSchemaCode { get; set; }
        string GenerateCode(string Name, string codeType);
        string GetDropCode(string sObjectName, string objectType);
        string GetColumnsCode(string sTableName);
        string GetColumnListCode(string sTableName);
        string GetIdentityColumnCode(string sTableName);
        #endregion

        void SetValues(string sProcedure, bool bLogError, CommandType type);

        IDbConnection Connection { get; }
        IDbCommand Command { get; }
        IDataReader Reader { get; }

        event dlgReaderOpen ReaderOpen;

        string ErrorText { get; set; }
        StringBuilder SQLInfoMessageBuilder { get; set; }

        //void AddInputParameter(string field, SqlDbType type, int size, object paramvalue);
        //void AddInputParameter(string field, SqlDbType type, object paramvalue);
        //void AddOutputParameter(string field, SqlDbType type);
        //void AddOutputParameter(string field, SqlDbType type, int size);
        void DeriveParameters(ref IDbCommand obj);
        bool Execute();
        Task<bool> Execute(dlgReaderOpen function);
        Task<DataSet> Execute(string TableName);
        bool ExecuteScalar(out object ScalarData);
        object RunScalar(string SQL);
        string GenerateSQL(string SQL, string objectType, SQLCommandType sQLCommandType);
        string GetCSharpCodeForParameter(IDataParameter parameter, string sParameterFunction, string sValue);
        string GetParmeterSize(DbParameter dbparameter, ref string paramValue);
        string GetParamType(IDataParameter dbparameter);
        string GetInputParamValue(IDataParameter dbparameter);

        string GetTableRowsCode(string sTableName, int Rows, bool isReverse = false, string columnList = null);
        string GetModuleCode(string sModuleType, string sModuleName);
    }
}