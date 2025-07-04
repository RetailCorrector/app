using RetailCorrector.Editor.Receipt.ViewModels;
using RetailCorrector.Utils;
using System.Collections.ObjectModel;

namespace RetailCorrector.Editor.Receipt;

public partial class PositionViewModel(ReceiptViewModel parent)
{
    public ObservableCollection<IndustryViewModel> Industry { get; set; } = [];
    public PositionViewModel(ReceiptViewModel parent, Position data) : this(parent)
    {
        PropertyChanged += (_, e) => parent.OnPropertyChanged(nameof(parent.Done));
        Name = data.Name;
        Type = data.PosType;
        Pay = data.PayType;
        Tax = data.TaxRate;
        Measure = data.MeasureUnit;
        Price = data.Price / 100.0;
        Quantity = data.Quantity / 1000.0;
        Sum = data.TotalSum / 100.0;
        Codes = data.Codes;
        foreach (var i in data.IndustryData ?? [])
            Industry.Add(i);
    }

    [NotifyChanged] private string _name = "";
    [NotifyChanged] private PositionType _type = PositionType.Product;
    [NotifyChanged] private PaymentType _pay = PaymentType.FullPayment;
    [NotifyChanged] private TaxRate _tax = TaxRate.Tax10;
    [NotifyChanged] private MeasureUnit _measure = MeasureUnit.Default;
    [NotifyChanged] private double _price = 0;
    [NotifyChanged] private double _quantity = 1;
    [NotifyChanged] private double _sum = 0;
    [NotifyChanged] private CodeViewModel _codes = new();

    public bool Done =>
        !string.IsNullOrWhiteSpace(Name) &&
        Price > 0 && Quantity > 0 &&
        Sum == Math.Round(Price * Quantity, 2);

    public Command Remove { get; } = 
        new(pos => parent.Items.Remove((PositionViewModel)pos!));
    public Command EditIndustry { get; } = 
        new(pos => new IndustryEditor((PositionViewModel)pos!).ShowDialog());
    public Command EditCodes { get; } = 
        new(pos => new ProductCodeEditor((PositionViewModel)pos!).ShowDialog());
}
