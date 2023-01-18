using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace CoreLogic.SqlServer
{
    public static class CodeGeneration
    {
        public static string GetProcedureCSharpCode(string sConnectionString, string Name, int Type
            , out string error)
        {
            error = string.Empty;
            string sCSharp = "", sParameterFunction = "", sValue = "";

            sCSharp = "string sConnectionString = ConfigurationManager.ConnectionStrings[\"MainConnection\"].ConnectionString;" + Environment.NewLine +
                "using (CommonDatabase<SqlConnection, SqlCommand, SqlDataAdapter> DataObj =" + Environment.NewLine +
                "    new CommonDatabase<SqlConnection, SqlCommand, SqlDataAdapter>" + Environment.NewLine +
                "    (sConnectionString, \"" + Name + "\"))" + Environment.NewLine +
                "{" + Environment.NewLine;

            try
            {
                using (IDynamicDAL DataObj = new DynamicDAL(sConnectionString, Name, true,
                    CommandType.StoredProcedure))
                {
                    DataObj.Connection.Open();
                    var obj = DataObj.Command;
                    DataObj.DeriveParameters(ref obj);
                    for (int i = 0; i < obj.Parameters.Count; i++)
                    {
                        var parameter = (SqlParameter)obj.Parameters[i];
                        sValue = "";

                        if (parameter.Direction == ParameterDirection.Input)
                        {
                            sParameterFunction = "AddInputParameter";
                            sValue = ", value" + i.ToString();
                        }
                        else if (parameter.Direction == ParameterDirection.Output
                                        || parameter.Direction == ParameterDirection.InputOutput)
                            sParameterFunction = "AddOutputParameter";
                        else if (parameter.Direction == ParameterDirection.ReturnValue)
                            sParameterFunction = "AddReturnParameter";

                        //todo IDataParameter
                        if (parameter.Size > 0)
                            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1}, {2} {3});" + Environment.NewLine,
                                parameter.ParameterName, parameter.SqlDbType.ToString("F"), parameter.Size.ToString(), sValue);
                        else
                            sCSharp += string.Format("   DataObj." + sParameterFunction + "(\"{0}\", SqlDbType.{1} {2});" + Environment.NewLine,
                                parameter.ParameterName, parameter.SqlDbType.ToString("F"), sValue); //parameter.DbType.GetType().FullName

                    }
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            sCSharp += "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
                        "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
                        "   {" + Environment.NewLine +
                        "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
                        "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
                        "   }" + Environment.NewLine +
                        "}" + Environment.NewLine;

            return sCSharp + Environment.NewLine + Environment.NewLine; // +sProcedure;
        }
        //string sProcedure = ""

        //else
        //    sParameterFunction = "AddIOParameter";

        //sProcedure += "Name:" + parameter.ParameterName;
        //sProcedure += Environment.NewLine + " Type:" + parameter.SqlDbType.ToString("F");
        //sProcedure += Environment.NewLine + " Size:" + parameter.Size.ToString();
        //sProcedure += Environment.NewLine + " Direction: " + parameter.Direction.ToString() +
        //    Environment.NewLine +
        //    Environment.NewLine;


        //"else" + Environment.NewLine +
        //"{" + Environment.NewLine +
        //"    (new ErrorLog()).LogError(new Exception(DataObj.ErrorText));" + Environment.NewLine +
        //"}" + Environment.NewLine + 

        //public static string GetExecutionType(int type)
        //{
        //    string sCSharp = "";

        //    switch (type)
        //    {
        //        case 1: //Dataset
        //            sCSharp = "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
        //                      "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
        //                      "   {" + Environment.NewLine +
        //                      "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
        //                      "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
        //                      "   }" + Environment.NewLine;
        //            break;

        //        case 2: //Reader
        //            sCSharp = "   DataSet DataobjSet = new DataSet();" + Environment.NewLine +
        //                      "   if (DataObj.Execute(\"DataobjTable\", out DataobjSet))" + Environment.NewLine +
        //                      "   {" + Environment.NewLine +
        //                      "      DataTable table = DataobjSet.Tables[\"DataobjTable\"];" + Environment.NewLine +
        //                      "      int iProcedureReturn = (int)DataObj[\"@RETURN_VALUE\"];" + Environment.NewLine +
        //                      "   }" + Environment.NewLine;
        //            break;

        //        case 3: //simple
        //            sCSharp = "   if (DataObj.Execute())" + Environment.NewLine +
        //                      "   {" + Environment.NewLine +
        //                      "   }" + Environment.NewLine;
        //            break;

        //        case 4: //Scalar
        //            sCSharp = "   Object DataobjSet = null;" + Environment.NewLine +
        //                      "   if (DataObj.Execute(out DataobjSet))" + Environment.NewLine +
        //                      "   {" + Environment.NewLine +
        //                      "   }" + Environment.NewLine;
        //            break;
        //    }

        //    sCSharp += "}" + Environment.NewLine;
        //    return sCSharp;
        //}
        //"else" + Environment.NewLine +
        //"{" + Environment.NewLine +
        //"    (new ErrorLog()).LogError(new Exception(DataObj.ErrorText));" + Environment.NewLine +
        //"}" + Environment.NewLine + 
    }
}
