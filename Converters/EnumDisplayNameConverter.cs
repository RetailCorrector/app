using RetailCorrector.Wizard.Extensions;
using System.Globalization;
using System.Windows.Data;

namespace RetailCorrector.Wizard.Converters
{

    public class EnumDisplayNameConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coll = EnumExtensions.GetDisplayNames(value.GetType()) as KeyValuePair<Enum, string>[];
            return coll.First(c => c.Key == value).Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
