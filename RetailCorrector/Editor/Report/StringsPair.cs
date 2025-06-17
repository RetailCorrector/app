namespace RetailCorrector.Editor.Report
{
    public partial class StringsPair(string key = "", string value = "")
    {
        [NotifyUpdated] private string _key = key;
        [NotifyUpdated] private string _value = value;
    }
}