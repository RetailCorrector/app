using RetailCorrector.RegistryManager.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace RetailCorrector.RegistryManager
{
    public partial class ModuleManager : Window, INotifyPropertyChanged
    {
        public string CurrentRegistry
        {
            get => _currentRegistry;
            set
            {
                if (_currentRegistry == value) return;
                _currentRegistry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentRegistry)));
                UpdateModuleList();
            }
        }
        private string _currentRegistry;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ModuleInfo> Modules { get; } = [];

        public ModuleManager()
        {
            _currentRegistry = RegistryList.Registries.Count == 0 ?
                Links.DefaultRegistry : RegistryList.Registries[0];
            UpdateModuleList();
            InitializeComponent();
        }

        private async void UpdateModuleList()
        {
            Modules.Clear();
            var remote = await GetRemoteModules();
            var local = GetLocalModules();
            foreach(var mod in remote.Modules)
            {
                var idLocal = local.FindIndex(l => l.Id == mod.Id);
                LocalModule? currLocal = idLocal == -1 ? null : local[idLocal];
                if (currLocal is LocalModule m) local.Remove(m);
                Modules.Add(new ModuleInfo(mod, currLocal));
            }
            foreach (var mod in local)
                Modules.Add(new ModuleInfo(mod));
        }

        private async Task<RemoteModules> GetRemoteModules()
        {
            try
            {
                using var http = new HttpClient();
                if (!Uri.TryCreate(CurrentRegistry, UriKind.Absolute, out var uri))
                    return new RemoteModules { Modules = [] };
                using var req = new HttpRequestMessage(HttpMethod.Get, uri);
                req.Headers.Add("User-Agent", $"RetailCorrector/rm-{App.Version}");
                using var resp = await http.SendAsync(req);
                var content = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RemoteModules>(content);
            }
            catch
            {
                return new RemoteModules { Modules = [] };
            }
        }

        private List<LocalModule> GetLocalModules()
        {
            var files = Directory.GetFiles(Pathes.Modules);
            var arr = new List<LocalModule>();
            for (int i = 0; i < files.Length; i++)
                if(LoadModuleAssembly(files[i]) is LocalModule mod)
                    arr.Add(mod);
            return arr;
        }
        internal static LocalModule? LoadModuleAssembly(string path)
        {
            try
            {
                using var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                var ctx = new ModuleLoadContext();
                var assembly = ctx.LoadFromStream(fs);
                var res = new LocalModule(assembly, path);
                assembly = null;
                ctx.Unload();
                ctx = null;
                return res;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Не удалось загрузить {path.Split(Path.DirectorySeparatorChar)[^1]}...\nПодробнее в лог-файле!");
                Log.Error(e, $"Не удалось загрузить {path.Split(Path.DirectorySeparatorChar)[^1]}...");
                File.Delete(path);
                return null;
            }
        }

        public void ShowRegistryList(object? sender, RoutedEventArgs e)
        {
            var uri = _currentRegistry;
            new RegistryList().ShowDialog();
            if (RegistryList.Registries.Any(r => r == uri))
                CurrentRegistry = uri;
            else CurrentRegistry = RegistryList.Registries[0];
        }
    }
}
