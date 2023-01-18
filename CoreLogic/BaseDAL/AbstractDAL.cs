using DocumentFormat.OpenXml.Office2013.Excel;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.BaseDAL
{
    public abstract class AbstractDAL
    {
        public IDbConnection Connection { get; protected set; }
        public IDbCommand Command { get; protected set; }
        public IDataReader Reader { get; set; }

        public string ErrorText { get; set; }
        public bool LogError;
        public StringBuilder SQLInfoMessageBuilder { get; set; }

        public event dlgReaderOpen ReaderOpen;

        protected string sProcedureName;
        protected string sConnectionString;

        public void Dispose()
        {
            if (Command != null) Command.Dispose();
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();        //Always close connection
                Connection.Dispose();
            }
        }
        protected abstract void ConnectionOpened();
        protected abstract IDataAdapter DataAdapter(IDbCommand dbCommand);

        public object RunScalar(string SQL)
        {
            object ScalarData = null;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    ConnectionOpened();
                }
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
        public bool ExecuteScalar(out object ScalarData)
        {
            bool bReturn = false;
            ScalarData = null;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    ConnectionOpened();
                }
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

        public bool Execute(dlgReaderOpen function)
        {
            bool bReturn = false;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    ConnectionOpened();
                }
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

        public bool Execute()
        {   //SqlTransaction transaction ==null;
            bool bReturn = false;
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    ConnectionOpened();
                }
                using (var transaction = Connection.BeginTransaction())
                {
                    Command.Transaction = transaction; // Assign Transaction to Command
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

        //to do remove tablename
        public Task<DataSet> Execute(string TableName)
        {
            try
            {
                return Task.Run(() =>
                {
                    try
                    {
                        if (Connection.State != ConnectionState.Open)
                        {
                            Connection.Open();
                            ConnectionOpened();
                        }
                        using (var transaction = Connection.BeginTransaction())
                        {
                            // Execute the command.                rowsAffected = cmd.ExecuteNonQuery();
                            Command.Transaction = transaction;     // Assign Transaction to Command
                            var ds = new DataSet();
                            // create and fill the DataSet
                            var da = DataAdapter(Command);
                            da.Fill(ds); //TableName
                            if(ds.Tables.Count > 0) ds.Tables[0].TableName = TableName;
                            da=null;
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





    }
}
