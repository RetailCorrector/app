using Microsoft.Win32;
using RetailCorrector.Wizard.Repository;
using Serilog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Publisher : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<FiscalPackage> Packages { get; } = [];
        public FiscalPackage CurrentPackage
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
        private FiscalPackage _current;

        public int MaxProgress 
        {
            get => _maxProgress;
            set
            {
                _maxProgress = value;
                OnPropertyChanged();
                CurrProgress = 0;
                LogText = "";
            }
        }
        private int _maxProgress;

        public string LogText
        {
            get => _log;
            set
            {
                if (_log == value) return;
                _log = value;
                OnPropertyChanged();
            }
        }
        private string _log;

        public int CurrProgress 
        {
            get => _currProgress;
            set
            {
                if (_currProgress == value) return;
                _currProgress = value; 
                OnPropertyChanged();
            }
        }
        private int _currProgress;

        public string TipText => string.Join("\n  ", CurrentPackage?.Tooltip ?? []);

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

        public Publisher()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private async void SaveConfig(object sender, RoutedEventArgs e)
        {
            try
            {
                MaxProgress = 4;
                await Task.Run(ClearTempFolder);
                await Task.Run(CreateTempFolder);
                await Task.Run(SaveReceipts);
                await Task.Run(SaveReport);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка при выгрузке... Сообщите разработчику!", MessageBoxButton.OK, MessageBoxImage.Hand);
                Log.Logger.Fatal(ex, "Не удалось выгрузить чеки и отчет!");
            }
        }

        private async void BuildSetup(object sender, RoutedEventArgs e)
        {
            try
            {
                MaxProgress = 8;
                await Task.Run(ClearTempFolder);
                await Task.Run(CreateTempFolder);
                await Task.Run(UnzipAgent);
                await Task.Run(DownloadFiscal);
                await Task.Run(SaveReceipts);
                await Task.Run(SaveReport);
                await Task.Run(Configure);
                await BuildInstaller();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка при сборке... Сообщите разработчику!", MessageBoxButton.OK, MessageBoxImage.Hand);
                Log.Logger.Fatal(ex, "Не удалось собрать установщик агента!");
            }
            finally
            {
                await Task.Run(ClearTempFolder);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Packages.Clear();
            if (e.OriginalSource is UserControl us && (int)us.ActualHeight != 0)
            {
                using var http = new HttpClient();
                using var req = new HttpRequestMessage(HttpMethod.Get, App.RepositoryUrl);
                using var resp = http.Send(req);
                var content = resp.Content.ReadAsStringAsync().Result;
                var repo = RepoPackage.Parse(JsonNode.Parse(content)!.AsArray());
                foreach (var p in repo)
                    if (p is FiscalPackage fiscal)
                        Packages.Add(fiscal);
            }
        }

        private void CreateTempFolder()
        {
            LogText += ">>> Создание временной папки...\n";
            var path = Path.Combine(AppContext.BaseDirectory, "Temp");
            Directory.CreateDirectory(path);
            CurrProgress++;
        }
        private void UnzipAgent()
        {
            LogText += ">>> Распаковка заготовки агента...\n";
            var path = Path.Combine(AppContext.BaseDirectory, "agent.zip");
            var temp = Path.Combine(AppContext.BaseDirectory, "Temp");
            using var fs = File.OpenRead(path);
            using var zip = new ZipArchive(fs);
            zip.ExtractToDirectory(temp, true);
            CurrProgress++;
        }
        private void DownloadFiscal()
        {
            LogText += ">>> Скачивание фискальной интеграции...\n";
            using var http = new HttpClient();
            using var req = new HttpRequestMessage(HttpMethod.Get, _current.Uri);
            using var resp = http.Send(req);
            using var stream = resp.Content.ReadAsStream();
            var path = Path.Combine(AppContext.BaseDirectory, "Temp", "ExtFiscal.dll");
            using var fs = File.Create(path);
            stream.CopyTo(fs);
            CurrProgress++;
        }
        private void SaveReceipts()
        {
            LogText += ">>> Выгрузка отредактированых чеков...\n";
            var chunks = App.Receipts.Edited.Chunk(25).ToArray();
            for (var i = 0; i < chunks.Length; i++)
            {
                var text = JsonSerializer.Serialize(chunks[i]);
                var path = Path.Combine(AppContext.BaseDirectory, "Temp", "tasks", $"{Path.GetRandomFileName().Replace(".", "")}.json");
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, text);
            }
            CurrProgress++;
        }
        private void SaveReport()
        {
            LogText += ">>> Выгрузка шаблона отчета...\n";
            var text = JsonSerializer.Serialize(App.Receipts.Report);
            var path = Path.Combine(AppContext.BaseDirectory, "Temp", "report.json");
            File.WriteAllText(path, text);
            CurrProgress++;
        }
        private void Configure()
        {
            LogText += ">>> Конфигурирование агента...\n";
            SetConfig("generic", "fiscal", FiscalConfig);
            CurrProgress++;
        }
        private async Task BuildInstaller()
        {
            LogText += ">>> Упаковка агента...\n";
            var info = new ProcessStartInfo()
            {
                FileName = Path.Combine(AppContext.BaseDirectory, "NSIS", "makensis.exe"),
                Arguments = Path.Combine(AppContext.BaseDirectory, "Temp", "installer.nsi"),
                CreateNoWindow = true
            };
            var proc = Process.Start(info)!;
            await proc.WaitForExitAsync();
            LogText += ">>> Сохранение установщика агента\n";
            var save = new SaveFileDialog
            {
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"RCAgent_Setup.exe"),
                Filter = "Исполняемый файл|*.exe"
            };
            save.ShowDialog();
            LogText += $">>> Установщик агента находится по пути: {save.FileName}\n";
            File.Move(Path.Combine(AppContext.BaseDirectory, "Temp", "RCAgent_Setup.exe"), save.FileName, true);
            CurrProgress++;
        }
        private void ClearTempFolder()
        {
            LogText += ">>> Удаление временных файлов...\n";
            var path = Path.Combine(AppContext.BaseDirectory, "Temp");
            if(Directory.Exists(path)) Directory.Delete(path, true);
            CurrProgress++;
        }

        private static void SetConfig(string section, string key, string value)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Temp", "settings.ini");
            WritePrivateProfileString(section, key, value, path);
        }

        #region Native
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        #endregion
    }
}
