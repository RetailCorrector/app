namespace RetailCorrector.Editor.Multi
{
    public partial class EditorItem(string[] items)
    {
        public string[] ItemsSource { get; } = items;
        [NotifyUpdated] private string _key;
        [NotifyUpdated] private string _value;
    }
}
