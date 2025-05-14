using RetailCorrector.Wizard.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Parser : UserControl, INotifyPropertyChanged
    {
        public ObservableCollection<SourcePackage> Packages { get; set; } = [];

        public SourcePackage Package
        {
            get => _package;
            set
            {
                if (_package == value) return;
                _package = value;
                OnPropertyChanged();
                Settings.Clear();
                foreach (var prop in _package.Properties)
                    Settings.Add(new StringsPair(prop.Key));
            }
        }
        private SourcePackage _package;

        private int _maxProgress = 1;
        public int MaxProgress 
        { 
            get => _maxProgress;
            set
            {
                _maxProgress = value;
                Dispatcher.Invoke(() => {

                    App.Receipts.Parsed.Clear();
                    App.Receipts.Filtered.Clear();
                    App.Receipts.Edited.Clear();
                });
                OnPropertyChanged();
            }
        }

        private int _currProgress = 0;
        public int CurrentProgress 
        { 
            get => _currProgress;
            set
            {
                if (_currProgress == value) return;
                _currProgress = value;
                if (value == MaxProgress) Cancel();
                OnPropertyChanged();
            }
        }

        public bool CancelEnabled => !SearchEnabled;
        public bool SearchEnabled => cancelSource.IsCancellationRequested;

        private CancellationTokenSource cancelSource = new();

        private Process powerShell;

        public ObservableCollection<StringsPair> Settings { get; set; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string nameProp = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProp));

        public Parser()
        {
            foreach(var p in App.Repository.Value) 
                if(p is SourcePackage source)
                    Packages.Add(source);
            Cancel();
            InitializeComponent();
        }

        private void RunPowerShell()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "WindowsPowerShell", "v1.0", "powershell.exe"),
                Arguments = "-ExecutionPolicy Unrestricted -NoExit -NoLogo -Command -",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                CreateNoWindow = true,
            };
            powerShell = Process.Start(startInfo)!;
            powerShell.ErrorDataReceived += (_, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data)) return;
                App.Logger.Error($"Ошибка из PowerShell: {e.Data}");
            };
            powerShell.OutputDataReceived += (_, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data)) return;
                App.Logger.Debug($"Ответ от PowerShell: {e.Data}");
                OutputHandle(e.Data);
            };
            powerShell.BeginErrorReadLine();
            powerShell.BeginOutputReadLine();
        }

        private Receipt? _currReceipt;
        private int _posIndex;

        private void OutputHandle(string output)
        {
            var args = output.Split('|');
            switch (args[0])
            {
                case "0": // количество пунктов
                    MaxProgress = int.Parse(args[1]);
                    break;
                case "1": // новый чек
                    if (_currReceipt is not null) 
                        App.Receipts.Parsed.Add((Receipt)_currReceipt);
                    _posIndex = 0;
                    _currReceipt = new Receipt
                    {
                        Operation = (Operation)int.Parse(args[1]),
                        ActNumber = " ",
                        CorrectionType = CorrType.ByYourself,
                        Created = DateTime.ParseExact(args[2], "yyyy'-'MM'-'dd'T'HH':'mm':'ss", CultureInfo.InvariantCulture),
                        FiscalSign = args[3],
                        RoundedSum = uint.Parse(args[4]),
                        Items = new Position[int.Parse(args[5])],
                        Payment = new Payment
                        {
                            Cash = uint.Parse(args[6]),
                            ECash = uint.Parse(args[7]),
                            Pre = uint.Parse(args[8]),
                            Post= uint.Parse(args[9]),
                            Provision= uint.Parse(args[10]),
                        }
                    };
                    break;
                case "2": // новая позиция
                    ((Receipt)_currReceipt!).Items[_posIndex] = new Position
                    {
                        Name = args[1],
                        Price = uint.Parse(args[2]),
                        Quantity = uint.Parse(args[3]),
                        TotalSum = uint.Parse(args[4]),
                        MeasureUnit = (MeasureUnit)int.Parse(args[5]),
                        TaxRate = (TaxRate)int.Parse(args[6]),
                        PayType = (PaymentType)int.Parse(args[7]),
                        PosType = (PositionType)int.Parse(args[8]),
                    };
                    _posIndex++;
                    break;
                case "3": // пункт пройден
                    if (_currReceipt is not null)
                    {
                        App.Receipts.Parsed.Add((Receipt)_currReceipt);
                        _currReceipt = null;
                    }
                    CurrentProgress++;
                    break;
            }
        }

        private async Task InputPowerShell(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            await powerShell.StandardInput.WriteLineAsync(text);
            await powerShell.StandardInput.FlushAsync();
            App.Logger.Debug($"Ввод в PowerShell: {text}");
        }

        private async void RunSearch(object sender, RoutedEventArgs e)
        {
            ResetSource();
            CurrentProgress = 0;
            RunPowerShell();
            await InputPowerShell($"Invoke-Expression (New-Object System.Net.WebClient).DownloadString(\"{_package.Uri}\")");
            foreach(var k in Settings) await InputPowerShell(k.Value);
        }

        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            powerShell?.Kill();
            powerShell?.Dispose();
            cancelSource.Cancel();
            OnPropertyChanged(nameof(CancelEnabled));
            OnPropertyChanged(nameof(SearchEnabled));
        }

        private void ResetSource()
        {
            cancelSource = new CancellationTokenSource();
            OnPropertyChanged(nameof(CancelEnabled));
            OnPropertyChanged(nameof(SearchEnabled));
        }
    }
}
