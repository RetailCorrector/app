using Serilog;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace RetailCorrector.Wizard.ModuleSystem
{
    public static class ModuleCollection
    {
        public static ObservableCollection<Module> Modules { get; } = [];

        public static async Task Load()
        {
            if(!Directory.Exists(Pathes.Modules)) Directory.CreateDirectory(Pathes.Modules);
            foreach (var file in Directory.GetFiles(Pathes.Modules)) await Add(file);
        }

        public static async Task Add(string filepath)
        {
            var assembly = Assembly.LoadFile(filepath);
            var guid = assembly.GetCustomAttribute<GuidAttribute>()?.Value;
            if (guid is null) return;
            var types = assembly.GetTypes();
            var type = types.FirstOrDefault(t => t.BaseType == typeof(AbstractSourceModule));
            if (type is null) return;
            var module = (AbstractSourceModule)Activator.CreateInstance(type)!;
            module.OnLog += WriteLog;
            module.OnNotify += Notify;
            await module.OnLoad();
            Modules.Add(new Module
            {
                Guid = new Guid(guid),
                Name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title,
                EntryPoint = module,
            });
        }

        private static void Notify(string text) => MessageBox.Show(text);

        private static void WriteLog(bool error, string text, Exception? e)
        {
            if (error)
                Log.Error(e, text);
            else
                Log.Information(e, text);
        }
    }
}

