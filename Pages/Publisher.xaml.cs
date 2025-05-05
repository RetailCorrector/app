using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Publisher : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<RepoPackage> Packages { get; } = [];
        public RepoPackage CurrentPackage
        {
            get => _current;
            set
            {
                if (_current == value) return;
                _current = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TipText));
            }
        }
        private RepoPackage _current;

        private string[] pastebinIds;

        public string ScriptText
        {
            get => _script;
            private set
            {
                if (_script == value) return;
                _script = value;
                OnPropertyChanged();
            }
        }
        private string _script = "";

        public string TipText => string.Join("\n  ", CurrentPackage?.ConfigTip ?? []);

        public string FiscalConfig
        {
            get => _config;
            set
            {
                if (_config == value) return;
                _config = value;
                OnPropertyChanged();
            }
        }
        private string _config = "";

        public bool IsPersistence
        {
            get => persistence;
            set
            {
                if (persistence == value) return;
                persistence = value;
                OnPropertyChanged();
            }
        }
        private bool persistence;

        private void UpdateScript()
        {
            if(_current is null)
            {
                ScriptText = "";
                return;
            }
            var builder = new StringBuilder();
            var agentPath = Path.Combine("C:", "RetailCorrector");
            builder.AppendLine($"New-Item -ItemType Directory -Path \"{agentPath}\"");
            var path = Path.Combine(agentPath, "tasks");
            builder.AppendLine($"New-Item -ItemType Directory -Path \"{path}\"");
            builder.AppendLine("$http = New-Object Net.WebClient");
            foreach (var depend in _current.Depends)
            {
                path = Path.Combine(agentPath, depend.FileName);
                builder.AppendLine($"$http.DownloadFile(\"{depend.Url}\", \"{path}\")");
            }
            path = Path.Combine(agentPath, $"{_current.EndpointPath.Split(", ")[^1]}.dll");
            builder.AppendLine($"$http.DownloadFile(\"{_current.Url}\", \"{path}\")");
            path = Path.Combine(agentPath, "RetailCorrector.Agent.exe");
            builder.AppendLine($"$http.DownloadFile(\"https://github.com/ornaras/RetailCorrector.Archive/raw/refs/heads/main/v{App.Version}/RetailCorrector.Agent.exe\", \"{path}\")");
            foreach(var id in pastebinIds ?? [])
            {
                path = Path.Combine(agentPath, "tasks", $"{id}.json");
                builder.AppendLine($"$http.DownloadFile(\"https://pastebin.com/raw/{id}\", \"{path}\")");
            }
            var binPath = new StringBuilder(Path.Combine(agentPath, "RetailCorrector.Agent.exe"));
            binPath.Append($" -m '{_current.EndpointPath}'");
            if (IsPersistence) binPath.Append(" -p");
            binPath.Append($" -c '{FiscalConfig}'");
            builder.AppendLine($"New-Service -Name \"RetailCorrector\" -DisplayName \"Корректирующий кассир\" -BinaryPathName \"{binPath}\" -StartupType Automatic");
            ScriptText = builder.ToString();
        }

        public Publisher()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(ScriptText))
                    UpdateScript();
            };
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private void PushReceipts(object sender, RoutedEventArgs e)
        {
            var chunks = App.Receipts.Edited.Chunk(25).ToArray();
            using var http = new HttpClient();
            pastebinIds = new string[chunks.Length];
            for (var i = 0; i < chunks.Length; i++)
            {
                var text = JsonSerializer.Serialize(chunks[i]);
                using var req = new HttpRequestMessage(HttpMethod.Post, "https://pastebin.com/api/api_post.php");
                Dictionary<string, string> @params = new()
                {
                    { "api_paste_format", "json"},
                    { "api_dev_key", "dJwp4jZ2sJbgOUBJxXC4kXjumUXEaCqr"},
                    { "api_option", "paste"},
                    { "api_paste_code", text},
                    { "api_paste_private", "1"},
                };
                req.Content = new FormUrlEncodedContent(@params);
                using var resp = http.Send(req);
                var url = resp.Content.ReadAsStringAsync().Result;
                pastebinIds[i] = url.Split('/')[^1];
            }
            UpdateScript();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Packages.Clear();
            if(e.OriginalSource is UserControl us && (int)us.ActualHeight != 0) 
                foreach(var p in App.Repository.Value.Where(p => p.Type == RepoPackage.RepoPackageType.Fiscal))
                    Packages.Add(p);            
        }
    }
}
