using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard
{
    public class ParserValueTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate NumberTemplate { get; set; }
        public DataTemplate DateTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate EnumTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is not KVPair pair) return StringTemplate;
            switch (pair.Value)
            {
                case short:
                case int:
                case long:
                case byte:
                    return NumberTemplate;
                case DateOnly:
                    return DateTemplate;
                case bool:
                    return BoolTemplate;
                case Enum:
                    return EnumTemplate;
            }
            return StringTemplate;
        }
    }
}
