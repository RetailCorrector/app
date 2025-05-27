using System.Diagnostics;
using System.Windows;

namespace RetailCorrector.Wizard.Windows
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        private void ShowDocs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true});

        private void ShowAbout(object? s, RoutedEventArgs args) => 
            new About().ShowDialog();

        private void ShowRegistryList(object? s, RoutedEventArgs args) => 
            new RegistryList().ShowDialog();
    }
}
