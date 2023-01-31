using System;
using System.Drawing;
using System.Windows.Forms;

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

            Application.Run(new MDIDBStudioLite(args));
        }

    }
}
