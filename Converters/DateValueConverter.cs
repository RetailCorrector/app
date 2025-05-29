using System.Globalization;
using System.Windows.Data;

namespace RetailCorrector.Wizard.Converters
{
    public class DateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly d)
                return d.ToDateTime(TimeOnly.MinValue);
            throw new Exception();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
                return DateOnly.FromDateTime(dt);
            throw new Exception();
        }
    }
}
