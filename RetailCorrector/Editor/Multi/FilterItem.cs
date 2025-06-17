using System.Collections.ObjectModel;

namespace RetailCorrector.Editor.Multi
{
    public partial class FilterItem(int number, string[] items)
    {
        public string[] ItemsSource { get; } = items;

        [NotifyUpdated] private int _number = number;
        [NotifyUpdated] private string _key = "";
        [NotifyUpdated] private string _operator = "";
        [NotifyUpdated] private object? _value = null;
    }
}