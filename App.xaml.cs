using Serilog;
using Serilog.Core;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
        public static Logger Logger { get; } = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                "logs\\.log", rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromMilliseconds(100))
            .CreateLogger();
        public static Lazy<RepoPackage[]> Repository = new(LoadRepository);
        public static string Version = typeof(App).Assembly.GetName().Version.ToString(2);

        private static RepoPackage[] LoadRepository()
        {
            var args = Environment.GetCommandLineArgs();
            var url = "https://raw.githubusercontent.com/ornaras/RCOfficialModules/refs/heads/stable/repository.json";
            if (args.Length > 1) url = args[1];
            using var http = new HttpClient();
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            using var resp = http.Send(req);
            var content = resp.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<RepoPackage[]>(content)!;
        }
    }
}
