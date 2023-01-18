using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    public delegate void dlgReaderOpen(object sender, object Reader);

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
        bool Execute(dlgReaderOpen function);
        Task<DataSet> Execute(string TableName);
        bool ExecuteScalar(out object ScalarData);
        object RunScalar(string SQL);

        string GetCSharpCodeForParameter(IDataParameter parameter, string sParameterFunction, string sValue);
        string GetParmeterSize(IDataParameter dbparameter, ref string paramValue);
        string GetParamType(IDataParameter dbparameter);
        string GetInputParamValue(IDataParameter dbparameter);

        string GetTableRowsCode(string sTableName, int Rows, bool isReverse = false, string columnList = null);
    }
}