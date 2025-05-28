using RetailCorrector.Wizard.ModuleSystem;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace RetailCorrector.Wizard.Windows
{
    public partial class Parser : Window, INotifyPropertyChanged
    {
        private AbstractSourceModule? _module;
        public AbstractSourceModule? Module
        {
            get => _module;
            set
            {
                _module = value;
                OnPropertyChanged();
            }
        }


        public Parser()
        {
            ModuleCollection.Load().Wait();
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected override async void OnClosed(EventArgs e)
        {
            await ModuleCollection.Unload();
            base.OnClosed(e);
        }
    }
}
