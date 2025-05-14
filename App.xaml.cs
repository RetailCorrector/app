using Serilog;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Windows;
using RetailCorrector.Wizard.Repository;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
        public static readonly Lazy<RepoPackage[]> Repository = new(LoadRepository);
        public static readonly string Version = typeof(App).Assembly.GetName().Version!.ToString(2);

        static App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Version", Version)
                .WriteTo.File(
                    Path.Combine(AppContext.BaseDirectory, "logs", ".log"),
                    outputTemplate: "{Timestamp:HH:mm:ss.ttt zzz} [{Level:u3}] ({Verion}) {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    flushToDiskInterval: TimeSpan.FromMilliseconds(100))
                .CreateLogger();
        }

        private static RepoPackage[] LoadRepository()
        {
            var args = Environment.GetCommandLineArgs();
            var url = "https://raw.githubusercontent.com/ornaras/RetailCorrector.Repository/refs/heads/stable/repository.json";
            if (args.Length > 1) url = args[1];
            using var http = new HttpClient();
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var resp = http.Send(req);
            var content = resp.Content.ReadAsStringAsync().Result;
            return RepoPackage.Parse(JsonNode.Parse(content)!.AsArray());
        }
    }
}
