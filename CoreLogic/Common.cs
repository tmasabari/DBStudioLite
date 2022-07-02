using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DBStudioLite
{
    static class Common
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

        /// <summary>
        /// https://stackoverflow.com/questions/1025670/how-do-you-automatically-resize-columns-in-a-datagridview-control-and-allow-the
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void AutoSizeGridView(DataGridView dataGridView, int fillColumn = -1)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            if (fillColumn > -1)
                dataGridView.Columns[fillColumn].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dataGridView.Refresh();
            //// Now that DataGridView has calculated it's Widths; we can now store each column Width values.
            //for (int i = 0; i <= dataGridView.Columns.Count - 1; i++)
            //{
            //    // Store Auto Sized Widths:
            //    int colw = dataGridView.Columns[i].Width;

            //    // Remove AutoSizing:
            //    dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            //    // Set Width to calculated AutoSize value:
            //    dataGridView.Columns[i].Width = colw;
            //}
        }

        /// <summary>
        /// https://stackoverflow.com/questions/1706454/c-multiline-text-in-datagridview-control
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void SupportMultipleLineCells(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
        }


        public static DataTable LoadJsonToTable(string fileName)
        {
            var jsonString = File.Exists(fileName) ? System.IO.File.ReadAllText(fileName) : "";
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                //https://stackoverflow.com/questions/11981282/convert-json-to-datatable
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(jsonString, (typeof(DataTable)));
                return dt;
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