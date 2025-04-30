using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
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
            }
        }
        private RepoPackage _current;

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
            var builder = new StringBuilder();
            var agentPath = Path.Combine("C:", "RetailCorrector");
            builder.AppendLine($"New-Item -ItemType Directory -Path \"{agentPath}\"");
            builder.AppendLine("$http = New-Object Net.WebClient");
            string path;
            foreach (var depend in _current.Depends)
            {
                path = Path.Combine(agentPath, depend.FileName);
                builder.AppendLine($"$http.DownloadFile(\"{depend.Url}\", \"{path}\")");
            }
            path = Path.Combine(agentPath, $"{_current.EndpointPath.Split(", ")[^1]}.dll");
            builder.AppendLine($"$http.DownloadFile(\"{_current.Url}\", \"{path}\")");
            path = Path.Combine(agentPath, "RetailCorrector.Agent.exe");
            builder.AppendLine($"$http.DownloadFile(\"https://\", \"{path}\")");
            var binPath = new StringBuilder(path);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            var chunks = App.Receipts.Edited.Chunk(25);
            foreach (var chunk in chunks)
            {
                var text = JsonSerializer.Serialize(chunk);
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Path.GetRandomFileName());
                File.WriteAllText(path, text);
            }*/
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
