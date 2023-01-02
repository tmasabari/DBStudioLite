using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBStudioLite.Model
{
    public class Snippets : IBaseData
    {
        public float Version { get; set; }

        public string UpdateURL { get; set; }

        public DataTable SnippetsData { get; set; }
    }
}
