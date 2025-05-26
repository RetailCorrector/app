using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Loader;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.RegistryManager
{
    public partial class ModuleInfo : UserControl, INotifyPropertyChanged
    {
        public Guid Id { get; } = new Guid();
        public string Title { get; } = "";
        public string Description { get; } = "";
        public string Url { get; } = "";
        public string Hash { get; } = "";
        public Version Version { get; } = new();
        public string LocalVersion =>
            Local is null ? "" : ((LocalModule)Local!).Version.ToString(3);
        public LocalModule? Local
        {
            get => _local;
            set
            {
                _local = value;
                OnPropertyChanged(nameof(Local));
                OnPropertyChanged(nameof(IsInstalled));
                OnPropertyChanged(nameof(LocalVersion));
            }
        }
        private LocalModule? _local = null;

        public bool IsInstalled => Local is not null;

        public ModuleInfo()
        {
            LoadLocal();
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private void Delete(object? s, RoutedEventArgs e)
        {
            var path = Path.Combine(Pathes.Modules, $"{((LocalModule)Local!).Id}.dll");
            File.Delete(path);
            Local = null;
        }

        private async void Download(object? s, RoutedEventArgs e)
        {
            using var http = new HttpClient();
            using var resp = await http.GetAsync(Url);
            var cont = await resp.Content.ReadAsByteArrayAsync();
            var path = Path.Combine(Pathes.Modules, $"{Id}.dll");
            File.WriteAllBytes(path, cont);
            LoadLocal();
        }

        private void LoadLocal()
        {
            var path = Path.Combine(Pathes.Modules, $"{Id}.dll");
            if (!File.Exists(path)) return;
            var data = File.ReadAllBytes(path);
            using var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var ctx = new AssemblyLoadContext(Id.ToString());
            var assembly = ctx.LoadFromStream(fs);
            Local = new LocalModule(assembly, data);
            assembly = null;
            ctx.Unload();
            ctx = null;
        }
    }
}
