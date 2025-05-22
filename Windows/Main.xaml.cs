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
            Process.Start(new ProcessStartInfo("explorer", DirPath.LogsDir) { UseShellExecute = true});

        private void ShowDocs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("https://retailcorrector.gitbook.io/wiki") { UseShellExecute = true});

        private void ShowAbout(object? s, RoutedEventArgs args) => 
            new About().ShowDialog();
    }
}
