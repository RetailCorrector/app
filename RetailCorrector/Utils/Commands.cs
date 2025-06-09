using RetailCorrector.Cashier;
using RetailCorrector.Editor.Receipt;
using RetailCorrector.History;
using RetailCorrector.History.Actions;
using System.Diagnostics;
using System.Windows.Input;
using RetailCorrector.Plugin;

namespace RetailCorrector.Utils
{
    public static class Commands
    {
        public static RoutedCommand Undo { get; } = new(nameof(Undo), typeof(Commands));
        public static RoutedCommand Redo { get; } = new(nameof(Redo), typeof(Commands));

        public static RoutedCommand AddReceipt { get; } = new(nameof(AddReceipt), typeof(Commands));
        public static RoutedCommand ParseReceipts { get; } = new(nameof(ParseReceipts), typeof(Commands));
        public static RoutedCommand PasteReceipt { get; } = new(nameof(PasteReceipt), typeof(Commands));
        public static RoutedCommand DuplicateReceipts { get; } = new(nameof(DuplicateReceipts), typeof(Commands));
        public static RoutedCommand RemoveReceipts { get; } = new(nameof(RemoveReceipts), typeof(Commands));

        public static RoutedCommand OpenPluginManager { get; } = new(nameof(OpenPluginManager), typeof(Commands));
        public static RoutedCommand OpenReportEditor { get; } = new(nameof(OpenReportEditor), typeof(Commands));
        public static RoutedCommand OpenCashier { get; } = new(nameof(OpenCashier), typeof(Commands));
        public static RoutedCommand OpenSettings { get; } = new(nameof(OpenSettings), typeof(Commands));
        public static RoutedCommand OpenConsole { get; } = new(nameof(OpenConsole), typeof(Commands));
        public static RoutedCommand OpenAbout { get; } = new(nameof(OpenAbout), typeof(Commands));
        public static RoutedCommand OpenDocs { get; } = new(nameof(OpenDocs), typeof(Commands));

        public static RoutedCommand Clear { get; } = new(nameof(Clear), typeof(Commands));

        public static RoutedCommand InvertSelection { get; } = new(nameof(InvertSelection), typeof(Commands));

        public static RoutedCommand InvertOperation { get; } = new(nameof(InvertOperation), typeof(Commands));

        public static RoutedCommand ExitDialog { get; } = new(nameof(ExitDialog), typeof(Commands));

        public static CommandBinding[] Init()
        {
            SetupHotKeys();
            return [
                new CommandBinding(Undo, (_,_) => HistoryController.Undo()),
                new CommandBinding(Redo, (_,_) => HistoryController.Redo()),

                new CommandBinding(AddReceipt, (_,_) => {
                    var wizard = new ReceiptWizard();
                    if (wizard.ShowDialog() == true)
                        HistoryController.Add(new AddReceipts(wizard.Data));
                }),
                new CommandBinding(ParseReceipts, (_,_) => new Parser.Parser().ShowDialog()),
                //new CommandBinding(PasteReceipt, (_,_) => HistoryController.Redo()),
                new CommandBinding(DuplicateReceipts, (_,_) => ReceiptPanel.Duplicate()),
                new CommandBinding(RemoveReceipts, (_,_) => ReceiptPanel.Delete()),

                new CommandBinding(OpenPluginManager, (_,_) => new AssemblyDownloader().ShowDialog()),
                new CommandBinding(OpenReportEditor, (_,_) => new Editor.Report.Report().ShowDialog()),
                new CommandBinding(OpenCashier, (_,_) => new CashierView().ShowDialog()),
                //new CommandBinding(OpenSettings, (_,_) => panel.Delete()),
                new CommandBinding(OpenDocs, (_,_) => Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true})),
                //new CommandBinding(OpenConsole, (_,_) => panel.Delete()),
                new CommandBinding(OpenAbout, (_,_) => new AboutWindow().ShowDialog()),

                new CommandBinding(Clear, (_,_) => {
                    Env.Report = new Report();
                    Env.Receipts.Clear();
                    HistoryController.Clear();
                }),

                new CommandBinding(InvertSelection, (_,_) => ReceiptPanel.InvertSelect()),

                new CommandBinding(InvertOperation, (_,_) => ReceiptPanel.InvertOperation()),
            ];
        }

        private static void SetupHotKeys()
        {
            Undo.InputGestures.Add(HotKeys.Undo);
            Redo.InputGestures.Add(HotKeys.Redo);

            AddReceipt.InputGestures.Add(HotKeys.AddReceipt);
            ParseReceipts.InputGestures.Add(HotKeys.ParseReceipts);
            PasteReceipt.InputGestures.Add(HotKeys.PasteReceipt);
            DuplicateReceipts.InputGestures.Add(HotKeys.DuplicateReceipts);
            RemoveReceipts.InputGestures.Add(HotKeys.RemoveReceipts);

            OpenPluginManager.InputGestures.Add(HotKeys.OpenPluginManager);
            OpenReportEditor.InputGestures.Add(HotKeys.OpenReportEditor);
            OpenCashier.InputGestures.Add(HotKeys.OpenCashier);
            OpenSettings.InputGestures.Add(HotKeys.OpenSettings);
            OpenDocs.InputGestures.Add(HotKeys.OpenDocs);
            OpenConsole.InputGestures.Add(HotKeys.OpenConsole);
            OpenAbout.InputGestures.Add(HotKeys.OpenAbout);

            Clear.InputGestures.Add(HotKeys.Clear);

            InvertSelection.InputGestures.Add(HotKeys.InvertSelection);

            InvertOperation.InputGestures.Add(HotKeys.InvertOperation);

            ExitDialog.InputGestures.Add(HotKeys.ExitDialog);
        }
    }
}
