using CoreLogic.PluginBase;
using CoreLogic.PluginBase.PluginBase;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

/// <summary>
/// Wrapper to do all database related operations
/// replaced below class names
/// SqlConnection       SQLiteConnection
/// SqlDataAdapter      SQLiteDataAdapter
/// SqlCommand          SQLiteCommand
/// SQLiteParameter     SQLiteParameter
/// https://github.com/prahladyeri/sqlite-gui
/// </summary>
//The using statement applies only to objects that implement the System.IDisposable interface.
//The using statement ensures that Dispose is called even if an exception occurs while you are calling methods on the object. 
//You can achieve the same result by putting the object inside a try block and then calling Dispose in a finally block;

public class DynamicDALSQLite : AbstractDAL, IDynamicDAL
{
    #region "Sql Server Terms"
    //https://www.sqlite.org/schematab.html
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

    public string GetColumnsCode(string sTableName)
    {
        //select 'albums' as TableName, name as COLUMN_NAME, type as DATA_TYPE, NULL as CHARACTER_MAXIMUM_LENGTH,  dflt_value as COLUMN_DEFAULT, NULL as NUMERIC_PRECISION, NULL as NUMERIC_SCALE, NULL as DATETIME_PRECISION FROM pragma_table_info('albums') Order by cid;
        var SQuery = "select '" + sTableName + "' as TableName, name as COLUMN_NAME, type as DATA_TYPE, NULL as CHARACTER_MAXIMUM_LENGTH,  IIF([notnull] = 0, 1, 0) as IS_NULLABLE, dflt_value as COLUMN_DEFAULT, NULL as NUMERIC_PRECISION, NULL as NUMERIC_SCALE, NULL as DATETIME_PRECISION "
                + "FROM pragma_table_info('" + sTableName + "') Order by cid";
        return SQuery;
    }
    public string GetColumnListCode(string sTableName)
    {
        //todo PRAGMA table_info(table_name); https://www.sqlite.org/pragma.html
        //cid|name|type|notnull|dflt_value|pk
        //0|AlbumId|INTEGER|1||1
        //1|Title|NVARCHAR(160)|1||0
        //2|ArtistId|INTEGER|1||0
        string SQL = "SELECT group_concat (name, ',') AS csv FROM pragma_table_info('" + sTableName + "')";
        return SQL;
    }
    public string GetIdentityColumnCode(string sTableName)
    {
        //https://stackoverflow.com/questions/20979239/how-to-tell-if-a-sqlite-column-is-autoincrement
        string SQL = "";
        return SQL;
    }
    public string GetTableRowsCode(string sTableName, int Rows, bool isReverse = false, string columnList = null)
    {
        string sQuery = "";
        sQuery = "select " + sQuery + " " + (!string.IsNullOrEmpty(columnList) ? columnList : "*") + " from " + sTableName;
        if (isReverse)
        {
            var columnName = !string.IsNullOrEmpty(columnList) ? GetFirstColumn(columnList) : "1";
            sQuery += $" order by {columnName} desc";
        }
        //https://www.sqlitetutorial.net/sqlite-limit/
        if (Rows != -1) sQuery += " LIMIT  " + Rows.ToString();
        return sQuery;
    }

    public virtual string GetDropCode(string sObjectName, string objectType)
    {
        //sqlite_schema
        //type|name|tbl_name|rootpage|sql
        //index|IFK_TrackGenreId|tracks|31|CREATE INDEX [IFK_TrackGenreId] ON "tracks" ([GenreId])
        //https://database.guide/drop-table-if-exists-in-sqlite/
        var SQuery = $"DROP {objectType}  IF EXISTS {sObjectName}";
        return SQuery;
    }
    public string GetModuleCode(string sModuleType, string sModuleName)
    {
        if (sModuleName.Contains(".")) sModuleName = sModuleName.Substring(sModuleName.IndexOf(".") + 1);
        sModuleName = sModuleName.Trim('[', ']');
        return "select sql from sqlite_schema where name = '" + sModuleName + "';";
    }
    #endregion

    #region Database

