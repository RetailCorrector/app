using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using RetailCorrector.Wizard;

namespace RetailCorrector.PluginSystem
{
    public partial class PluginManager : Window, INotifyPropertyChanged
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

        public ObservableCollection<PluginInfo> Modules { get; } = [];

        public PluginManager()
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
            foreach(var mod in remote.Plugins)
            {
                //var idLocal = local.FindIndex(l => l.Id == mod.Id);
                //LocalPluginInfo? currLocal = idLocal == -1 ? null : local[idLocal];
                //if (currLocal is LocalPluginInfo m) local.Remove(m);
                //Modules.Add(new PluginInfo(mod, currLocal));
            }
            foreach (var mod in local)
                Modules.Add(new PluginInfo(mod));
        }

        private async Task<RemotePlugins> GetRemoteModules()
        {
            try
            {
                using var http = new HttpClient();
                if (!Uri.TryCreate(CurrentRegistry, UriKind.Absolute, out var uri))
                    return new RemotePlugins { Plugins = [] };
                using var req = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, uri);
                req.Headers.Add("User-Agent", $"RetailCorrector/rm-{App.Version}");
                using var resp = await http.SendAsync(req);
                var content = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RemotePlugins>(content);
            }
            catch
            {
                return new RemotePlugins { Plugins = [] };
            }
        }

        private List<LocalPluginInfo> GetLocalModules()
        {
            var files = Directory.GetFiles(Pathes.Modules);
            var arr = new List<LocalPluginInfo>();
            for (int i = 0; i < files.Length; i++)
                if(LoadPluginAssembly(files[i]) is LocalPluginInfo mod)
                    arr.Add(mod);
            return arr;
        }
        internal static LocalPluginInfo? LoadPluginAssembly(string path)
        {
            try
            {
                return new LocalPluginInfo(path);
            }
            catch(Exception e)
            {
                MessageBox.Show($"Не удалось получить информацию модуля {Path.GetFileNameWithoutExtension(path)}...");
                Log.Error(e, $"Не удалось получить информацию модуля {Path.GetFileNameWithoutExtension(path)}...");
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
