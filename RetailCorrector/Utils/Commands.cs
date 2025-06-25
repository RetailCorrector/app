using RetailCorrector.Cashier;
using RetailCorrector.Editor.Receipt;
using RetailCorrector.History;
using RetailCorrector.History.Actions;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using RetailCorrector.Plugin;

namespace RetailCorrector.Utils
{
    public static partial class Commands
    {
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
                //new CommandBinding(DuplicateReceipts, (_,_) => ReceiptPanel.Duplicate()),
                //new CommandBinding(RemoveReceipts, (_,_) => ReceiptPanel.Delete()),

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
                    Env.Receipts.Clear();
                    HistoryController.Clear();
                }),

                //new CommandBinding(InvertSelection, (_,_) => ReceiptPanel.InvertSelect()),

//                new CommandBinding(InvertOperation, (_,_) => ReceiptPanel.InvertOperation()),
                new CommandBinding(MultiEditor, (_,_) => {
                    var path = Path.Combine(Path.GetTempPath(), "RetailCorrectorMultiEdit.sql");
                    if(File.Exists(path)) File.Delete(path);
                    Process.Start("notepad",path)!.WaitForExit();
                    if(!File.Exists(path)) return;
                    var query = File.ReadAllText(path);
                    if(string.IsNullOrWhiteSpace(query)) return;
                    HistoryController.Add(new MultiEditReceipts(query));
                    File.Delete(path);
                }),
            ];
        }
    }
}