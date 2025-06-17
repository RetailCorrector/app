using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace RetailCorrector.Utils
{
    public class LoggerWindow : Window
    {
        public LoggerWindow()
        {
            WindowStyle = WindowStyle.ToolWindow;
            Width = 120 * 9;
            Height = 30 * 19;
            ResizeMode = ResizeMode.NoResize;
            Title = "Вывод консоли";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, new Binding { Source = App.LineTTY, 
                Path = new PropertyPath(nameof(App.LineTTY.Output)) });
            textBox.Background = Brushes.Black;
            textBox.Foreground = Brushes.White;
            textBox.IsReadOnly = true;
            textBox.FontSize = 12;
            textBox.TextWrapping = TextWrapping.WrapWithOverflow;
            textBox.FontFamily = new FontFamily("Cascadia Mono");
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            textBox.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            Content = textBox;
        }
    }
}
