using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AdvancedQuery;
using xmlDbEditor;
using SmartQueryRunner;

namespace SmartQueryRunner
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
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.GetUpperBound(0) == -1)
            {
                Application.Run(new MDIAdvancedQuery());
            }
            else
            {
                xmlEditor startForm = new xmlEditor();
                startForm.OpenFile(args[0]);
                Application.Run(startForm);
            }
        }

    }
}