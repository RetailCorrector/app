using System.Windows;

namespace RetailCorrector
{
    public partial class App : Application
    {
        public static readonly string Version = typeof(App).Assembly.GetName().Version!.ToString(3);

        static App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("Version", Version)
                .WriteTo.File(Pathes.Log,
                    outputTemplate: Patterns.OutputLog,
                    rollingInterval: RollingInterval.Day,
                    flushToDiskInterval: TimeSpan.FromMilliseconds(100))
                .CreateLogger();
        }
    }
}
