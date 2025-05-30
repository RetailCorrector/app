using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.HistoryActions;
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
        public RoutedCommand Undo { get; } = new RoutedCommand(nameof(Undo), typeof(Main));

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
            CommandBindings.Add(new CommandBinding(ShowReceiptWizard, (_, _) =>
            {
                var wizard = new ReceiptWizard();
                if (wizard.ShowDialog() == true)
                {
                    var index = WizardDataContext.Receipts.Count;
                    WizardDataContext.Receipts.Add(wizard.Data);
                    WizardDataContext.History.Push(new AddReceipts(index, 1));
                }
            }));
            ClearSpace.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ClearSpace, (_, _) =>
            {
                WizardDataContext.Report = new RetailCorrector.Report();
                WizardDataContext.Receipts.Clear();
                WizardDataContext.History.Clear();
            }));
            Undo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(Undo, (_, _) =>
            {
                if (WizardDataContext.History.Count > 0)
                {
                    var action = WizardDataContext.History.Pop();
                    action.Undo();
                }
            }));
        }

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        private void ShowDocs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true});

        private void ShowAbout(object? s, RoutedEventArgs args) => 
            new About().ShowDialog();

        private void RunModuleManager(object? s, RoutedEventArgs args)
        {
            var path = Path.Combine(Pathes.RegistryManager, "ModuleManager.exe");
            Process.Start(path);
        }
    }
}
