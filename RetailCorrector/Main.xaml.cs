using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RetailCorrector.Cashier;
using RetailCorrector.Editor.Receipt;
using RetailCorrector.History;
using RetailCorrector.History.Actions;
using RetailCorrector.Utils;

namespace RetailCorrector
{
    public partial class Main : Window, INotifyPropertyChanged
    {
        public RoutedCommand ShowReport { get; } = new(nameof(ShowReport), typeof(Main));
        public RoutedCommand ShowReceiptWizard { get; } = new(nameof(ShowReceiptWizard), typeof(Main));
        public RoutedCommand ShowParser { get; } = new(nameof(ShowParser), typeof(Main));
        public RoutedCommand ClearSpace { get; } = new(nameof(ClearSpace), typeof(Main));
        public RoutedCommand Undo { get; } = new(nameof(Undo), typeof(Main));
        public RoutedCommand Redo { get; } = new(nameof(Redo), typeof(Main));
        public RoutedCommand Delete { get; } = new(nameof(Delete), typeof(Main));
        public RoutedCommand InvertSelect { get; } = new(nameof(InvertSelect), typeof(Main));
        public RoutedCommand InvertOperation { get; } = new(nameof(InvertOperation), typeof(Main));
        public RoutedCommand Duplicate { get; } = new(nameof(Duplicate), typeof(Main));
        public RoutedCommand LocalExport { get; } = new(nameof(LocalExport), typeof(Main));

        public Main()
        {
            SetupHotKeys();
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SetupHotKeys()
        {
            ShowReport.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ShowReport, (_, _) => new Editor.Report.Report().ShowDialog()));
            ShowParser.InputGestures.Add(new KeyGesture(Key.P, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(ShowParser, (_, _) => new Parser.Parser().ShowDialog()));
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
                Env.Report = new Report();
                Env.Receipts.Clear();
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
            new AboutWindow().ShowDialog();
/*
        private void RunModuleManager(object? s, RoutedEventArgs args) =>
            Process.Start(Path.Combine(Pathes.RegistryManager, "ModuleManager.exe"));*/

        private void RunLocalCashier(object? s, RoutedEventArgs args) =>
            new CashierView().ShowDialog();
            //Process.Start(Path.Combine(Pathes.Cashier, "Cashier.exe"));

        private void DoLocalExport(object? s, RoutedEventArgs args)
        {
            File.WriteAllText(Pathes.Report, JsonSerializer.Serialize(Env.Report));
            foreach (var stack in Env.Receipts.Chunk(25))
            {
                string filename;
                do
                {
                    filename = Path.Combine(Pathes.Receipts, $"{Path.GetRandomFileName().Replace(".", "")}.json");
                } while (File.Exists(filename));
                File.WriteAllText(filename, JsonSerializer.Serialize(stack));
            }
            AlertHelper.Alert("Экспорт чеков для локального отбития завершён!");
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
