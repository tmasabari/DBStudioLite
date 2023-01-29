using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.PluginBase.PluginBase
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
                //cannot access the disposed object so commented out
                //if (Connection.State == ConnectionState.Open) Connection.Close(); //Always close connection
                Connection.Dispose();
            }
        }
        protected abstract void ConnectionOpened();
        protected abstract IDataAdapter GetDataAdapter(IDbCommand dbCommand);

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
                            var da = GetDataAdapter(Command);
                            da.Fill(ds); //TableName
                            if (ds.Tables.Count > 0) ds.Tables[0].TableName = TableName;
                            da = null;
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
        public virtual string GetParamType(IDataParameter dbparameter)
        {
            return dbparameter.DbType.ToString("F");
        }


        public virtual string GetInputParamValue(IDataParameter dbparameter)
        {
            string paramValue;
            switch (dbparameter.DbType)
            {
                //case DbType.VarBinary:
                //case DbType.Binary:
                //case DbType.Xml:
                //    break;
                //case DbType.Udt:
                //    break;
                //case DbType.Structured:
                //case DbType.Image:
                //case DbType.UniqueIdentifier:

                case DbType.Date:
                case DbType.Time:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                //case DbType.SmallDateTime:
                case DbType.DateTime:
                    //case DbType.Timestamp:
                    paramValue = "'" + DateTime.Today.ToString("MM/dd/yyyy hh:mm") + "'";
                    break;

                case DbType.Boolean:
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Single:
                case DbType.Double:
                case DbType.Byte:
                case DbType.SByte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:

                    //case DbType.Float:
                    //case DbType.Bit:
                    //case DbType.TinyInt:
                    //case DbType.SmallInt:
                    //case DbType.Int:
                    //case DbType.BigInt:
                    //case DbType.Real:
                    //case DbType.SmallMoney:
                    //case DbType.Money:
                    paramValue = "0";
                    break;
                case DbType.AnsiString:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Object:
                    //case DbType.Char:
                    //case DbType.NChar:
                    //case DbType.NText:
                    //case DbType.NVarChar:
                    //case DbType.Text:
                    //case DbType.VarChar:
                    //case DbType.Variant:
                    paramValue = "''";
                    break;
                default:
                    paramValue = "NULL";
                    break;
            }

            return paramValue;
        }

        protected static string GetFirstColumn(string columnList)
        {
            string[] separator = { "," };
            return columnList.Split(separator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

    }
}
