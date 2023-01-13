using DBStudioLite;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Wrapper to do all database related operations
/// </summary>
//The using statement applies only to objects that implement the System.IDisposable interface.
//The using statement ensures that Dispose is called even if an exception occurs while you are calling methods on the object. 
//You can achieve the same result by putting the object inside a try block and then calling Dispose in a finally block;

public class DynamicDAL : IDisposable
{
    #region Database
    public SqlCommand command;
    public SqlDataReader reader;

    public string ErrorText;
    public bool LogError;
    public StringBuilder SQLInfoMessageBuilder = new StringBuilder();
    public delegate void dlgReaderOpen(object sender, object Reader);
    public event dlgReaderOpen ReaderOpen;

    public SqlConnection connection;
    private string sProcedureName;
    private string sConnectionString;

    public void Dispose()
    {
        if (command != null) command.Dispose();
        if (connection != null)
        {
            if (connection.State == ConnectionState.Open) connection.Close();        //Always close connection
            connection.Dispose();
        }
    }
    ~DynamicDAL() { Dispose(); }

    // <param name="paramvalue">If paramvalue not required Pass null. pass DBNull.Value to pass database null</param>
    public void AddInputParameter(string field, SqlDbType type, int size, object paramvalue)
    {
        //length must be within database limit //database procedure will automatically ignore extra chars but for safety
        if (paramvalue.GetType() == typeof(string))  //if (paramvalue is string)
        {
            string sTemp = (string)paramvalue;
            if (size < sTemp.Length) paramvalue = sTemp.Substring(0, size);
        }
        command.Parameters.Add(field, type, size);
        command.Parameters[field].Value = paramvalue;
    }
    public void AddInputParameter(string field, SqlDbType type, object paramvalue)
    {
        command.Parameters.Add(field, type);
        command.Parameters[field].Value = paramvalue;
    }

    public void AddOutputParameter(string field, SqlDbType type, int size)
    {
        command.Parameters.Add(field, type, size);
        command.Parameters[field].Direction = ParameterDirection.Output;
    }
    public void AddOutputParameter(string field, SqlDbType type)
    {
        command.Parameters.Add(field, type);
        command.Parameters[field].Direction = ParameterDirection.Output;
    }

    public bool Execute()
    {   //SqlTransaction transaction ==null;
        bool bReturn = false;
        try
        {
            connection.Open();
            connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                command.Transaction = transaction;                                  // Assign Transaction to Command
                command.ExecuteNonQuery();
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

    //changed
    public DynamicDAL(string sConnection, string sProcedure, bool bLogError)
        : this(sConnection, sProcedure, bLogError, CommandType.StoredProcedure)
    {
    }

    //new
    public DynamicDAL(string sConnection, string sProcedure, bool bLogError, CommandType type)
    {
        sConnectionString = sConnection;
        sProcedureName = sProcedure;

        connection = new SqlConnection(sConnectionString); //Raises error in case of invalid connection string

        command = connection.CreateCommand();
        command.CommandText = sProcedureName;
        command.CommandType = type;
        command.StatementCompleted += SelectCommand_StatementCompleted;

        ErrorText = "";
        LogError = bLogError;
    }
    void connection_InfoMessage(object sender, SqlInfoMessageEventArgs args)
    {
        foreach (SqlError sqlError in args.Errors)
        {
            var error = String.Format("Msg {0}, Number {1}, Class {2}, State {3}, Line {4}", sqlError.Message, sqlError.Number, sqlError.Class, sqlError.State, sqlError.LineNumber);
            SQLInfoMessageBuilder.AppendLine(error);
        }

    }
    void SelectCommand_StatementCompleted(object sender, StatementCompletedEventArgs args)
    {
        var rowsMessage = String.Format("({0} row(s) affected)", args.RecordCount);
        SQLInfoMessageBuilder.AppendLine(rowsMessage);
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
                    connection.Open();
                    connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        // Execute the command.                rowsAffected = cmd.ExecuteNonQuery();
                        command.Transaction = transaction;     // Assign Transaction to Command

                        // create and fill the DataSet
                        using (SqlDataAdapter da = new SqlDataAdapter(command))
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
            connection.Open();
            connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                // Execute the command.                rowsAffected = cmd.ExecuteNonQuery();
                command.Transaction = transaction;                                  // Assign Transaction to Command
                using (reader = command.ExecuteReader())
                {
                    ReaderOpen += function;
                    //Invoking all the event handlers
                    if (ReaderOpen != null) ReaderOpen(this, reader);
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
            if (connection.State != ConnectionState.Open) connection.Open();
            connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            ScalarData = command.ExecuteScalar();
            bReturn = true;
        }
        catch (Exception ex)
        {
            if (this.LogError) ErrorText = ex.Message;
        }
        if (command != null) command.Dispose();
        if (connection != null)
        {
            if (connection.State == ConnectionState.Open) connection.Close();        //Always close connection
            connection.Dispose();
        }
        return bReturn;
    }

    public object RunScalar(string SQL)
    {
        object ScalarData = null;
        try
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            connection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
            using (SqlCommand command = connection.CreateCommand())
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
        if (connection != null)
        {
            if (connection.State == ConnectionState.Open) connection.Close();        //Always close connection
            connection.Dispose();
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

