using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Data;

namespace RetailCorrector.Converters
{
    public class SimpleCalcConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var expression = new StringBuilder($"{value}");
            if (long.TryParse((string)parameter, out _) ||
                double.TryParse((string)parameter, out _))
                expression.Append('+');
            expression.Append($"{parameter}");
            expression.Replace(',', '.');
            return new DataTable().Compute(expression.ToString(), null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
