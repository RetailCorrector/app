using RetailCorrector.Editor.Receipt.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace RetailCorrector.Editor.Receipt
{
    public partial class IndustryEditor : Window
    {
        [NotifyUpdated] private int _selectedIndex = -1;

        public IndustryEditor(ReceiptViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
        public IndustryEditor(PositionViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        public void Add(object? s, RoutedEventArgs e) =>
            ((ObservableCollection<IndustryViewModel>)((dynamic)DataContext)
                .Industry).Add(new IndustryViewModel());

        public void Remove(object? s, RoutedEventArgs e)
        {
            if (_selectedIndex == -1) return;
            ((ObservableCollection<IndustryViewModel>)((dynamic)DataContext).Industry).RemoveAt(_selectedIndex);
        }
            
    }
}
