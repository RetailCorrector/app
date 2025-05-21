using Serilog;
using System.IO;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
        public static readonly string Version = typeof(App).Assembly.GetName().Version!.ToString(2);
        public static string RepositoryUrl { get; set; } = 
            "https://raw.githubusercontent.com/ornaras/RetailCorrector.Repository/refs/heads/stable/repository.json";

        static App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Version", Version)
                .WriteTo.File(
                    Path.Combine(AppContext.BaseDirectory, "logs", ".log"),
                    outputTemplate: "{Timestamp:HH:mm:ss.ttt zzz} [{Level:u3}] ({Version}) {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    flushToDiskInterval: TimeSpan.FromMilliseconds(100))
                .CreateLogger();
            ModuleCollection.Load().Wait();
        }
    }
}
