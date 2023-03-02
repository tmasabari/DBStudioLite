using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreLogic.PluginBase;
using DBStudioLite.Plugins;
namespace DBStudioLite
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            //https://devblogs.microsoft.com/dotnet/whats-new-in-windows-forms-in-net-6-0-preview-5/#application-wide-default-font
            Application.SetDefaultFont(new Font(new FontFamily("Microsoft Sans Serif"), 8f));
            Application.SetCompatibleTextRenderingDefault(false);
            var task = PluginHandler.CreateCatalogs();
            if (!task.IsCompleted) task.Wait();
            if (!task.Result)
            {
                MessageBox.Show("The data access plugins are missing. Please check the Plugins folder",
                    "Plugins", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataAccessFactory.FuncGetDynamicDAL = PluginHandler.GetDynamicDAL;
            Application.Run(new MDIDBStudioLite(args));
        }
    }
}
