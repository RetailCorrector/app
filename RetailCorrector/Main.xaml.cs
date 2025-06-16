using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using RetailCorrector.Utils;

namespace RetailCorrector
{
    public partial class Main : Window
    {
        public Main()
        {
            CommandBindings.AddRange(Commands.Init());
            InitializeComponent();
        }

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        [NotifyUpdated] private Visibility _historyVisibility = Visibility.Visible;

        private void SwitchVisiblity(object? s, RoutedEventArgs args)
        {
            HistoryVisibility = ((MenuItem)s!).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
