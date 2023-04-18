using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

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
        protected abstract DbDataAdapter GetDataAdapter(IDbCommand dbCommand);
        protected abstract DbCommandBuilder GetCommandBuilder();

        public virtual void DeriveParameters(ref IDbCommand obj)
        {        //unsupported
                 // SQLiteCommandBuilder does not implement DeriveParameters.
                 // This is not surprising, since SQLite has no support for stored procs.
                 //SQLiteCommandBuilder.DeriveParameters((SQLiteCommand)obj);

            throw new NotImplementedException("This feature is not available for the databse.");
        }
        public virtual string GetParmeterSize(DbParameter parameter, ref string paramValue)
        {
            string sSize = string.Empty;
            if (parameter.DbType == DbType.AnsiString || parameter.DbType == DbType.String
                || parameter.DbType == DbType.VarNumeric || parameter.DbType == DbType.Decimal)
            {
                if (parameter.Size > 0)
                {
                    sSize = "(" + parameter.Size.ToString() + ") ";
                    paramValue = "''";
                }
                //The scale must be less than or equal to the precision.
                else if (parameter.Precision > 0)
                {
                    sSize = "(" + parameter.Precision.ToString();
                    if (parameter.Scale > 0) sSize += "," + parameter.Scale.ToString();
                    sSize += ") ";
                    paramValue = "0";
                }
                else
                {
                    sSize = "(1000) "; //consider 1000 length
                    paramValue = "''";
                }
            }
            return sSize;
        }
        public void SetValues(string sProcedure, bool bLogError, CommandType type)
        {
            sProcedureName = sProcedure;
            Command.CommandText = sProcedureName;
            Command.CommandType = type;
            LogError = bLogError;
        }

        public async Task<Tuple<object, bool>> ExecuteScalar(string SQL)
        {
            SetValues(SQL, true, CommandType.Text);
            return await ExecuteScalar();
        }
        private async Task<Tuple<object, bool>> ExecuteScalar()
        {
            return await Task.Run(() =>
            {
                object data = null;
                bool bReturn = false;
                try
                {
                    if (Connection.State != ConnectionState.Open)
                    {
                        Connection.Open();
                        ConnectionOpened();
                    }
                    data = Command.ExecuteScalar();
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
                return new Tuple<object, bool>(data, bReturn);
            });
        }

        public Task<bool> Execute(dlgReaderOpen function)
        {
            return Task.Run(() =>
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
            });
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
                    //https://www.sqlservertutorial.net/sql-server-system-functions/convert-string-to-datetime/
                    //https://code-maze.com/convert-datetime-to-iso-8601-string-csharp/
                    paramValue = "CONVERT(DATETIME, '" + DateTime.Today.ToString("u").Replace(" ", "T") + "')";
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

        public string GenerateSQL(string ObjectName, string objectType, SQLCommandType sQLCommandType)
        {
            try
            {
                if (Connection.State != ConnectionState.Open)
                {
                    Connection.Open();
                    ConnectionOpened();
                }
                using (var command = Connection.CreateCommand())
                {
                    command.CommandText = $"Select * from [{ObjectName}]";
                    command.CommandType = CommandType.Text;

                    var da = GetDataAdapter(command);
                    da.SelectCommand = (DbCommand)command;
                    var builder = GetCommandBuilder();
                    builder.DataAdapter = da;
                    builder.QuotePrefix = "[";
                    builder.QuoteSuffix = "]";
                    DbCommand generatedCommand = null;
                    var code = string.Empty;
                    switch (sQLCommandType)
                    {
                        case SQLCommandType.Insert:
                            generatedCommand = builder.GetInsertCommand(true);
                            if (generatedCommand == null) return String.Empty;
                            code = generatedCommand.CommandText;
                            break;
                        case SQLCommandType.Update:
                            generatedCommand = builder.GetUpdateCommand(true);
                            if (generatedCommand == null) return String.Empty;
                            code = generatedCommand.CommandText;
                            //remove unnecesary where conditions and match only with primary key value
                            code = code.Remove(code.ToLower().LastIndexOf("where"));
                            for (int i = 0; i < generatedCommand.Parameters.Count; i++)
                            {
                                var paramname = generatedCommand.Parameters[i].ParameterName.ToLower();
                                if (paramname.StartsWith("@isnull_")
                                    || paramname.StartsWith("@original_"))
                                {
                                    generatedCommand.Parameters.RemoveAt(i);
                                    i--;
                                }
                            }
                            break;
                        case SQLCommandType.Delete:
                            //generatedCommand = builder.GetDeleteCommand(true);
                            //if (generatedCommand == null) return String.Empty;
                            //code = generatedCommand.CommandText;
                            //https://stackoverflow.com/questions/14485115/synchronously-waiting-for-an-async-operation-and-why-does-wait-freeze-the-pro
                            var idColumnTask = Task.Run(async () => { return await GetIdentityColumn(ObjectName); });
                            idColumnTask.Wait(); 
                            var idColumn = idColumnTask.Result;
                            if (string.IsNullOrWhiteSpace(idColumn)) idColumn = "PrimaryKey";
                            code = $"DELETE FROM [{ObjectName}] WHERE {idColumn} = ";
                            break;
                    }
                    if (generatedCommand != null)
                    {
                        ProcessParameters(generatedCommand, out var paramStringList, out var paramNameList, out var _);
                        if (paramStringList.Count > 0)
                            code = "DECLARE " + string.Join(Environment.NewLine + "DECLARE ", paramStringList.ToArray())
                                + Environment.NewLine + code;
                    }
                    return code;
                }
            }
            catch (Exception ex)
            {
                if (this.LogError)
                {
                    ErrorText = ex.Message;
                }
            }
            return null;
        }

        public virtual string GenerateCode(string Name, string codeType)
        {
            //todo refactor to Interface 
            string executionMapStartText =
                "FN|*|SELECT <Name/> (" + Environment.NewLine
                + "AF|*|SELECT <Name/> (" + Environment.NewLine
                + "IF|*|SELECT * From <Name/> (" + Environment.NewLine
                + "TF|*|SELECT * From <Name/> (" + Environment.NewLine
                + "P|*|EXECUTE <Return/> <Name/> " + Environment.NewLine;

            string executionMapCloseText =
                "FN|*|)" + Environment.NewLine
                + "AF|*|)" + Environment.NewLine
                + "IF|*|)" + Environment.NewLine
                + "TF|*|)" + Environment.NewLine
                + "P|*|" + Environment.NewLine;


            var startCollection = GetNameValueCollection(executionMapStartText);
            var endCollection = GetNameValueCollection(executionMapCloseText);
            string sProcedure = Environment.NewLine + startCollection[codeType].Replace("<Name/>", Name); // "EXECUTE " + Name + " (";

            Connection.Open();
            var obj = Command;
            DeriveParameters(ref obj);
            ProcessParameters(obj, out var paramStringList, out var paramNameList, out var returnParamName);
            sProcedure = sProcedure.Replace("<Return/> ", $"{returnParamName} = ");
            if (paramStringList.Count > 0)
                sProcedure = "DECLARE " + string.Join(Environment.NewLine + "DECLARE ", paramStringList.ToArray()) + sProcedure;
            if (paramNameList.Count > 0)
                sProcedure += Environment.NewLine + string.Join(Environment.NewLine + ", ", paramNameList.ToArray());
            return sProcedure + endCollection[codeType];
        }

        private void ProcessParameters(IDbCommand obj, out List<string> paramDeclareList, out List<string> paramExecutionList,
            out string ReturnParamName)
        {
            ReturnParamName = string.Empty;
            paramDeclareList = new List<string>();
            paramExecutionList = new List<string>();
            for (int paramIndex = 0; paramIndex < obj.Parameters.Count; paramIndex++)
            {
                var parameter = (DbParameter)obj.Parameters[paramIndex];
                string paramValue = string.Empty;
                if (parameter.IsNullable)
                    paramValue = "NULL";

                var sSize = GetParmeterSize(parameter, ref paramValue);

                string currentParam = string.Empty;
                //string paramName = parameter.ParameterName;
                //if (paramIndex > 1) sProcedure += Environment.NewLine + ", ";
                //if (parameter.Direction == ParameterDirection.Input)
                if (parameter.Direction == ParameterDirection.Output
                            || parameter.Direction == ParameterDirection.InputOutput)
                {
                    paramExecutionList.Add(parameter.ParameterName + " OUTPUT");
                    paramValue = "";
                }
                else if (parameter.Direction == ParameterDirection.ReturnValue)
                {
                    ReturnParamName = parameter.ParameterName;
                    paramValue = "";
                }
                else         //Input parameters
                {
                    paramExecutionList.Add(parameter.ParameterName + " ");
                    paramValue = GetInputParamValue(parameter);
                }

                //declaration logic
                currentParam += parameter.ParameterName + " "
                    + GetParamType(parameter)
                    + sSize;
                if (!string.IsNullOrWhiteSpace(paramValue))
                    currentParam += " = " + paramValue;

                paramDeclareList.Add(currentParam);
            }
        }

        public static NameValueCollection GetNameValueCollection(string input)
        {
            string[] lineSplit = { Environment.NewLine };
            var executionMapArray = input.Split(lineSplit, StringSplitOptions.RemoveEmptyEntries);
            string[] mapKeySplit = { "|*|" };
            var nameValueCollection = new NameValueCollection();
            Array.ForEach(executionMapArray, x =>
            {
                var splits = x.Split(mapKeySplit, StringSplitOptions.None); //keep empty values
                nameValueCollection.Add(splits[0], splits[1]);
            });

            return nameValueCollection;
        }
        public abstract string GetIdentityColumnCode(string sTableName); 

        public async Task<string> GetIdentityColumn(string sTableName)
        {
            string SQL = GetIdentityColumnCode(sTableName);
            if(string.IsNullOrWhiteSpace(SQL))
                return string.Empty;

            var result = await ExecuteScalar(SQL);
            if (result.Item2)
            {
                var objreturn = result.Item1;
                if (objreturn != null && !(objreturn is DBNull))
                {
                    return objreturn.ToString();
                }
            }
            return string.Empty;
        }

    }
}