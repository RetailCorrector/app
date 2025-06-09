using System.Globalization;
using System.Windows.Data;

namespace RetailCorrector.Converters
{
    public class SimpleCalcConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = 0;
            if(value is double)
                v = (double)value;
            else if(value is uint)
                v = (uint)value / 1.0;
            var s = (string)parameter;
            if (s.StartsWith("*"))
            {

                s = s[1..];
                if (double.TryParse(s, out var p))
                    return v * p;
            }
            else if (s.StartsWith("/"))
            {
                s = s[1..];
                if (double.TryParse(s, out var p))
                    return v / p;
            }
            else
            {
                if (double.TryParse(s, out var p))
                    return v + p;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
