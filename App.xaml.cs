using Serilog;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
        public static readonly string Version = typeof(App).Assembly.GetName().Version!.ToString(3);
        public static string RepositoryUrl { get; set; } = 
            "https://raw.githubusercontent.com/ornaras/RetailCorrector.Repository/refs/heads/stable/repository.json";

        static App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Version", Version)
                .WriteTo.File(Pathes.WizardLog,
                    outputTemplate: Patterns.OutputLog,
                    flushToDiskInterval: TimeSpan.FromMilliseconds(100))
                .CreateLogger();
            ModuleCollection.Load().Wait();
        }
    }
}
