namespace RetailCorrector.Editor.Receipt.ViewModels
{
    public partial class IndustryViewModel
    {
        [NotifyChanged] private byte _gosId;
        [NotifyChanged] private DateTime _date;
        [NotifyChanged] private int _number;
        [NotifyChanged] private string _value = "";

        public static implicit operator IndustryViewModel(IndustryData ro) => new()
        {
            Date = ro.Date.ToDateTime(TimeOnly.MinValue),
            GosId = ro.GosId,
            Number = ro.Number,
            Value = ro.Value,
        };

        public static implicit operator IndustryData(IndustryViewModel rw) => new()
        {
            Date = DateOnly.FromDateTime(rw.Date),
            GosId = rw.GosId,
            Number = rw.Number,
            Value = rw.Value,
        };
    }
}
