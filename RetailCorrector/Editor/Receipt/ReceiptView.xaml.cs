using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RetailCorrector.History;
using RetailCorrector.History.Actions;

namespace RetailCorrector.Editor.Receipt
{
    public partial class ReceiptView : UserControl, INotifyPropertyChanged
    {
        public readonly static DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(RetailCorrector.Receipt), typeof(ReceiptView));

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public RetailCorrector.Receipt DataSource
        {
            get => (RetailCorrector.Receipt)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public ReceiptView()
        {
            InitializeComponent();
        }

        public void SwitchSelection(object? s, RoutedEventArgs e) => IsSelected = !IsSelected;

        private void OpenDialog(object sender, MouseButtonEventArgs e)
        {
            var i = Env.Receipts.IndexOf(DataSource);
            var wizard = new ReceiptWizard(DataSource);
            if(wizard.ShowDialog() == true)
                HistoryController.Add(new EditReceipts(i, wizard.Data));
        }
    }
}
