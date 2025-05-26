using System.Windows;
using System.Windows.Navigation;

namespace RetailCorrector.RegistryManager;

public partial class App : Application
{
    public static string Version => typeof(App).Assembly.GetName().Version!.ToString(3);

    protected override void OnLoadCompleted(NavigationEventArgs e)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Pathes.RegistryManagerLog,
                outputTemplate: Patterns.OutputLog,
                flushToDiskInterval: TimeSpan.FromMilliseconds(100))
            .CreateLogger();
        base.OnLoadCompleted(e);
    }
}
