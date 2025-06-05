using RetailCorrector.ModuleManager.Data;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.ModuleManager
{
    public partial class ModuleInfo : UserControl, INotifyPropertyChanged
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Url { get; init; }
        public Version Version { get; init; }
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
        private LocalModule? _local;

        public bool IsInstalled => Local is not null;

        public ModuleInfo(LocalModule local)
        {
            Id = local.Id;
            Title = local.Name;
            Version = new Version();
            Description = "";
            Url = "";
            Local = local;
            InitializeComponent();
        }

        public ModuleInfo(RemoteModule remote, LocalModule? local)
        {
            Id = remote.Id;
            Title = remote.Name;
            Version = remote.Version;
            Description = remote.Description;
            Url = remote.File;
            Local = local;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private void Delete(object? s, RoutedEventArgs e)
        {
            File.Delete(((LocalModule)Local!).Path);
            Local = null;
        }

        private async void Download(object? s, RoutedEventArgs e)
        {
            using var http = new HttpClient();
            using var resp = await http.GetAsync(Url);
            var cont = await resp.Content.ReadAsByteArrayAsync();
            var path = Path.Combine(Pathes.Modules, Url.Split('/')[^1]);
            File.WriteAllBytes(path, cont);
            if (!File.Exists(path)) return;
            Local = ModuleManager.LoadModuleAssembly(path);
        }
    }
}
