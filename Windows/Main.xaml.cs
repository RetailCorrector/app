using RetailCorrector.Wizard.Contexts;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace RetailCorrector.Wizard.Windows
{
    public partial class Main : Window
    {
        public RoutedCommand ShowReport { get; } = new RoutedCommand(nameof(ShowReport), typeof(Main));
        public RoutedCommand ShowReceiptWizard { get; } = new RoutedCommand(nameof(ShowReceiptWizard), typeof(Main));
        public RoutedCommand ShowParser { get; } = new RoutedCommand(nameof(ShowParser), typeof(Main));
        public RoutedCommand ClearSpace { get; } = new RoutedCommand(nameof(ClearSpace), typeof(Main));

        public Main()
        {
            SetupHotKeys();
            InitializeComponent();
        }

        private void SetupHotKeys()
        {
            ShowReport.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ShowReport, (_, _) => new Report().ShowDialog()));
            ShowParser.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ShowParser, (_, _) => new Parser().ShowDialog()));
            ShowReceiptWizard.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(ShowReceiptWizard, (_, _) => new ReceiptWizard().ShowDialog()));
            ClearSpace.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ClearSpace, (_, _) =>
            {
                WizardDataContext.Report = new RetailCorrector.Report();
                WizardDataContext.Receipts.Clear();
            }));
        }

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        private void ShowDocs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true});

        private void ShowAbout(object? s, RoutedEventArgs args) => 
            new About().ShowDialog();
    }
}
