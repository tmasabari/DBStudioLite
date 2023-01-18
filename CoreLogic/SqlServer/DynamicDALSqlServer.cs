using CoreLogic;
using CoreLogic.BaseDAL;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Wrapper to do all database related operations
/// </summary>
//The using statement applies only to objects that implement the System.IDisposable interface.
//The using statement ensures that Dispose is called even if an exception occurs while you are calling methods on the object. 
//You can achieve the same result by putting the object inside a try block and then calling Dispose in a finally block;

internal class DynamicDALSqlServer : AbstractDAL, IDynamicDAL
{
    #region "Sql Server Terms"
    public string BaseTableType { get; set; }
    public string[] executableType { get; set; }
    public string[] codeTypeCodes { get; set; }
    public string[] codeTypeDescriptions { get; set; }
    public string[] codeTypeKeywordss { get; set; }

    public string GetAllDBsCode { get; set; }
    public string GetAllSchemaCode { get; set; }


    public string CodeModuleFieldName { get; set; }
    //get all types except views as views are already included in the data list.
    public string GetAllDBModulesCode { get; set; }
    public string GetDropCode(string sObjectName, string objectType)
    {
        var SQuery = "IF OBJECT_ID('" + sObjectName + "') IS NOT NULL" + Environment.NewLine
            + "  DROP " + objectType + " " + sObjectName;
        return SQuery;
    }
    public string GetColumnsCode(string sTableName)
    {
        var SQuery = "select '" + sTableName + "' as TableName, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, COLUMN_DEFAULT, NUMERIC_PRECISION, NUMERIC_SCALE,DATETIME_PRECISION "
                + "from INFORMATION_SCHEMA.COLUMNS where table_name = '" + sTableName + "' Order by ORDINAL_POSITION";
        return SQuery;
    }
    public string GetColumnListCode(string sTableName)
    {
        string SQL = "SELECT STRING_AGG ('[' + COLUMN_NAME + ']', ',') AS csv FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'"
            + sTableName + "'";
        return SQL;
    }
    public string GetIdentityColumnCode(string sTableName)
    {
        //https://raresql.com/2012/10/22/sql-server-multiple-ways-to-find-identity-column/
        //SELECT Name FROM sys.columns WHERE object_id = OBJECT_ID('instrument') and is_identity=1 And Objectproperty(object_id,'IsUserTable')=1
        string SQL = "SELECT Name FROM sys.columns WHERE object_id = OBJECT_ID('" + sTableName
            + "') and is_identity=1 And Objectproperty(object_id,'IsUserTable')=1";
        return SQL;
    }
    #endregion

    #region Database

    ~DynamicDALSqlServer() { Dispose(); }

    protected override void ConnectionOpened()
    {
        var sqlconnection = (SqlConnection)Connection;
        sqlconnection.InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
    }
    protected override IDataAdapter DataAdapter(IDbCommand dbCommand)
    {
        return new SqlDataAdapter((SqlCommand)Command);
    }

