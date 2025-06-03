using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.HistoryActions;
using RetailCorrector.Wizard.Managers;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailCorrector.Wizard.Windows
{
    public partial class Main : Window, INotifyPropertyChanged
    {
        public RoutedCommand ShowReport { get; } = new RoutedCommand(nameof(ShowReport), typeof(Main));
        public RoutedCommand ShowReceiptWizard { get; } = new RoutedCommand(nameof(ShowReceiptWizard), typeof(Main));
        public RoutedCommand ShowParser { get; } = new RoutedCommand(nameof(ShowParser), typeof(Main));
        public RoutedCommand ClearSpace { get; } = new RoutedCommand(nameof(ClearSpace), typeof(Main));
        public RoutedCommand Undo { get; } = new RoutedCommand(nameof(Undo), typeof(Main));
        public RoutedCommand Redo { get; } = new RoutedCommand(nameof(Redo), typeof(Main));
        public RoutedCommand Delete { get; } = new RoutedCommand(nameof(Delete), typeof(Main));
        public RoutedCommand InvertSelect { get; } = new RoutedCommand(nameof(InvertSelect), typeof(Main));
        public RoutedCommand InvertOperation { get; } = new RoutedCommand(nameof(InvertOperation), typeof(Main));
        public RoutedCommand Duplicate { get; } = new RoutedCommand(nameof(Duplicate), typeof(Main));
        public RoutedCommand LocalExport { get; } = new RoutedCommand(nameof(LocalExport), typeof(Main));

        public Main()
        {
            SetupHotKeys();
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

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
                    HistoryController.Add(new AddReceipts(wizard.Data));
            }));
            ClearSpace.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ClearSpace, (_, _) =>
            {
                WizardDataContext.Report = new RetailCorrector.Report();
                WizardDataContext.Receipts.Clear();
                HistoryController.Clear();
            }));
            Undo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(Undo, (_, _) => HistoryController.Undo()));
            Redo.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(Redo, (_, _) => HistoryController.Redo()));
            Delete.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(Delete, (_, _) => panel.Delete()));
            InvertSelect.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Control | ModifierKeys.Shift));
            CommandBindings.Add(new CommandBinding(InvertSelect, (_, _) => panel.InvertSelect()));
            InvertOperation.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Control | ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(InvertOperation, (_, _) => panel.InvertOperation()));
            Duplicate.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control | ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(Duplicate, (_, _) => panel.Duplicate()));
            LocalExport.InputGestures.Add(new KeyGesture(Key.B, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(LocalExport, DoLocalExport));
        }

        private void ShowLogs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true});

        private void ShowDocs(object? s, RoutedEventArgs e) =>
            Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true});

        private void ShowAbout(object? s, RoutedEventArgs args) => 
            new About().ShowDialog();

        private void RunModuleManager(object? s, RoutedEventArgs args) =>
            Process.Start(Path.Combine(Pathes.RegistryManager, "ModuleManager.exe"));

        private void RunLocalCashier(object? s, RoutedEventArgs args) =>
            Process.Start(Path.Combine(Pathes.Cashier, "Cashier.exe"));

        private void DoLocalExport(object? s, RoutedEventArgs args)
        {
            File.WriteAllText(Pathes.Report, JsonSerializer.Serialize(WizardDataContext.Report));
            foreach (var stack in WizardDataContext.Receipts.Chunk(25))
            {
                string filename;
                do
                {
                    filename = Path.Combine(Pathes.Receipts, $"{Path.GetRandomFileName().Replace(".", "")}.json");
                } while (File.Exists(filename));
                File.WriteAllText(filename, JsonSerializer.Serialize(stack));
            }
        }

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
