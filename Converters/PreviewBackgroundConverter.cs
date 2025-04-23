using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RetailCorrector.Wizard.Converters
{
    public class PreviewBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? Brushes.White : Brushes.Gray;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
