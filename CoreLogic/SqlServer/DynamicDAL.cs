using Microsoft.Data.SqlClient;
using CoreLogic.SqlServer;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Linq;
using System.Reflection;

/// <summary>
/// Wrapper to do all database related operations
/// </summary>
//The using statement applies only to objects that implement the System.IDisposable interface.
//The using statement ensures that Dispose is called even if an exception occurs while you are calling methods on the object. 
//You can achieve the same result by putting the object inside a try block and then calling Dispose in a finally block;

public class DynamicDAL : IDynamicDAL
{
    #region Database
    public IDbConnection Connection { get; }
    public IDbCommand Command { get; }
    public IDataReader Reader { get; set; }

    public string ErrorText { get; set; }
    public bool LogError;
    public StringBuilder SQLInfoMessageBuilder = new StringBuilder();

    public delegate void dlgReaderOpen(object sender, object Reader);
    public event dlgReaderOpen ReaderOpen;

    private string sProcedureName;
    private string sConnectionString;

    public void Dispose()
    {
        if (Command != null) Command.Dispose();
        if (Connection != null)
        {
            if (Connection.State == ConnectionState.Open) Connection.Close();        //Always close connection
            Connection.Dispose();
        }
    }
    ~DynamicDAL() { Dispose(); }

    //new
    public DynamicDAL(string sConnection, string sProcedure, bool bLogError, CommandType type)
    {
        sConnectionString = sConnection;
        sProcedureName = sProcedure;

        Connection = new SqlConnection(sConnectionString); //Raises error in case of invalid connection string

        Command = Connection.CreateCommand();
        Command.CommandText = sProcedureName;
        Command.CommandType = type;
        ((SqlCommand)Command).StatementCompleted += SelectCommand_StatementCompleted;

        ErrorText = "";
        LogError = bLogError;
    }

    public void DeriveParameters(ref IDbCommand obj)
    {
        SqlCommandBuilder.DeriveParameters((SqlCommand)obj);
    }

    //// <param name="paramvalue">If paramvalue not required Pass null. pass DBNull.Value to pass database null</param>
    //public void AddInputParameter(string field, SqlDbType type, int size, object paramvalue)
    //{
    //    //length must be within database limit //database procedure will automatically ignore extra chars but for safety
    //    if (paramvalue.GetType() == typeof(string))  //if (paramvalue is string)
    //    {
    //        string sTemp = (string)paramvalue;
    //        if (size < sTemp.Length) paramvalue = sTemp.Substring(0, size);
    //    }
    //    Command.Parameters.Add(field, type, size);
    //    Command.Parameters[field].Value = paramvalue;
    //}
    //public void AddInputParameter(string field, SqlDbType type, object paramvalue)
    //{
    //    Command.Parameters.Add(field, type);
    //    Command.Parameters[field].Value = paramvalue;
    //}

    //public void AddOutputParameter(string field, SqlDbType type, int size)
    //{
    //    Command.Parameters.Add(field, type, size);
    //    Command.Parameters[field].Direction = ParameterDirection.Output;
    //}
    //public void AddOutputParameter(string field, SqlDbType type)
    //{
    //    Command.Parameters.Add(field, type);
    //    Command.Parameters[field].Direction = ParameterDirection.Output;
    //}

    public bool Execute()
    {   //SqlTransaction transaction ==null;
        bool bReturn = false;
        try
        {
            Connection.Open();
            var sqlconnection = (SqlConnection)Connection;
            sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (var transaction = Connection.BeginTransaction())
            {
                Command.Transaction = transaction;                                  // Assign Transaction to Command
                Command.ExecuteNonQuery();
                transaction.Commit();
                bReturn = true;
            }
        }
        catch (Exception ex)
        {
            if (this.LogError)
            {
                if (this.LogError) ErrorText = ex.Message;
            }
        }
        return bReturn;
    }

