namespace RetailCorrector.ReportEditor
{
    public partial class StringsPair(string key = "", string value = "")
    {
        [NotifyChanged] private string _key = key;
        [NotifyChanged] private string _value = value;
    }
}