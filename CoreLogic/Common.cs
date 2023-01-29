using DBStudioLite.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace CoreLogic.PluginBase
{
    public static class Common
    {
        public static string ReadFile(string sFile)
        {
            string sData = "";
            try
            {
                using (StreamReader objfile = new StreamReader(sFile))
                {
                    sData = objfile.ReadToEnd();
                }
            }
            catch
            {
            }
            return sData;

        }
        public static void WriteFile(string sFile, string data)
        {
            using (StreamWriter objfile = new StreamWriter(sFile))
            {
                objfile.Write(data);
            }

        }


        public static DataTable LoadJsonToTable<T>(string fileName)
        {
            var jsonString = File.Exists(fileName) ? System.IO.File.ReadAllText(fileName) : "";
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                //https://stackoverflow.com/questions/11981282/convert-json-to-datatable
                var data = (T)JsonConvert.DeserializeObject(jsonString, (typeof(T)));
                if (data is IBaseData)
                    return (data as IBaseData).SnippetsData;
                else if (data is DataTable)
                    return data as DataTable;
            }
            return new DataTable();
        }


        public static void SaveTableToJson(string fileName, DataTable dt)
        {
            //https://stackoverflow.com/questions/17398019/convert-datatable-to-json-in-c-sharp
            var JSONresult = JsonConvert.SerializeObject(dt);
            System.IO.File.WriteAllText(fileName, JSONresult);
        }

        public static void OpenWithDefaultProgram(string path)
        {
            using (var fileopener = new Process())
            {
                fileopener.StartInfo.FileName = "explorer";
                fileopener.StartInfo.Arguments = "\"" + path + "\"";
                fileopener.Start();
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
    }
}