    ~DynamicDALSQLite() { Dispose(); }

    protected override void ConnectionOpened()
    {
        //unsupported
        //var SQLiteConnection  = (SQLiteConnection )Connection;
        //SQLiteConnection .InfoMessage += new SqlInfoMessageEventHandler(connection_InfoMessage);
    }
    protected override DbDataAdapter GetDataAdapter(IDbCommand dbCommand)
    {
        return new SQLiteDataAdapter((SQLiteCommand)Command);
    }

    protected override DbCommandBuilder GetCommandBuilder()
    {
        return new SQLiteCommandBuilder();
    }

    //new
    public DynamicDALSQLite(string sConnection)
    {
        BaseTableType = "TABLE"; //must be upper case
        executableType = new string[] { };
        //'table', 'index', 'view', or 'trigger' 
        codeTypeCodes = new string[] { "trigger" };
        codeTypeDescriptions = new string[] { "VIEW", "SQL_TRIGGER" };
        codeTypeKeywordss = new string[] { "VIEW", "TRIGGER" };

        //https://stackoverflow.com/questions/1819095/sql-server-how-to-tell-if-a-database-is-a-system-database
        //if a database is named master, model, msdb or tempdb, it IS a system db; it is also a system db, if field is_distributor = 1 in the view sys.databases.
        GetAllDBsCode = "";
        //https://www.sqlite.org/pragma.html#pragma_table_list
        GetAllSchemaCode = "SELECT name as TABLE_NAME, schema as TABLE_SCHEMA, type as TABLE_TYPE FROM pragma_table_list ORDER BY type, name;";
        CodeModuleFieldName = "ModuleName";
        //get all types except views as views are already included in the data list.
        GetAllDBModulesCode = "SELECT name as ModuleName, '' as TABLE_SCHEMA, type as Type, NULL as Modified,NULL as Created FROM sqlite_schema WHERE type IN( 'trigger') ORDER BY type, name;";

        ErrorText = "";
        LogError = true;

        sConnectionString = sConnection;
        SQLInfoMessageBuilder = new StringBuilder();
        Connection = new SQLiteConnection(sConnectionString); //Raises error in case of invalid connection string

        Command = Connection.CreateCommand();
        Command.CommandType = CommandType.Text; //this is default type
        //unsupported
        //((SQLiteCommand)Command).StatementCompleted += SelectCommand_StatementCompleted;
    }
    public DynamicDALSQLite(string sConnection, string sProcedure, bool bLogError, CommandType type)
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

    public string GetCSharpCodeForParameter(IDataParameter parameter, string sParameterFunction, string sValue)
    {
        var SQLiteParameter = parameter as SQLiteParameter;
        string sCSharp = string.Empty;
        if (SQLiteParameter.Size > 0)
            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", DbType.{1}, {2} {3});" + Environment.NewLine,
                parameter.ParameterName, SQLiteParameter.DbType.ToString("F"), SQLiteParameter.Size.ToString(), sValue);
        else
            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", DbType.{1} {2});" + Environment.NewLine,
                parameter.ParameterName, SQLiteParameter.DbType.ToString("F"), sValue); //parameter.DbType.GetType().FullName
        return sCSharp;
    }

    //unsupported
    //void connection_InfoMessage(object sender, SqlInfoMessageEventArgs args)
    //{
    //    foreach (SqlError sqlError in args.Errors)
    //    {
    //        //'{sqlError.Server}'
    //        var error = $"@{DateTime.Now.ToString("hh:mm:ss FFF")}, @{sqlError.LineNumber} line =>'{sqlError.Message}'";
    //        if (sqlError.Number > 0) error += $", Error {sqlError.Number}, Class {sqlError.Class}, State {sqlError.State}";
    //        SQLInfoMessageBuilder.AppendLine(error);
    //    }
    //}
    //void SelectCommand_StatementCompleted(object sender, StatementCompletedEventArgs args)
    //{
    //    var rowsMessage = String.Format("({0} row(s) affected)", args.RecordCount);
    //    SQLInfoMessageBuilder.AppendLine(rowsMessage);
    //}


    #endregion
}