    void connection_InfoMessage(object sender, SqlInfoMessageEventArgs args)
    {
        foreach (SqlError sqlError in args.Errors)
        {
            //'{sqlError.Server}'
            var error = $"@{DateTime.Now.ToString("hh:mm:ss FFF")}, @Line {sqlError.LineNumber} '{sqlError.Message}'";
            if (sqlError.Number > 0) error += $", Error {sqlError.Number}, Class {sqlError.Class}, State {sqlError.State}";
            SQLInfoMessageBuilder.AppendLine(error);
        }
    }
    void SelectCommand_StatementCompleted(object sender, StatementCompletedEventArgs args)
    {
        var rowsMessage = String.Format("({0} row(s) affected)", args.RecordCount);
        SQLInfoMessageBuilder.AppendLine(rowsMessage);
    }
    //https://stackoverflow.com/questions/10723558/instantiate-idataadapter-from-instance-of-idbconnection
    IDataAdapter GetAdapter(IDbConnection connection)
    {
        var assembly = connection.GetType().Assembly;
        var @namespace = connection.GetType().Namespace;

        // Assumes the factory is in the same namespace
        var factoryType = assembly.GetTypes()
                            .Where(x => x.Namespace == @namespace)
                            .Where(x => x.IsSubclassOf(typeof(DbProviderFactory)))
                            .Single();

        // SqlClientFactory and OleDbFactory both have an Instance field.
        var instanceFieldInfo = factoryType.GetField("Instance", BindingFlags.Static | BindingFlags.Public);
        var factory = (DbProviderFactory)instanceFieldInfo.GetValue(null);

        return factory.CreateDataAdapter();
    }
    public Task<DataSet> Execute(string TableName)
    {
        try
        {
            return Task.Run(() =>
            {
                try
                {
                    var ds = new DataSet();
                    Connection.Open(); var sqlconnection = (SqlConnection)Connection;
                    sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
                    using (var transaction = Connection.BeginTransaction())
                    {
                        // Execute the command.                rowsAffected = cmd.ExecuteNonQuery();
                        Command.Transaction = transaction;     // Assign Transaction to Command

                        // create and fill the DataSet
                        using (var da = new SqlDataAdapter((SqlCommand) Command))
                        {
                            da.Fill(ds, TableName);
                        }

                        transaction.Commit();
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    if (this.LogError)
                    {
                        ErrorText = ex.Message; // (new ErrorLog()).LogError(ex, sConnectionString); 
                    }
                }
                return null;
            });
        }
        catch (Exception ex)
        {
            if (this.LogError)
            {
                ErrorText = ex.Message; // (new ErrorLog()).LogError(ex, sConnectionString); 
            }
        }
        return null;
    }

    public bool Execute(dlgReaderOpen function)
    {
        bool bReturn = false;
        try
        {
            Connection.Open(); var sqlconnection = (SqlConnection)Connection;
            sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (var transaction = Connection.BeginTransaction())
            {
                // Execute the command.                rowsAffected = cmd.ExecuteNonQuery();
                Command.Transaction = transaction;                                  // Assign Transaction to Command
                using (Reader = Command.ExecuteReader())
                {
                    ReaderOpen += function;
                    //Invoking all the event handlers
                    if (ReaderOpen != null) ReaderOpen(this, Reader);
                    ReaderOpen -= function;
                }

                transaction.Commit();
                bReturn = true;
            }
        }
        catch (Exception ex)
        {
            if (this.LogError)
            {
                ErrorText = ex.Message; // (new ErrorLog()).LogError(ex, sConnectionString); 
            }
        }
        return bReturn;
    }
    public bool ExecuteScalar(out object ScalarData)
    {
        bool bReturn = false;
        ScalarData = null;
        try
        {
            if (Connection.State != ConnectionState.Open) Connection.Open(); 
            var sqlconnection = (SqlConnection)Connection;
            sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            ScalarData = Command.ExecuteScalar();
            bReturn = true;
        }
        catch (Exception ex)
        {
            if (this.LogError) ErrorText = ex.Message;
        }
        if (Command != null) Command.Dispose();
        if (Connection != null)
        {
            if (Connection.State == ConnectionState.Open) Connection.Close();        //Always close connection
            Connection.Dispose();
        }
        return bReturn;
    }

    public object RunScalar(string SQL)
    {
        object ScalarData = null;
        try
        {
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var sqlconnection = (SqlConnection)Connection;
            sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = SQL;
                command.CommandType = CommandType.Text;
                ScalarData = command.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            if (this.LogError) ErrorText = ex.Message;
        }
        if (Connection != null)
        {
            if (Connection.State == ConnectionState.Open) Connection.Close();        //Always close connection
            Connection.Dispose();
        }
        return ScalarData;
    }
    #endregion
    #region Table properties
    public static string GetColumnList(string sConnectionString, string sTableName)
    {
        string SQL = DynamicDataSourceCode.GetColumnListCode(sTableName);
        string lsConnection = sConnectionString;
        using (DynamicDAL DataObj = new DynamicDAL(lsConnection, SQL, true, CommandType.Text))
        {
            object objreturn;
            if (DataObj.ExecuteScalar(out objreturn))
            {
                if (!(objreturn is System.DBNull))
                {
                    return objreturn.ToString();
                }
            }
            return String.Empty;
        }
    }

    public static string GetIdentityColumn(string sConnectionString, string sTableName)
    {
        string SQL = DynamicDataSourceCode.GetIdentityColumnCode(sTableName);
        string lsConnection = sConnectionString;
        using (DynamicDAL DataObj = new DynamicDAL(lsConnection, SQL, true, CommandType.Text))
        {
            object objreturn;
            if (DataObj.ExecuteScalar(out objreturn))
            {
                if (objreturn != null && !(objreturn is System.DBNull))
                {
                    return objreturn.ToString();
                }
            }
            return String.Empty;
        }
    }
    #endregion
}

