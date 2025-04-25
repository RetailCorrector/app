using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
                Editor.RefreshFilterPreview();
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
                Editor.RefreshFilterPreview();
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
                Editor.RefreshFilterPreview();
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

    public enum PropertyType
    {
        Receipt,
        Position
    }
    public enum ReceiptProperty
    {
        PaymentCash,
        PaymentEcash,
        PaymentPre,
        PaymentPost,
        PaymentProvision,
        Operation,
        //DateTime,
        TotalSum
    }
    public enum PositionProperty
    {
        Name,
        Quantity,
        Price,
        Sum,
        Measure,
        Payment,
        Type,
        Tax
    }
    public class EditorItem : INotifyPropertyChanged
    {
        public PropertyType DataType
        {
            get => _dataType;
            set
            {
                if (_dataType == value) return;
                _dataType = value;
                Editor.RefreshEditorPreview();
                OnPropertyChanged();
                OnPropertyChanged(nameof(EnumValues));
                OnPropertyChanged(nameof(ConditionPropertyName));
                OnPropertyChanged(nameof(PropertyName));
            }
        }
        public Array EnumValues => 
            Enum.GetValues(
                _dataType == PropertyType.Receipt ? 
                typeof(ReceiptProperty) : 
                typeof(PositionProperty));
        public int ConditionPropertyId
        {
            get => _conditionId;
            set
            {
                if (_conditionId == value) return;
                _conditionId = value;
                Editor.RefreshEditorPreview();
                OnPropertyChanged();
                OnPropertyChanged(nameof(ConditionPropertyName));
            }
        }
        public string ConditionPropertyName =>
            _dataType == PropertyType.Receipt ?
            ((ReceiptProperty)_conditionId).ToString() :
            ((PositionProperty)_conditionId).ToString();
        public string ConditionPropertyPattern
        {
            get => _pattern;
            set
            {
                if (_pattern == value) return;
                _pattern = value;
                Editor.RefreshEditorPreview();
                OnPropertyChanged();
            }
        }
        public int PropertyId
        {
            get => _propId;
            set
            {
                if (_propId == value) return;
                _propId = value;
                Editor.RefreshEditorPreview();
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertyName));
            }
        }
        public string PropertyName =>
            _dataType == PropertyType.Receipt ?
            ((ReceiptProperty)_propId).ToString() :
            ((PositionProperty)_propId).ToString();
        public string PropertyValue
        {
            get => _propValue;
            set
            {
                if (_propValue == value) return;
                _propValue = value;
                Editor.RefreshEditorPreview();
                OnPropertyChanged();
            }
        }

        private PropertyType _dataType = PropertyType.Receipt;
        private int _conditionId = 0;
        private string _pattern = "";
        private int _propId = 0;
        private string _propValue = "";

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Логика взаимодействия для Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public ObservableCollection<FilterItem> FilterItems { get; }
        public ObservableCollection<EditorItem> EditorItems { get; }
        internal static ObservableCollection<FilterItem> StaticFilters => Instance.FilterItems;
        internal static EditorItem[] StaticREditors => [..Instance.EditorItems.Where(e => e.DataType == PropertyType.Receipt)];
        internal static EditorItem[] StaticPEditors => [.. Instance.EditorItems.Where(e => e.DataType == PropertyType.Position)];
        private static Editor Instance;

        public static void RefreshFilterPreview()
        {
            App.Receipts.Filtered.Clear();
            App.Receipts.Edited.Clear();
            foreach (var rec in App.Receipts.Parsed)
                App.Receipts.Filtered.Add(new Tuple<bool, Receipt>(Check(rec), rec));
        }

        public static void RefreshEditorPreview()
        {
            App.Receipts.Edited.Clear();
            foreach (var rec in App.Receipts.Filtered.Where(r => r.Item1).Select(r => r.Item2))
                App.Receipts.Edited.Add(Edit(rec));
        }

        private static Receipt Edit(Receipt origin)
        {
            var res = origin.Clone();
            foreach(var e in StaticREditors)
            {
                if (string.IsNullOrWhiteSpace(e.PropertyValue)) continue;
                if (!CheckReceipt(origin, (ReceiptProperty)e.ConditionPropertyId, e.ConditionPropertyPattern)) continue;
                switch ((ReceiptProperty)e.PropertyId)
                {
                    case ReceiptProperty.PaymentCash:
                        var pay = res.Payment;
                        pay.Cash = uint.Parse(e.PropertyValue);
                        res.Payment = pay;
                        break;
                    case ReceiptProperty.PaymentEcash:
                        pay = res.Payment;
                        pay.ECash = uint.Parse(e.PropertyValue);
                        res.Payment = pay;
                        break;
                    case ReceiptProperty.PaymentPre:
                        pay = res.Payment;
                        pay.Pre = uint.Parse(e.PropertyValue);
                        res.Payment = pay;
                        break;
                    case ReceiptProperty.PaymentPost:
                        pay = res.Payment;
                        pay.Post = uint.Parse(e.PropertyValue);
                        res.Payment = pay;
                        break;
                    case ReceiptProperty.PaymentProvision:
                        pay = res.Payment;
                        pay.Provision = uint.Parse(e.PropertyValue);
                        res.Payment = pay;
                        break;
                    case ReceiptProperty.Operation: res.Operation = (Operation)int.Parse(e.PropertyValue); break;
                    case ReceiptProperty.TotalSum: res.RoundedSum = uint.Parse(e.PropertyValue); break;
                    default: break;
                }
            }
            for(var i = 0; i < res.Items.Length; i++)
            {
                var pos = res.Items[i].Clone();
                foreach(var e in StaticPEditors)
                {
                    if (string.IsNullOrWhiteSpace(e.PropertyValue)) continue;
                    if (!CheckPosition(res.Items[i], (PositionProperty)e.ConditionPropertyId, e.ConditionPropertyPattern)) continue;
                    switch ((PositionProperty)e.PropertyId)
                    {
                        case PositionProperty.Name: pos.Name = e.PropertyValue; break;
                        case PositionProperty.Measure: pos.MeasureUnit = (MeasureUnit)uint.Parse(e.PropertyValue); break;
                        case PositionProperty.Payment: pos.PayType = (PaymentType)uint.Parse(e.PropertyValue); break;
                        case PositionProperty.Price: pos.Price = uint.Parse(e.PropertyValue); break;
                        case PositionProperty.Quantity: 
                            pos.Quantity = uint.Parse(e.PropertyValue);
                            pos.TotalSum = (uint)(pos.Quantity * pos.Price / 1000.0);
                            break;
                        case PositionProperty.Sum: pos.TotalSum = uint.Parse(e.PropertyValue); break;
                        case PositionProperty.Tax: pos.TaxRate = (TaxRate)uint.Parse(e.PropertyValue); break;
                        case PositionProperty.Type: pos.PosType = (PositionType)uint.Parse(e.PropertyValue); break;
                        default: break;
                    }
                }
                res.Items[i] = pos;
            }
            return res;
        }

        private static bool CheckReceipt(Receipt receipt, ReceiptProperty type, string pattern) =>
            type switch
            {
                ReceiptProperty.PaymentPre => $"{receipt.Payment.Pre}" == pattern,
                ReceiptProperty.PaymentPost => $"{receipt.Payment.Post}" == pattern,
                ReceiptProperty.PaymentCash => $"{receipt.Payment.Cash}" == pattern,
                ReceiptProperty.PaymentEcash => $"{receipt.Payment.ECash}" == pattern,
                ReceiptProperty.PaymentProvision => $"{receipt.Payment.Provision}" == pattern,
                ReceiptProperty.Operation => $"{receipt.Operation:D}" == pattern,
                ReceiptProperty.TotalSum => $"{receipt.RoundedSum}" == pattern,
                _ => false
            };

        private static bool CheckPosition(Position position, PositionProperty type, string pattern) =>
            type switch
            {
                PositionProperty.Name => Regex.IsMatch(position.Name, pattern),
                PositionProperty.Measure => $"{position.MeasureUnit:D}" == pattern,
                PositionProperty.Payment => $"{position.PayType:D}" == pattern,
                PositionProperty.Price => $"{position.Price}" == pattern,
                PositionProperty.Quantity => $"{position.Quantity}" == pattern,
                PositionProperty.Sum => $"{position.TotalSum}" == pattern,
                PositionProperty.Tax => $"{position.TaxRate:D}" == pattern,
                PositionProperty.Type => $"{position.PosType:D}" == pattern,
                _ => false
            };

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
            FilterItems = [];
            EditorItems = [];
            InitializeComponent();
        }

        private void AddParamFilter(object sender, RoutedEventArgs e)
        {
            FilterItems.Add(new FilterItem());
            RefreshFilterPreview();
        }

        private void RemoveParamFilter(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem i || i.Parent is not ContextMenu c || c.PlacementTarget is not DataGrid d || d.SelectedIndex == -1) return;
            FilterItems.RemoveAt(d.SelectedIndex);
            RefreshFilterPreview();
        }

        private void AddParamEditor(object sender, RoutedEventArgs e)
        {
            EditorItems.Add(new EditorItem());
            RefreshEditorPreview();
        }

        private void RemoveParamEditor(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem i || i.Parent is not ContextMenu c || c.PlacementTarget is not DataGrid d || d.SelectedIndex == -1) return;
            EditorItems.RemoveAt(d.SelectedIndex);
            RefreshEditorPreview();
        }

        private void ShowEditor(object sender, RoutedEventArgs e) => RefreshEditorPreview();
        private void ShowFilter(object sender, RoutedEventArgs e) => RefreshFilterPreview();

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                previewEditor.SelectedIndex = -1;
                previewFilter.SelectedIndex = -1;
            }
            base.OnKeyDown(e);
        }
    }
}
