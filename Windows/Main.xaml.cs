using RetailCorrector.Wizard.Contexts;
using System.Diagnostics;
using System.Windows;

namespace RetailCorrector.Wizard.Windows
{
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

        private void ShowReport(object? s, RoutedEventArgs args) =>
            new Report().ShowDialog();

        private void ShowParser(object s, RoutedEventArgs args) =>
            new Parser().ShowDialog();

        private void ShowReceiptWizard(object s, RoutedEventArgs args) =>
            new ReceiptWizard().ShowDialog();

        private void ClearSpace(object s, RoutedEventArgs args)
        {
            WizardDataContext.Report = new RetailCorrector.Report();
            WizardDataContext.Receipts.Clear();
        }
    }
}
