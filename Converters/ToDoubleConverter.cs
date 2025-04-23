using System.Globalization;
using System.Windows.Data;

namespace RetailCorrector.Wizard.Converters
{
    internal class ToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Math.Round((uint)value / Math.Pow(10, int.Parse((string)parameter)), int.Parse((string)parameter));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
