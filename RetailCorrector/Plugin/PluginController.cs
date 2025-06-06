using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Loader;
using RetailCorrector.Utils;

namespace RetailCorrector.Plugin;

public static class PluginController
{
    private static AssemblyLoadContext? ctx;
    private static readonly List<Assembly> assemblies = [];
    public static ReadOnlyCollection<Assembly> Assemblies => assemblies.AsReadOnly();

    public static ObservableCollection<SourcePlugin> SourcePlugins { get; } = [];
    public static ObservableCollection<FiscalPlugin> FiscalPlugins { get; } = [];

    public static void Load()
    {
        ctx = new AssemblyLoadContext(null, true);
        foreach (var file in Directory.GetFiles(Pathes.Plugins))
        {
            var _stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                assemblies.Add(new Assembly(_stream, ctx));
            }
            catch(Exception ex)
            {
                _stream.Dispose();
                AlertHelper.ErrorAlert(ex, $"Не удалось загрузить сборку {Path.GetFileNameWithoutExtension(file)}");
            }
        }
        foreach (var plugin in assemblies.SelectMany(SearchPlugins<SourcePlugin>))
            SourcePlugins.Add(plugin);
        foreach (var plugin in assemblies.SelectMany(SearchPlugins<FiscalPlugin>))
            FiscalPlugins.Add(plugin);
    }
    
    public static void Unload()
    {
        FiscalPlugins.Clear();
        SourcePlugins.Clear();
        assemblies.ForEach(a => a.Dispose());
        assemblies.Clear();
        ctx?.Unload();
        ctx = null;
    }

    private static IEnumerable<TPlugin> SearchPlugins<TPlugin>(Assembly assembly) where TPlugin: IPlugin =>
        assembly.Plugins.Where(p => typeof(TPlugin) == p.GetType().BaseType).Cast<TPlugin>();
}