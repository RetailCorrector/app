using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows;
using RetailCorrector.Plugin;

namespace RetailCorrector.PluginSystem
{
    public static class PluginCollection
    {
        public static ReadOnlyObservableCollection<Plugin> Plugins { get; }
        private readonly static ObservableCollection<Plugin> _plugins;
        private static AssemblyLoadContext? ctx;

        static PluginCollection()
        {
            _plugins = [];
            Plugins = new ReadOnlyObservableCollection<Plugin>(_plugins);
        }

        public static async Task Load()
        {
            ctx = new AssemblyLoadContext(null, true);
            if(!Directory.Exists(Pathes.Modules)) Directory.CreateDirectory(Pathes.Modules);
            foreach (var file in Directory.GetFiles(Pathes.Modules)) await Add(file);
        }

        private static async Task Add(string filepath)
        {
            try
            {
                Log.Information("Подключение модуля: {file}...", Path.GetFileName(filepath));
                var fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None);
                var assembly = ctx!.LoadFromStream(fs);
                var types = assembly.GetTypes();
                var type = types.FirstOrDefault(t => t.BaseType == typeof(SourcePlugin));
                if (type is null) return;
                var module = (SourcePlugin)Activator.CreateInstance(type)!;
                module.Logging += WriteLog;
                module.Notification += Notify;
                await module.OnLoad(ctx);
                var name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
                var infoName = Patterns.AssemblyNameRegex().Match(name);
                _plugins.Add(new Plugin(infoName.Groups["name"].Value, module, fs));
            }
            catch(Exception e)
            {
                Log.Error(e, "Не удалось подключить модуль!");
                return;
            }
        }

        private static void Notify(string text, string? caption = null) => MessageBox.Show(text, caption ?? "");

        private static void WriteLog(string text, bool error, Exception? e)
        {
            if (error)
                Log.Error(e, text);
            else
                Log.Information(e, text);
        }

        public static async Task Unload()
        {
            foreach (var mod in Plugins)
            {
                await mod.EntryPoint!.OnUnload();
                mod.EntryPoint.Notification -= Notify;
                mod.EntryPoint.Logging -= WriteLog;
                mod.EntryPoint = null;
                Log.Information("Отключение модуля {name}", Path.GetFileName(mod.Stream!.Name));
            }
            ctx?.Unload();
            foreach (var mod in Plugins)
            {
                mod.Stream!.Dispose();
                mod.Stream = null;
            }
            _plugins.Clear();
            ctx = null;
        }
    }
}

