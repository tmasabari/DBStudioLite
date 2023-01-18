using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

public interface IDynamicDAL: IDisposable
{
    IDbConnection Connection { get; }
    IDbCommand Command { get; }
    IDataReader Reader { get; }

    event DynamicDAL.dlgReaderOpen ReaderOpen;

    string ErrorText { get; set; }

    //void AddInputParameter(string field, SqlDbType type, int size, object paramvalue);
    //void AddInputParameter(string field, SqlDbType type, object paramvalue);
    //void AddOutputParameter(string field, SqlDbType type);
    //void AddOutputParameter(string field, SqlDbType type, int size);
    void DeriveParameters(ref IDbCommand obj);
    bool Execute();
    bool Execute(DynamicDAL.dlgReaderOpen function);
    Task<DataSet> Execute(string TableName);
    bool ExecuteScalar(out object ScalarData);
    object RunScalar(string SQL);
}