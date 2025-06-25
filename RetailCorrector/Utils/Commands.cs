using RetailCorrector.Cashier;
using RetailCorrector.Editor.Receipt;
using System.Diagnostics;
using System.Windows.Input;
using RetailCorrector.Plugin;
using RetailCorrector.Storage;
using RetailCorrector.Editor;

namespace RetailCorrector.Utils
{
    public static partial class Commands
    {
        public static CommandBinding[] Init()
        {
            SetupHotKeys();
            return [
                /*new CommandBinding(Undo, (_,_) => HistoryController.Undo()),
                new CommandBinding(Redo, (_,_) => HistoryController.Redo()),
                */
                new CommandBinding(AddReceipt, (_,_) => {
                    var wizard = new ReceiptWizard();
                    if (wizard.ShowDialog() == true)
                    {
                        StorageContext.Instance.Receipts.Add(wizard.Data);
                        StorageContext.Instance.SaveChanges();
                    }
                }),
                new CommandBinding(ParseReceipts, (_,_) => new Parser.Parser().ShowDialog()),

                new CommandBinding(OpenPluginManager, (_,_) => new AssemblyDownloader().ShowDialog()),
                new CommandBinding(OpenReportEditor, (_,_) => new ReportEditor.Report().ShowDialog()),
                new CommandBinding(OpenCashier, (_,_) => new CashierView().ShowDialog()),
                //new CommandBinding(OpenSettings, (_,_) => panel.Delete()),
                new CommandBinding(OpenDocs, (_,_) => Process.Start(new ProcessStartInfo(Links.Wiki) { UseShellExecute = true})),
                new CommandBinding(OpenConsole, (_,_) => new LoggerWindow().ShowDialog()),
                new CommandBinding(OpenLogDir, (_,_) => 
                    Process.Start(new ProcessStartInfo("explorer", Pathes.Logs) { UseShellExecute = true})),
                new CommandBinding(OpenAbout, (_,_) => new AboutWindow().ShowDialog()),

                new CommandBinding(Clear, (_,_) => {
                    Env.Report = new Report();
                    StorageContext.Instance.Dispose();
                    StorageContext.Init();
                    EditorView.Instance.Table = new();
                    EditorView.Instance.QueryText = "";
                }),
            ];
        }
    }
}