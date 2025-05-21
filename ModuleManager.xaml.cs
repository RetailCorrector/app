using RetailCorrector.Wizard.Repository;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class ModuleManager : Window, INotifyPropertyChanged
    {
        public string Url
        {
            get => App.RepositoryUrl;
            set
            {
                App.RepositoryUrl = value;
                UpdateSources();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Url)));
            }
        }
        public string[] Repositories { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ModuleManager()
        {
            LoadRepositories();
            InitializeComponent();
            UpdateSources();
        }

        public void UpdateSources()
        {
            panel.Children.Clear();
            if (string.IsNullOrWhiteSpace(Url)) return;
            using var http = new HttpClient();
            using var resp = http.GetAsync(Url).Result;
            if (!resp.IsSuccessStatusCode) return;
            var content = resp.Content.ReadAsStringAsync().Result;
            var json = JsonNode.Parse(content);
            if (json is null) return;
            var arr = RepoPackage.Parse(json!.AsArray());
            foreach (var package in arr)
                if (package is not FiscalPackage)
                    panel.Children.Add(new ModuleInfo(package));
        }

        private void LoadRepositories()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "repositories.txt");
            if (!File.Exists(path))
                File.WriteAllLines(path, ["https://raw.githubusercontent.com/ornaras/RetailCorrector.Repository/refs/heads/stable/repository.json"]);
            Repositories = File.ReadAllLines(path);
        }
    }
}
