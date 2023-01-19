using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLogic
{
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
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                return DataObj.GetTableRowsCode(sTableName, Rows, isReverse, columnList);
            }
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

        //todo change for sqlite
        public async Task<Tuple<string, string>> GetProcedureDefinition(string sConnectionString, string Name)
        {
            var error = string.Empty;
            string sProcedure = "";
            using (IDynamicDAL DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                var sQuery = DataObj.GetModuleCode(string.Empty, Name);
                DataObj.SetValues(sQuery, true, CommandType.Text);
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
                    if (!(objreturn is DBNull))
                    {
                        return objreturn.ToString();
                    }
                }
                return string.Empty;
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
                    if (objreturn != null && !(objreturn is DBNull))
                    {
                        return objreturn.ToString();
                    }
                }
                return string.Empty;
            }
        }
        #endregion
    }
}