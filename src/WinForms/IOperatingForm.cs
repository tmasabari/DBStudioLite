namespace DBStudioLite
{
    public enum FormTypes { SQLEditor, JsonEditor, XMLEditor }

    internal interface IOperatingForm
    {
        int FillColumn { get; set; }

        void LoadFromFile();
        void SaveToFile();
        string FileName { get; set; }
    }
}
