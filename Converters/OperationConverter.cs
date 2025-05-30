using System.Globalization;

namespace RetailCorrector.Wizard.Converters
{
    public class OperationConverter : EnumDisplayNameConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = ((string)base.Convert(value, targetType, parameter, culture)).ToLower();
            return $"Чек {text}{(text.EndsWith('а') ? "" : "а")}";
        }
    }
}
