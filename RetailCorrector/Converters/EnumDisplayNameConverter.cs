using System.Globalization;
using System.Windows.Data;
using RetailCorrector.Utils;

namespace RetailCorrector.Wizard.Converters
{

    public class EnumDisplayNameConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var coll = EnumHelper.GetDisplayNames(value.GetType()) as KeyValuePair<Enum, string>[];
            return coll.First(c => c.Key.ToString() == ((Enum)value).ToString()).Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
