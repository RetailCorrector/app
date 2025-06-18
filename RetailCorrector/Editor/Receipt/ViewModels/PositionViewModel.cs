using RetailCorrector.Editor.Receipt.ViewModels;
using RetailCorrector.Utils;
using System.Collections.ObjectModel;

namespace RetailCorrector.Editor.Receipt;

public partial class PositionViewModel(ReceiptViewModel parent)
{
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
    }

    [NotifyUpdated] private string _name = "";
    [NotifyUpdated] private PositionType _type = PositionType.Product;
    [NotifyUpdated] private PaymentType _pay = PaymentType.FullPayment;
    [NotifyUpdated] private TaxRate _tax = TaxRate.Tax10;
    [NotifyUpdated] private MeasureUnit _measure = MeasureUnit.Default;
    [NotifyUpdated] private double _price = 0;
    [NotifyUpdated] private double _quantity = 1;
    [NotifyUpdated] private double _sum = 0;

    public bool Done =>
        !string.IsNullOrWhiteSpace(Name) &&
        Price > 0 && Quantity > 0 &&
        Sum == Math.Round(Price * Quantity, 2);

    private readonly Command _remove =
        new(pos => parent.Items.Remove((PositionViewModel)pos!));
    public Command Remove => _remove;
}
