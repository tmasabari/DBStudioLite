using System.Data;

namespace DBStudioLite.Model
{
    public interface IBaseData
    {
        DataTable SnippetsData { get; set; }
    }
}