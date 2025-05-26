using System.Windows;

namespace RetailCorrector.RegistryManager;

public partial class App : Application
{
    public static string Version => typeof(App).Assembly.GetName().Version!.ToString(3);

    public App()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithProperty("Version", Version)
            .WriteTo.File(Pathes.RegistryManagerLog,
                outputTemplate: Patterns.OutputLog,
                flushToDiskInterval: TimeSpan.FromMilliseconds(100))
            .CreateLogger();
    }
}
