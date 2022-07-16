using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedQueryOrganizer
{
    public enum FormTypes { SQLEditor, JsonEditor, XMLEditor}

    internal interface IOperatingForm
    {
        int FillColumn { get; set; }

        void LoadFromFile();
        void SaveToFile();
        string FileName { get; set; }
    }
}