    //new
    public DynamicDALSqlServer(string sConnection)
    {
        BaseTableType = "BASE TABLE";
        executableType = new string[] { "P", "AF", "IF", "FN", "TF" };

        codeTypeCodes = new string[] { "P", "V", "TR", "AF", "IF", "TF", "FN", "SN" };
        codeTypeDescriptions = new string[] { "SQL_STORED_PROCEDURE", "VIEW", "SQL_TRIGGER",
                    "AGGREGATE_FUNCTION", "SQL_INLINE_TABLE_VALUED_FUNCTION", "SQL_TABLE_VALUED_FUNCTION", "SQL_SCALAR_FUNCTION", "SYNONYMN" };
        codeTypeKeywordss = new string[] { "PROCEDURE", "VIEW", "TRIGGER",
                    "FUNCTION", "FUNCTION", "FUNCTION", "FUNCTION", "SYNONYMN" };

        //https://stackoverflow.com/questions/1819095/sql-server-how-to-tell-if-a-database-is-a-system-database
        //if a database is named master, model, msdb or tempdb, it IS a system db; it is also a system db, if field is_distributor = 1 in the view sys.databases.
        GetAllDBsCode =
            "SELECT name, CAST( IIF( name in ('master','model','msdb','tempdb') , 1 , is_distributor) AS bit) AS [IsSystemObject], "
            + " create_date FROM sys.databases"; //database_id,
        GetAllSchemaCode =
            "SELECT TABLE_NAME,TABLE_SCHEMA,TABLE_TYPE from INFORMATION_SCHEMA.Tables order by TABLE_TYPE, TABLE_SCHEMA, TABLE_NAME; ";
        //"SELECT Name FROM sysobjects WHERE (xtype = 'V') order by Name; " + // AND (status > 0)
        //U - tables V' - views 'S' - system tables
        //TR - trigger FN - scalar function, IF - table valued function, V - view, P - procedure 
        //[type_desc]     IN('SQL_STORED_PROCEDURE','VIEW','SQL_TRIGGER','AGGREGATE_FUNCTION','SQL_INLINE_TABLE_VALUED_FUNCTION','SQL_TABLE_VALUED_FUNCTION','SQL_SCALAR_FUNCTION','SYNONYMN')
        //IN('P','V','TR','AF','IF','FN','TF','SN')
        CodeModuleFieldName = "ModuleName";
        //get all types except views as views are already included in the data list.
        GetAllDBModulesCode = "SELECT o.name as ModuleName, s.name as SchemaName, o.type as Type, modify_date as Modified,create_date as Created "
        + " FROM sys.sql_modules AS m INNER JOIN sys.objects AS o ON m.object_id = o.object_id"
        + " INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id WHERE o.type <> 'V' ORDER By o.type, s.name, o.name; ";

        ErrorText = "";
        LogError = true;

        sConnectionString = sConnection;
        SQLInfoMessageBuilder = new StringBuilder();
        Connection = new SqlConnection(sConnectionString); //Raises error in case of invalid connection string

        Command = Connection.CreateCommand();
        Command.CommandType = CommandType.Text; //this is default type
        ((SqlCommand)Command).StatementCompleted += SelectCommand_StatementCompleted;
    }
    public DynamicDALSqlServer(string sConnection, string sProcedure, bool bLogError, CommandType type)
        : this(sConnection)
    {
        SetValues(sProcedure, bLogError, type);
    }
    public void SetValues(string sProcedure, bool bLogError, CommandType type)
    {
        sProcedureName = sProcedure;
        Command.CommandText = sProcedureName;
        Command.CommandType = type;
        LogError = bLogError;
    }
    public void DeriveParameters(ref IDbCommand obj)
    {
        SqlCommandBuilder.DeriveParameters((SqlCommand)obj);
    }
    public string GetCSharpCodeForParameter(IDataParameter parameter, string sParameterFunction, string sValue)
    {
        SqlParameter sqlParameter = parameter as SqlParameter;
        string sCSharp = string.Empty;
        if (sqlParameter.Size > 0)
            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1}, {2} {3});" + Environment.NewLine,
                parameter.ParameterName, sqlParameter.SqlDbType.ToString("F"), sqlParameter.Size.ToString(), sValue);
        else
            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1} {2});" + Environment.NewLine,
                parameter.ParameterName, sqlParameter.SqlDbType.ToString("F"), sValue); //parameter.DbType.GetType().FullName
        return sCSharp;
    }
    public string GetParmeterSize(IDataParameter dbparameter, ref string paramValue)
    {
        SqlParameter parameter = dbparameter as SqlParameter;
        string sSize;
        if (parameter.Size > 0)
        {
            sSize = "(" + parameter.Size.ToString() + ") ";
            paramValue = "''";
        }
        else if (parameter.Scale > 0)
        {
            sSize = "(" + parameter.Scale.ToString();
            if (parameter.Precision > 0) sSize += "," + parameter.Precision.ToString();
            sSize += ") ";
            paramValue = "0";
        }
        else
            sSize = "";
        return sSize;
    }
    public string GetParamType(IDataParameter dbparameter)
    {
        SqlParameter parameter = dbparameter as SqlParameter;
        return parameter.SqlDbType.ToString("F");
    }

    public string GetInputParamValue(IDataParameter dbparameter)
    {
        SqlParameter parameter = dbparameter as SqlParameter;
        string paramValue;
        switch (parameter.SqlDbType)
        {
            //case SqlDbType.VarBinary:
            //case SqlDbType.Binary:
            //case SqlDbType.Xml:
            //    break;
            //case SqlDbType.Udt:
            //    break;
            //case SqlDbType.Structured:
            //case SqlDbType.Image:
            //case SqlDbType.UniqueIdentifier:

            case SqlDbType.Date:
            case SqlDbType.Time:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
            case SqlDbType.SmallDateTime:
            case SqlDbType.DateTime:
            case SqlDbType.Timestamp:
                paramValue = "'" + DateTime.Today.ToString("dd/MM/yyyy hh:mm") + "'";
                break;

            case SqlDbType.Decimal:
            case SqlDbType.Float:
            case SqlDbType.Bit:
            case SqlDbType.TinyInt:
            case SqlDbType.SmallInt:
            case SqlDbType.Int:
            case SqlDbType.BigInt:
            case SqlDbType.Real:
            case SqlDbType.SmallMoney:
            case SqlDbType.Money:
                paramValue = "0";
                break;

            case SqlDbType.Char:
            case SqlDbType.NChar:
            case SqlDbType.NText:
            case SqlDbType.NVarChar:
            case SqlDbType.Text:
            case SqlDbType.VarChar:
            case SqlDbType.Variant:
                paramValue = "''";
                break;
            default:
                paramValue = "NULL";
                break;
        }

        return paramValue;
    }

    void connection_InfoMessage(object sender, SqlInfoMessageEventArgs args)
    {
        foreach (SqlError sqlError in args.Errors)
        {
            //'{sqlError.Server}'
            var error = $"@{DateTime.Now.ToString("hh:mm:ss FFF")}, @{sqlError.LineNumber} line =>'{sqlError.Message}'";
            if (sqlError.Number > 0) error += $", Error {sqlError.Number}, Class {sqlError.Class}, State {sqlError.State}";
            SQLInfoMessageBuilder.AppendLine(error);
        }
    }
    void SelectCommand_StatementCompleted(object sender, StatementCompletedEventArgs args)
    {
        var rowsMessage = String.Format("({0} row(s) affected)", args.RecordCount);
        SQLInfoMessageBuilder.AppendLine(rowsMessage);
    }


    #endregion
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