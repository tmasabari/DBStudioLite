using System.Data;

namespace DBStudioLite.Model
{
    public class Snippets : IBaseData
    {
        public float Version { get; set; }

        public string UpdateURL { get; set; }

        public DataTable SnippetsData { get; set; }
    }
}
