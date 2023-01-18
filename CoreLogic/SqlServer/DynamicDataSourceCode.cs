using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLogic.SqlServer
{
    //http://www.stormrage.com/SQLStuff/sp_GetDDL_Latest.txt
    public class DynamicDataSourceCode
    {
        private string sConnectionString;
        public DynamicDataSourceCode(string connectionstring) 
        {
            sConnectionString = connectionstring;
        }
        public string GetSelectedCodeType(string UICodeType)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                int index = Array.IndexOf(DataObj.codeTypeCodes, UICodeType.Trim());
                if (index >= 0)
                {
                    return DataObj.codeTypeKeywordss[index];
                }
                else
                    return "";
            }
        }
        public string GetAllDBsCode
        {
            get
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
                {
                    return DataObj.GetAllDBsCode;
                }
            }
        }
        public string GetAllSchemaCode
        {
            get
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
                {
                    return DataObj.GetAllSchemaCode;
                }
            }
        }
        public string GetAllDBModulesCode
        {
            get
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
                {
                    return DataObj.GetAllDBModulesCode;
                }
            }
        }
        public string BaseTableType
        {
            get
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
                {
                    return DataObj.BaseTableType;
                }
            }
        }
        public string[] executableType
        {
            get
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
                {
                    return DataObj.executableType;
                }
            }
        }
        public string GetColumnsCode(string sTableName)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                return DataObj.GetColumnsCode(sTableName);
            }
        }
        public string GetDropCode(string sObjectName, string objectType)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                return DataObj.GetDropCode(sObjectName, objectType);
            }
        }

        //GetAllDBModulesCode
        public string GetTableRowsCode(string sTableName, int Rows, bool isReverse = false, string columnList = null)
        {
            string sQuery = "";
            if (Rows != -1) sQuery = " top " + Rows.ToString() + " ";
            sQuery = "select " + sQuery + " " + (!string.IsNullOrEmpty(columnList) ? columnList : "*") + " from " + sTableName;
            if (isReverse)
            {
                var columnName = !string.IsNullOrEmpty(columnList) ? getFirstColumn(columnList) : "1";
                sQuery += $" order by {columnName} desc";
            }
            return sQuery;
        }
        private static string getFirstColumn(string columnList)
        {
            string[] separator = { "," };
            return columnList.Split(separator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

        public string GetProcedureRun(string sConnectionString, string Name, string codeType, out string error)
        {
            error = string.Empty;
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


            var startCollection = Common.GetNameValueCollection(executionMapStartText);
            var endCollection = Common.GetNameValueCollection(executionMapCloseText);
            string sProcedure = Environment.NewLine + startCollection[codeType].Replace("<Name/>", Name); // "EXECUTE " + Name + " (";
            try
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString, Name, true,
                    CommandType.StoredProcedure))
                {
                    string sSize = "";

                    DataObj.Connection.Open();
                    var obj = DataObj.Command;
                    DataObj.DeriveParameters(ref obj);
                    var paramStringList = new List<string>();
                    var paramNameList = new List<string>();
                    for (int paramIndex = 1; paramIndex < obj.Parameters.Count; paramIndex++)
                    {
                        var parameter = (IDataParameter)obj.Parameters[paramIndex];
                        string paramValue = string.Empty;
                        if (parameter.IsNullable)
                            paramValue = "NULL";

                        sSize = DataObj.GetParmeterSize(parameter, ref paramValue);

                        string currentParam = string.Empty;
                        string paramName = parameter.ParameterName;
                        //if (paramIndex > 1) sProcedure += Environment.NewLine + ", ";
                        string paramType = string.Empty;
                        //if (parameter.Direction == ParameterDirection.Input)
                        if (parameter.Direction == ParameterDirection.Output
                                    || parameter.Direction == ParameterDirection.InputOutput)
                        {
                            paramType = "OUTPUT";
                            paramValue = "";
                        }
                        else if (parameter.Direction == ParameterDirection.ReturnValue)
                        {
                            paramType = "RETURN";
                            paramValue = "";
                        }
                        else         //Input parameters
                        {
                            paramValue = DataObj.GetInputParamValue(parameter);
                        }
                        //if (parameter.Value != null)
                        //    sValue = " = " + parameter.Value.ToString() + " ";
                        //else
                        //    sValue = "";

                        paramNameList.Add(parameter.ParameterName + " " + paramType);
                        currentParam += parameter.ParameterName + " "
                            + DataObj.GetParamType(parameter)
                            + sSize;
                        if (!string.IsNullOrWhiteSpace(paramValue))
                            currentParam += " = " + paramValue;

                        paramStringList.Add(currentParam);
                    }
                    if (paramStringList.Count > 0)
                        sProcedure = "DECLARE " + string.Join(Environment.NewLine + "DECLARE ", paramStringList.ToArray()) + sProcedure;
                    if (paramNameList.Count > 0)
                        sProcedure += Environment.NewLine + string.Join(Environment.NewLine + ", ", paramNameList.ToArray());
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return sProcedure + endCollection[codeType];

            //remove start brace and comma and dont use end brace + ")"
            //return sProcedure.Replace("(" + Environment.NewLine + ",", Environment.NewLine)
            //    + Environment.NewLine;
        }

        public async Task<Tuple<string, string>> GetProcedureDefinition(string sConnectionString, string Name)
        {
            var error = string.Empty;
            //string tablename = Name.Substring(Name.LastIndexOf(".")+1);
            //string sQuery = "SELECT ROUTINE_DEFINITION FROM INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = '" + tablename + "' and ROUTINE_TYPE = '" + sType + "'";
            string sQuery = "EXEC sp_helptext N'" + Name + "';";
            //string sQuery = "SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + Name + "'));";
            //string sQuery = "SELECT definition FROM sys.sql_modules WHERE object_id = (OBJECT_ID(N'" + Name + "'));";
            //object objReturn;
            string sProcedure = "";
            using (IDynamicDAL DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString, sQuery, true, CommandType.Text))
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
                    error = DataObj.ErrorText;
                    //MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                }
                //if (DataObj.ExecuteScalar(out objReturn) == false)
                //    MessageBox.Show("Error occured" + DataObj.ErrorText, Application.ProductName, MessageBoxButtons.OK);
                //else
                //    sProcedure = (string) objReturn;
            }
            return new Tuple<string, string>(sProcedure, error); ;
        }


        #region Table properties
        public string GetColumnList(string sConnectionString, string sTableName)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                string SQL = DataObj.GetColumnListCode(sTableName);
                DataObj.SetValues(SQL, true, CommandType.Text);
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

        public string GetIdentityColumn(string sConnectionString, string sTableName)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                string SQL = DataObj.GetIdentityColumnCode(sTableName);
                DataObj.SetValues(SQL, true, CommandType.Text);
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
}

//public bool CheckColumIdentity(string TableName, string ColumnName)
//{
//    string SQL;

//    SQL = "SELECT COLUMNPROPERTY( OBJECT_ID('" + TableName + "'),'" + ColumnName + "','IsIdentity')";
//    string lsConnection = sConnectionString;
//    using (DynamicDAL DataObj = DataAccessFactory.GetDynamicDAL(lsConnection, SQL, true, CommandType.Text))
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