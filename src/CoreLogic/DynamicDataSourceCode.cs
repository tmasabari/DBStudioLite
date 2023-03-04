using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CoreLogic.PluginBase.PluginBase;

namespace CoreLogic.PluginBase
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


        public string GetDMLCode(string sObjectName, string objectType, SQLCommandType sQLCommandType )
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                return DataObj.GenerateSQL(sObjectName, objectType, sQLCommandType);
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
            try
            {
                using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString, Name, true,
                    CommandType.StoredProcedure))
                {
                    return DataObj.GenerateCode(Name, codeType);
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return String.Empty;
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
        public async Task<string> GetColumnList(string sConnectionString, string sTableName)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                string SQL = DataObj.GetColumnListCode(sTableName);
                var result = await DataObj.ExecuteScalar(SQL);
                if (result.Item2)
                {
                    var objreturn = result.Item1;
                    if (!(objreturn is DBNull))
                    {
                        return objreturn.ToString();
                    }
                }
                return string.Empty;
            }
        }

        public async Task<string> GetIdentityColumn(string sConnectionString, string sTableName)
        {
            using (var DataObj = DataAccessFactory.GetDynamicDAL(sConnectionString))
            {
                string SQL = DataObj.GetIdentityColumnCode(sTableName);
                var result = await DataObj.ExecuteScalar(SQL);
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
        public async Task<DataTable> GetTableDetails(string TableName)
        {
            DataSet ds;
            string localColumn;
            string localDataType = "";
            int localLength = 0, scale = 0;
            bool localIdentity;
            string SQuery;

            DataTable dt = new DataTable("TableInfo"); //output table
            DataRow workrow;
            string lsConnection = sConnectionString;

            using (var DataObj = DataAccessFactory.GetDynamicDAL(lsConnection))
            {
                SQuery = DataObj.GetColumnsCode(TableName);
                DataObj.SetValues(SQuery, true, CommandType.Text);
                ds = await DataObj.Execute("RawTableInfo");
                if (ds != null)
                {
                    dt.Columns.Add("TableName", Type.GetType("System.String"));
                    dt.Columns.Add("ColumnName", Type.GetType("System.String"));

                    dt.Columns.Add("IsNullable", Type.GetType("System.Boolean"));
                    dt.Columns.Add("DefaultValue", Type.GetType("System.String"));

                    dt.Columns.Add("DataType", Type.GetType("System.String"));
                    dt.Columns.Add("Length", Type.GetType("System.Int64"));
                    dt.Columns.Add("DecimalPlaces", Type.GetType("System.Int64"));
                    dt.Columns.Add("Identity", Type.GetType("System.Boolean"));

                    var identityColumn = await GetIdentityColumn(sConnectionString, TableName);

                    foreach (DataRow rw in ds.Tables["RawTableInfo"].Rows)
                    {
                        scale = 0;
                        if (!System.Convert.IsDBNull(rw["Column_Name"]))
                        {
                            localColumn = rw["Column_Name"].ToString();
                            if (!System.Convert.IsDBNull(rw["DATA_TYPE"]))
                            {
                                localDataType = rw["DATA_TYPE"].ToString();
                            }
                            if (!System.Convert.IsDBNull(rw["CHARACTER_MAXIMUM_LENGTH"]))
                            {
                                localLength = int.Parse(rw["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            }
                            else if (!System.Convert.IsDBNull(rw["NUMERIC_PRECISION"]))
                            {
                                localLength = int.Parse(rw["NUMERIC_PRECISION"].ToString());
                                scale = int.Parse(rw["NUMERIC_SCALE"].ToString());
                            }
                            //showing 3 for datetime which is not length but precision so disabled
                            //else if (!System.Convert.IsDBNull(rw["DATETIME_PRECISION"]))
                            //{
                            //    localLength = int.Parse(rw["DATETIME_PRECISION"].ToString());
                            //}
                            else
                            {
                                localLength = 0;
                            }
                            localIdentity = localColumn == identityColumn;

                            workrow = dt.NewRow();
                            workrow["TableName"] = TableName;
                            workrow["ColumnName"] = localColumn;
                            workrow["IsNullable"] = rw["IS_NULLABLE"];
                            workrow["DefaultValue"] = rw["COLUMN_DEFAULT"].ToString();
                            workrow["Datatype"] = localDataType;
                            if (localLength > 0) workrow["Length"] = localLength;
                            if (scale > 0) workrow["DecimalPlaces"] = scale;
                            workrow["Identity"] = localIdentity;
                            dt.Rows.Add(workrow);
                        }
                    }
                }
            }
            return dt;

        }

        #endregion
    }
}