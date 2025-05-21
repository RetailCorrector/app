using Serilog;
using System.Buffers.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public static class ModuleCollection
    {
        public static ObservableCollection<LocalModule> Modules { get; } = [];

        public static async Task Load()
        {
            var dir = Path.Combine(AppContext.BaseDirectory, "sources");
            if(!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            foreach (var file in Directory.GetFiles(dir)) await Add(file);
        }

        public static string Remove(string guid)
        {
            var mod = Modules.FirstOrDefault(m => m.Guid == guid);
            if (mod is null) return "";
            var path = mod.FilePath;
            mod.EntryPoint!.OnNotify -= Notify;
            mod.EntryPoint.OnLog -= WriteLog;
            Modules.Remove(mod);
            mod.Dispose();
            mod = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return path;
        }

        public static async Task Add(string filepath)
        {
            var fs = File.OpenRead(filepath);
            var ctx = new ModuleLoadContext();
            var assembly = ctx.LoadFromStream(fs);
            var guid = assembly.GetCustomAttribute<GuidAttribute>()?.Value;
            if (guid is null) return;
            var types = assembly.GetTypes();
            var type = types.FirstOrDefault(t => t.BaseType == typeof(AbstractSourceModule));
            if (type is null) return;
            var module = (AbstractSourceModule)Activator.CreateInstance(type)!;
            module.OnLog += WriteLog;
            module.OnNotify += Notify;
            await module.OnLoad();
            Modules.Add(new LocalModule
            {
                LoadContext = ctx,
                Guid = guid,
                Name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()!.Title,
                HashSum = GenerateHash(filepath),
                EntryPoint = module,
                Stream = fs,
                FilePath = filepath,
                Version = Version.Parse(assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()!.Version)
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

        private static string GenerateHash(string filepath)
        {
            var hash = SHA512.HashData(File.ReadAllBytes(filepath));
            var buffer = new byte[88];
            Base64.EncodeToUtf8(hash, buffer, out _, out _);
            return Encoding.UTF8.GetString(buffer);
        }

        public static void Unload()
        {
            foreach (var module in Modules)
                module.Dispose();
            Modules.Clear();
        }
    }
}

