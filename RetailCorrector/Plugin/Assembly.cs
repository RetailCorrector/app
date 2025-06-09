using System.IO;
using System.Runtime.Loader;
using System.Windows;

namespace RetailCorrector.Plugin;

public class Assembly: IDisposable
{
    public AssemblyInfo? Info { get; set; }
    public IPlugin?[] Plugins { get; set; }
    public FileStream? Stream { get; set; }

    public Assembly(FileStream stream, AssemblyLoadContext ctx)
    {
        Stream = stream;
        var assembly = ctx.LoadFromStream(Stream);
        Info = new AssemblyInfo(assembly);
        var types = assembly.GetTypes().Where(t => t.BaseType?.GetInterfaces().Contains(typeof(IPlugin)) ?? false);
        Plugins = [..types.Select(t => (IPlugin)Activator.CreateInstance(t)!)];
        foreach (var plugin in Plugins)
        {
            plugin!.Notification += Notify;
            plugin.Logging += Logging;
            plugin.OnLoad(ctx).Wait();
        }
    }

    private void Notify(string text, string? caption) =>
        MessageBox.Show(text, caption ?? "");

    private void Logging(string text, bool error, Exception? ex)
    {
        if(error) Log.Logger.Error(ex, text);
        else Log.Logger.Information(ex, text);
    }

    public void Dispose()
    {
        Info = null;
        for (var i = 0; i < Plugins.Length; i++)
        {
            Plugins[i]!.OnUnload().Wait();
            Plugins[i]!.Notification -= Notify;
            Plugins[i]!.Logging -= Logging;
            Plugins[i] = null;
        }
        Stream!.Dispose();
        Stream = null;
        GC.WaitForPendingFinalizers();
    }
}