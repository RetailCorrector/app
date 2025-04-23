using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.Pages
{
    public enum Property
    {
        ItemName,
        ItemQuantity,
        ItemPrice,
        ItemSum,
        ItemMeasure,
        ItemPayment,
        ItemType,
        ItemTax,
        PaymentCash,
        PaymentEcash,
        PaymentPre,
        PaymentPost,
        PaymentProvision,
        Operation,
        DateTime,
        TotalSum
    }

    public class FilterItem : INotifyPropertyChanged
    {
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled == value) return;
                isEnabled = value;
                Editor.RefreshPreview();
                OnPropertyChanged();
            }
        }
        public Property Property
        {
            get => property;
            set
            {
                if (property == value) return;
                property = value;
                Editor.RefreshPreview();
                OnPropertyChanged();
            }
        }
        public string Pattern
        {
            get => pattern;
            set
            {
                if (pattern == value) return;
                pattern = value;
                Editor.RefreshPreview();
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool isEnabled = false;
        private Property property = Property.DateTime;
        private string pattern = "";
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }

    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public ObservableCollection<FilterItem> Items { get; }
        internal static ObservableCollection<FilterItem> StaticFilters => Instance.Items;
        private static Editor Instance;

        public static void RefreshPreview()
        {
            App.Receipts.Filtered.Clear();
            App.Receipts.Edited.Clear();
            foreach (var rec in App.Receipts.Parsed)
                App.Receipts.Filtered.Add(new Tuple<bool, Receipt>(Check(rec), rec));
        }

        private static bool Check(Receipt receipt)
        {
            var checks = new bool[StaticFilters.Count];
            for(var i = 0; i < StaticFilters.Count; i++)
            {
                var Pattern = StaticFilters[i].Pattern;
                checks[i] = !StaticFilters[i].IsEnabled || StaticFilters[i].Property switch
                {
                    Property.DateTime => receipt.Created.ToString("yyyy'-'MM'-'dd") == Pattern,
                    Property.ItemName => receipt.Items.Any(i => Regex.IsMatch(i.Name, Pattern)),
                    Property.ItemMeasure => receipt.Items.Any(i => $"{i.MeasureUnit:D}" == Pattern),
                    Property.ItemPayment => receipt.Items.Any(i => $"{i.PayType:D}" == Pattern),
                    Property.ItemPrice => receipt.Items.Any(i => $"{i.Price}" == Pattern),
                    Property.ItemQuantity => receipt.Items.Any(i => $"{i.Quantity}" == Pattern),
                    Property.ItemSum => receipt.Items.Any(i => $"{i.TotalSum}" == Pattern),
                    Property.ItemTax => receipt.Items.Any(i => $"{i.TaxRate:D}" == Pattern),
                    Property.ItemType => receipt.Items.Any(i => $"{i.PosType:D}" == Pattern),
                    Property.PaymentPre => $"{receipt.Payment.Pre}" == Pattern,
                    Property.PaymentPost => $"{receipt.Payment.Post}" == Pattern,
                    Property.PaymentCash => $"{receipt.Payment.Cash}" == Pattern,
                    Property.PaymentEcash => $"{receipt.Payment.ECash}" == Pattern,
                    Property.PaymentProvision => $"{receipt.Payment.Provision}" == Pattern,
                    Property.Operation => $"{receipt.Operation:D}" == Pattern,
                    Property.TotalSum => $"{receipt.RoundedSum}" == Pattern,
                    _ => false,
                };
            }
            return checks.Length == 0 || checks.All(c => c);
        }

        public Editor()
        {
            Instance = this;
            Items = [];
            InitializeComponent();
        }

        private void AddParam(object sender, RoutedEventArgs e)
        {
            Items.Add(new FilterItem());
            RefreshPreview();
        }

        private void RemoveParam(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem i || i.Parent is not ContextMenu c || c.PlacementTarget is not DataGrid d || d.SelectedIndex == -1) return;
            Items.RemoveAt(d.SelectedIndex);
            RefreshPreview();
        }
    }
}
