using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.HistoryActions;
using RetailCorrector.Wizard.Windows;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailCorrector.Wizard.UserControls
{
    public partial class ReceiptView : UserControl, INotifyPropertyChanged
    {
        public readonly static DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(Receipt), typeof(ReceiptView));

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

        public Receipt DataSource
        {
            get => (Receipt)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        public ReceiptView()
        {
            InitializeComponent();
        }

        public void SwitchSelection(object? s, RoutedEventArgs e) => IsSelected = !IsSelected;

        private void OpenDialog(object sender, MouseButtonEventArgs e)
        {
            var i = WizardDataContext.Receipts.IndexOf(DataSource);
            var wizard = new ReceiptWizard(DataSource);
            if(wizard.ShowDialog() == true)
            {
                WizardDataContext.History.Add(new EditReceipts(i));
                WizardDataContext.Receipts[i] = wizard.Data;
            }
        }
    }
}
