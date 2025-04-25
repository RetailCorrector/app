using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Filter : Grid
    {
        public ObservableCollection<FilterItem> FilterItems { get; } = [];

        public Filter()
        {
            InitializeComponent();
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            var item = new FilterItem();
            item.PropertyChanged += (s, e) => RefreshPreview();
            FilterItems.Add(item);
            RefreshPreview();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem i || i.Parent is not ContextMenu c || c.PlacementTarget is not DataGrid d || d.SelectedIndex == -1) return;
            FilterItems.RemoveAt(d.SelectedIndex);
            RefreshPreview();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => RefreshPreview();

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                preview.SelectedIndex = -1;
            base.OnKeyDown(e);
        }
        public void RefreshPreview()
        {
            App.Receipts.Filtered.Clear();
            App.Receipts.Edited.Clear();
            foreach (var rec in App.Receipts.Parsed)
                App.Receipts.Filtered.Add(new Tuple<bool, Receipt>(Check(rec), rec));
        }
        private bool Check(Receipt receipt)
        {
            var checks = new bool[FilterItems.Count];
            for (var i = 0; i < FilterItems.Count; i++)
                checks[i] = FilterItems[i].Check(receipt);
            return checks.Length == 0 || checks.All(c => c);
        }
    }
}
