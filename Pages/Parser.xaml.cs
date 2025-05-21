using System.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Parser : UserControl, INotifyPropertyChanged
    {
        public LocalModule Package
        {
            get => _package;
            set
            {
                if (_package == value) return;
                _package = value;
                OnPropertyChanged();
                UpdateSetting();
            }
        }
        private void UpdateSetting()
        {
            Settings.Clear();
            var type = Package.EntryPoint!.GetType();
            var names = from prop in type.GetProperties()
                    where prop.GetCustomAttribute<DisplayNameAttribute>() is not null
                    select prop.GetCustomAttribute<DisplayNameAttribute>()!.DisplayName;
            foreach (var name in names)
                Settings.Add(new StringsPair(name));
        }
        private LocalModule _package;

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

        public ObservableCollection<StringsPair> Settings { get; set; } = [];

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string nameProp = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameProp));

        static Parser()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public Parser()
        {
            Cancel();
            InitializeComponent();
        }

        private async void RunSearch(object sender, RoutedEventArgs e)
        {
            ResetSource();
            App.Receipts.Parsed.Clear();
            CurrentProgress = 0;
            var type = Package.EntryPoint!.GetType();
            PropertyInfo[] names = [..type.GetProperties().Where(prop => prop.GetCustomAttribute<DisplayNameAttribute>() is not null)];
            for (var i = 0; i < names.Length; i++)
                names[i].SetValue(Package.EntryPoint!, Settings[i].Value);
            var arr = await Package.EntryPoint!.Parse(cancelSource.Token);
            foreach (var receipt in arr)
                App.Receipts.Parsed.Add(receipt);
            Cancel();
        }

        private void CancelSearch(object sender, RoutedEventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ModuleManager().ShowDialog();
        }
    }
}
