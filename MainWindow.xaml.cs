using RetailCorrector.Wizard.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            App.Receipts.Parsed.CollectionChanged += (_, e) =>
            {
                IsEnableEditor = (e.NewItems?.Count ?? 0) > 0;
                Editor.RefreshPreview();
            };
            App.Receipts.Edited.CollectionChanged += (_, e) => IsEnablePublisher = (e.NewItems?.Count ?? 0) > 0;
            InitializeComponent();
            parser.OnSearched += arr =>
            {
                App.Receipts.Parsed.Clear();
                App.Receipts.Filtered.Clear();
                App.Receipts.Edited.Clear();
                foreach (var item in arr)
                    App.Receipts.Parsed.Add(item);
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
    }
}