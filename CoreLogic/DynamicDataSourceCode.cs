using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBStudioLite
{
    //http://www.stormrage.com/SQLStuff/sp_GetDDL_Latest.txt
    public static class DynamicDataSourceCode
    {
        public static readonly string BaseTableType = "BASE TABLE";
        public static readonly string[] executableType = { "P", "AF", "IF", "FN", "TF" };

        public static readonly string[] codeTypeCodes = { "P", "V", "TR", "AF", "IF", "TF", "FN", "SN" };
        public static readonly string[] codeTypeDescriptions = { "SQL_STORED_PROCEDURE", "VIEW", "SQL_TRIGGER",
                "AGGREGATE_FUNCTION", "SQL_INLINE_TABLE_VALUED_FUNCTION", "SQL_TABLE_VALUED_FUNCTION", "SQL_SCALAR_FUNCTION", "SYNONYMN" };
        public static readonly string[] codeTypeKeywordss = { "PROCEDURE", "VIEW", "TRIGGER",
                "FUNCTION", "FUNCTION", "FUNCTION", "FUNCTION", "SYNONYMN" };

        //https://stackoverflow.com/questions/1819095/sql-server-how-to-tell-if-a-database-is-a-system-database
        //if a database is named master, model, msdb or tempdb, it IS a system db; it is also a system db, if field is_distributor = 1 in the view sys.databases.
        public static readonly string GetAllDBsCode =
            "SELECT name, CAST( IIF( name in ('master','model','msdb','tempdb') , 1 , is_distributor) AS bit) AS [IsSystemObject], "
            + " create_date FROM sys.databases"; //database_id,
        public static readonly string GetAllSchemaCode =
            "SELECT TABLE_NAME,TABLE_SCHEMA,TABLE_TYPE from INFORMATION_SCHEMA.Tables order by table_type, table_name; ";
        //"SELECT Name FROM sysobjects WHERE (xtype = 'V') order by Name; " + // AND (status > 0)
        //U - tables V' - views 'S' - system tables
        //TR - trigger FN - scalar function, IF - table valued function, V - view, P - procedure 
        //[type_desc]     IN('SQL_STORED_PROCEDURE','VIEW','SQL_TRIGGER','AGGREGATE_FUNCTION','SQL_INLINE_TABLE_VALUED_FUNCTION','SQL_TABLE_VALUED_FUNCTION','SQL_SCALAR_FUNCTION','SYNONYMN')
        //IN('P','V','TR','AF','IF','FN','TF','SN')


        public static string GetSelectedCodeType(string UICodeType)
        {
            int index = Array.IndexOf(codeTypeCodes, UICodeType.Trim());
            if (index >= 0)
            {
                return codeTypeKeywordss[index];
            }
            else
                return "";
        }

        public static readonly string CodeModuleFieldName = "ModuleName";
        //get all types except views as views are already included in the data list.
        public static readonly string GetAllDBModulesCode
            = "SELECT o.name as ModuleName, s.name as SchemaName, o.type as Type, modify_date as Modified,create_date as Created "
            + " FROM sys.sql_modules AS m INNER JOIN sys.objects AS o ON m.object_id = o.object_id"
            + " INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id WHERE o.type <> 'V' ORDER By o.type; ";
        public static string GetDropCode(string sObjectName, string objectType)
        {
            var SQuery = "IF OBJECT_ID('" + sObjectName + "') IS NOT NULL" + Environment.NewLine
                + "  DROP " + objectType + " " + sObjectName;
            return SQuery;
        }
        public static string GetColumnsCode(string sTableName)
        {
            var SQuery = "select '" + sTableName + "' as TableName, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where table_name = '" + sTableName + "' ";
            return SQuery;
        }
        public static string GetColumnListCode(string sTableName)
        {
            string SQL = "SELECT STRING_AGG (COLUMN_NAME, ',') AS csv FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'"
                + sTableName + "'";
            return SQL;
        }
        public static string GetIdentityColumnCode(string sTableName)
        {
            //https://raresql.com/2012/10/22/sql-server-multiple-ways-to-find-identity-column/
            //SELECT Name FROM sys.columns WHERE object_id = OBJECT_ID('instrument') and is_identity=1 And Objectproperty(object_id,'IsUserTable')=1
            string SQL = "SELECT Name FROM sys.columns WHERE object_id = OBJECT_ID('" + sTableName
                + "') and is_identity=1 And Objectproperty(object_id,'IsUserTable')=1";
            return SQL;
        }

        public static string GetTableRowsCode(string sTableName, int Rows, bool isReverse = false, string columnList = null)
        {
            string sQuery = "";
            if (Rows != -1) sQuery = " top " + Rows.ToString() + " ";
            sQuery = "select " + sQuery + " " + (!string.IsNullOrEmpty(columnList) ? columnList : "*") + " from " + sTableName;
            if (isReverse) sQuery += " order by 1 desc";
            return sQuery;
        }

        public static string GetProcedureRun(string sConnectionString, string Name, string codeType)
        {
            string executionMapStartText =
                "FN|*|SELECT <Name/> (" + Environment.NewLine
                + "AF|*|SELECT <Name/> (" + Environment.NewLine
                + "IF|*|SELECT * From <Name/> (" + Environment.NewLine
                + "TF|*|SELECT * From <Name/> (" + Environment.NewLine
                + "P|*|EXECUTE <Name/> " + Environment.NewLine;

            string executionMapCloseText =
                "FN|*|)" + Environment.NewLine
                + "AF|*|)" + Environment.NewLine
                + "IF|*|)" + Environment.NewLine
                + "TF|*|)" + Environment.NewLine
                + "P|*|" + Environment.NewLine;


            var startCollection = DBStudioLite.Common.GetNameValueCollection(executionMapStartText);
            var endCollection = DBStudioLite.Common.GetNameValueCollection(executionMapCloseText);
            string sProcedure = Environment.NewLine + startCollection[codeType].Replace("<Name/>", Name); // "EXECUTE " + Name + " (";
            SqlParameter parameter = null;
            try
            {
                using (DynamicDAL DataObj = new DynamicDAL(sConnectionString, Name, true,
                    CommandType.StoredProcedure))
                {
                    string sSize = "";
                    string sValue = "";

                    DataObj.connection.Open();
                    SqlCommand obj = DataObj.command;
                    SqlCommandBuilder.DeriveParameters(obj);
                    for (int paramIndex = 1; paramIndex < obj.Parameters.Count; paramIndex++)
                    {
                        parameter = obj.Parameters[paramIndex];

                        if (parameter.Size > 0)
                            sSize = "(" + parameter.Size.ToString() + ") ";
                        else
                            sSize = "";
                        if (parameter.Value != null)
                            sValue = " = " + parameter.Value.ToString() + " ";
                        else
                            sValue = "";

                        if (paramIndex > 1) sProcedure += Environment.NewLine + ", ";
                        if (parameter.Direction == ParameterDirection.Input)
                            sProcedure += parameter.ParameterName + " " + parameter.SqlDbType.ToString("F")
                                + sSize + sValue;
                        else if (parameter.Direction == ParameterDirection.Output
                                    || parameter.Direction == ParameterDirection.InputOutput)
                            sProcedure += parameter.ParameterName + " " + parameter.SqlDbType.ToString("F")
                                + sSize + " OUTPUT" + sValue;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured" + e.Message, Application.ProductName, MessageBoxButtons.OK);
            }
            return sProcedure + endCollection[codeType];
            //remove start brace and comma and dont use end brace + ")"
            //return sProcedure.Replace("(" + Environment.NewLine + ",", Environment.NewLine)
            //    + Environment.NewLine;
        }

        public static async Task<string> GetProcedureDefinition(string sConnectionString, string Name)
        {
            //string tablename = Name.Substring(Name.LastIndexOf(".")+1);
            //string sQuery = "SELECT ROUTINE_DEFINITION FROM INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = '" + tablename + "' and ROUTINE_TYPE = '" + sType + "'";
            string sQuery = "EXEC sp_helptext N'" + Name + "';";
            //string sQuery = "SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + Name + "'));";
            //string sQuery = "SELECT definition FROM sys.sql_modules WHERE object_id = (OBJECT_ID(N'" + Name + "'));";
            //object objReturn;
            string sProcedure = "";
            using (DynamicDAL DataObj = new DynamicDAL(sConnectionString, sQuery, true, CommandType.Text))
            {
                var ds = await DataObj.Execute("MyTable");
                //task.Wait();
                //var ds = task.Result;
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables["MyTable"].Rows.Count; i++)
                    {
                        sProcedure = sProcedure + (string)ds.Tables["MyTable"].Rows[i].ItemArray[0]; // +Environment.NewLine;
                    }
                    //.Text = "Total lines :" + ds.Tables["MyTable"].Rows.Count.ToString();
                }
                else
                {
                    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
                //if (DataObj.ExecuteScalar(out objReturn) == false)
                //    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                //else
                //    sProcedure = (string) objReturn;
            }
            return sProcedure;
        }
    }
}

//public bool CheckColumIdentity(string TableName, string ColumnName)
//{
//    string SQL;

//    SQL = "SELECT COLUMNPROPERTY( OBJECT_ID('" + TableName + "'),'" + ColumnName + "','IsIdentity')";
//    string lsConnection = sConnectionString;
//    using (DynamicDAL DataObj = new DynamicDAL(lsConnection, SQL, true, CommandType.Text))
//    {
//        object objreturn;
//        if (DataObj.ExecuteScalar(out objreturn))
//        {
//            if (!(objreturn is System.DBNull))
//            {
//                if ((int)objreturn == 1)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }
//        return false;
//    }
//}


//Now, let's turn to another routine that I'll need to complete the search: GetTableDetails (see Figure 4). This routine uses the SQL query shown here to retrieve the details of a particular table, including field names, datatype, and maximum length: 
// SQL = "select '" & TableName & "' as TableName, " _
//    SQL &= "COLUMN_NAME, DATA_TYPE, " _
//    SQL &= "CHARACTER_MAXIMUM_LENGTH  from " _
//    SQL &= "INFORMATION_SCHEMA.COLUMNS where " _
//    SQL &= "table_name = '" & TableName & "' "

//GetTableDetails is called when the user clicks the Search button (called cmdSearch). More on this soon.

//One more function that I need to look at is CheckColumnIdentity. 
//Function CheckColumnIdentity(ByVal TableName As String, _
//    ByVal ColumnName As String) As Boolean
//    Dim SQL As String
//    SQL = "SELECT COLUMNPROPERTY( OBJECT_ID('" & " _
//    SQL &= "TableName & "'),'" & ColumnName & _
//    SQL &= "','IsIdentity')"
//    If CInt(RunSQLScalar(SQL)) = 1 Then
//        Return True
//    Else
//        Return False
//    End If
//End Function
//This function takes a table name and column name as parameters and returns a Boolean value, indicating whether the column is an identity column. It uses COLUMNPROPERTY to determine if the column is an identity column. You can also use this property to determine if a column allows nulls, and so on.

//Now, let's put this together by looking at the cmdSearch Click event code. The first few lines of code define the variables I'll need: 
//Dim dt, dttemp As DataTable
//Dim ds As New DataSet
//Dim tabletosearch As String
//Dim rw As DataRow
//Dim IsMatch As Boolean = False

//The following two lines set tabletosearch to the name of the table to search and clears the output textbox: 
//tabletosearch = cboTables.Text
//txtOutput.ResetText()

//The next few lines control how the search is handled. If chkAllTables is True, then all of the details for each table are loaded into the dt table. If chkAllTables is False, then dt is loaded with the details for only the selected table: 
//If chkAllTables.Checked Then
//    For Each rw In dsTables.Tables(0).Rows
//        dttemp = GetTableDetails(rw("Name").ToString)
//        ds.Merge(dttemp)
//    Next
//    dt = ds.Tables(0)
//Else
//    dt = GetTableDetails(tabletosearch)
//End If

//Now that the tables are loaded, I can perform the search. The search is handled inside the For Each loop, which moves through all rows in the dt table: 
//For Each rw In dt.Rows
//The next line sets IsMatch to False as the default for each field. IsMatch is a flag that determines whether to output the field: 
//IsMatch = False

//Next, if chkContains is True, then InStr is used to determine if a match exists. If chkContains is False, then the Else clause checks for an exact match. If a match is found, IsMatch is set to True: 
//If chkContains.Checked Then
//    If InStr(UCase(rw("ColumnName").ToString), _
//        UCase(txtSearchField.Text)) > 0 Then
//        IsMatch = True
//    End If
//Else
//    If UCase(rw("ColumnName").ToString) = UCase(txtSearchField.Text) Then
//        IsMatch = True
//    End If
//End If
//Now that the test is complete, I can output the data if IsMatch is True. The values in the various fields in the datatable are output to txtOutput, as shown here: 
//If IsMatch Then
//    txtOutput.Text &= rw("TableName").ToString _
//        & " : " & rw("ColumnName").ToString & " - "
//    txtOutput.Text &= rw("DataType").ToString _
//        & " (" & rw("Length").ToString & ")"

//    If CBool(rw("Identity").ToString) Then
//        txtOutput.Text &= " Identity"
//End If
//        txtOutput.Text &= vbCrLf
//    End If
//Next
//That's it. There is not a lot of code to this tool, but it sure is handy.