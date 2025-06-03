using RetailCorrector.Cashier.Forms;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RetailCorrector.Cashier.ModuleSystem
{
    public static class ModuleCollection
    {
        public static ReadOnlyObservableCollection<Module> Modules { get; }
        private readonly static ObservableCollection<Module> _modules;
        private static ModuleLoadContext? ctx;

        static ModuleCollection()
        {
            _modules = [];
            Modules = new ReadOnlyObservableCollection<Module>(_modules);
        }

        public static async Task Load()
        {
            Program.Form.fiscalModules.Items.Clear();
            ctx = new ModuleLoadContext();
            if(!Directory.Exists(Pathes.Modules)) Directory.CreateDirectory(Pathes.Modules);
            foreach (var file in Directory.GetFiles(Pathes.Modules)) await Add(file);
        }

        private static async Task Add(string filepath)
        {
            var fs = File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None);
            var assembly = ctx!.LoadFromStream(fs);
            var guid = assembly.GetCustomAttribute<GuidAttribute>()?.Value;
            if (guid is null) return;
            var types = assembly.GetTypes();
            var type = types.FirstOrDefault(t => t.BaseType == typeof(AbstractFiscalModule));
            if (type is null) return;
            var module = (AbstractFiscalModule)Activator.CreateInstance(type)!;
            module.OnLog += WriteLog;
            module.OnNotify += Notify;
            await module.OnLoad(ctx);
            var name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title;
            _modules.Add(new Module(guid, name, module, fs));
            Program.Form.fiscalModules.Items.Add(name);
        }

        private static void Notify(string text) => MessageBox.Show(text);

        private static void WriteLog(bool error, string text, Exception? e)
        {
            Program.Form.status.Text = text;
            /*if (error)
                Log.Error(e, text);
            else
                Log.Information(e, text);*/
        }

        public static async Task Unload()
        {
            foreach (var mod in Modules)
            {
                await mod.EntryPoint!.OnUnload();
                mod.EntryPoint.OnNotify -= Notify;
                mod.EntryPoint.OnLog -= WriteLog;
                mod.EntryPoint = null;
            }
            ctx?.Unload();
            foreach (var mod in Modules)
            {
                mod.Stream!.Dispose();
                mod.Stream = null;
            }
            _modules.Clear();
            ctx = null;
        }
    }
}

