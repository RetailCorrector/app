using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using RetailCorrector.Utils;

namespace RetailCorrector
{
    public partial class Main : Window, INotifyPropertyChanged
    {
        public Main()
        {
            CommandBindings.AddRange(Commands.Init());
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        public Visibility HistoryVisibility
        {
            get => _historyVisibility;
            set
            {
                _historyVisibility = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HistoryVisibility)));
            }
        }
        private Visibility _historyVisibility = Visibility.Visible;

        private void SwitchVisiblity(object? s, RoutedEventArgs args)
        {
            HistoryVisibility = ((MenuItem)s!).IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
