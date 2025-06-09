using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RetailCorrector.Converters
{
    public class BoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = (string)parameter;
            var ne = p.StartsWith('!');
            var v = value.ToString();
            return (ne ? v != p[1..] : v == p)? Visibility.Visible:Visibility.Collapsed;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
