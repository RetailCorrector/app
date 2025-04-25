using RetailCorrector.Wizard.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            App.Receipts.Parsed.CollectionChanged += (_, e) => IsEnableEditor = (e.NewItems?.Count ?? 0) > 0;
            App.Receipts.Edited.CollectionChanged += (_, e) => IsEnablePublisher = (e.NewItems?.Count ?? 0) > 0;
            InitializeComponent();
            parser.OnSearchBegin += () =>
            {
                App.Receipts.Parsed.Clear();
                App.Receipts.Filtered.Clear();
                App.Receipts.Edited.Clear();
            };
            parser.OnSearched += arr =>
            {
                foreach (var item in arr)
                    App.Receipts.Parsed.Add(item);
                MessageBox.Show("Сканирование завершено!");
            };
        }

        public bool IsEnableEditor
        {
            get => isEnableEditor;
            set
            {
                if (isEnableEditor == value) return;
                isEnableEditor = value;
                OnPropertyChanged();
            }
        }
        private bool isEnableEditor = false;

        public bool IsEnablePublisher
        {
            get => isEnablePublisher;
            set
            {
                if (isEnablePublisher == value) return;
                isEnablePublisher = value;
                OnPropertyChanged();
            }
        }
        private bool isEnablePublisher = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string paramName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(paramName));

        private void ActivationTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != sender || e.RemovedItems.Count < 1) return;
            switch (((TabItem)e.AddedItems[0]).Header)
            {
                case "Сборка":
                    break;
                default:
                    break;
            }
        }

        private void ParserLog(bool error, string text, Exception exception)
        {
            if (error)
                App.Logger.Error(exception, text);
            else
                App.Logger.Information(text);
        }
    }
}