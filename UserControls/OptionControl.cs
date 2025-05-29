using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailCorrector.Wizard.UserControls
{
    public class OptionControl : StackPanel 
    {
        public OptionControl(string text)
        {
            Orientation = Orientation.Vertical;
            
            var textBlock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center
            };
            Children.Add(textBlock);
        }

        public void AddItem(UIElement element) =>
            Children.Add(element);

        public Border WrapBorder() => new()
        {
            BorderThickness = new Thickness(1),
            Padding = new Thickness(7),
            BorderBrush = Brushes.Black,
            CornerRadius = new CornerRadius(10),
            Width = 250,
            Child = this
        };
    }
}
