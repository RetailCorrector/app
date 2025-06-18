using RetailCorrector.Editor.Receipt.ViewModels;
using System.Collections.ObjectModel;

namespace RetailCorrector.Editor.Receipt;

public partial class ReceiptViewModel
{
    public ObservableCollection<PositionViewModel> Items { get; set; } = [];

    [NotifyUpdated(nameof(Done))] private CorrType _corrType = CorrType.ByYourself;
    [NotifyUpdated(nameof(Done))] private string _act = " ";
    [NotifyUpdated(nameof(Done))] private Operation _operation = Operation.Income;
    [NotifyUpdated(nameof(Done))] private string _fiscal = "";
    [NotifyUpdated(nameof(Done))] private DateTime? _date = null;
    [NotifyUpdated(nameof(Done))] private bool _roundedTotal = false;
    [NotifyUpdated(nameof(Done))] private double _cash = 0;
    [NotifyUpdated(nameof(Done))] private double _ecash = 0;
    [NotifyUpdated(nameof(Done))] private double _pre = 0;
    [NotifyUpdated(nameof(Done))] private double _post = 0;
    [NotifyUpdated(nameof(Done))] private double _provision = 0;

    public ReceiptViewModel()
    {
        Items.CollectionChanged += (_, _) =>
            OnPropertyChanged(nameof(Done));
    }
    
    public ReceiptViewModel(RetailCorrector.Receipt data) : this()
    {
        CorrType = data.CorrectionType;
        Act = data.ActNumber ?? " ";
        Operation = data.Operation;
        Fiscal = data.FiscalSign;
        Date = data.Created;
        Cash = data.Payment.Cash / 100.0;
        Ecash = data.Payment.ECash / 100.0;
        Pre = data.Payment.Pre / 100.0;
        Post = data.Payment.Post / 100.0;
        Provision = data.Payment.Provision / 100.0;
        RoundedTotal = data.TotalSum != data.Items.Sum(i => i.TotalSum);
        foreach (var pos in data.Items)
            Items.Add(new PositionViewModel(this, pos));
    }

    public bool Done =>
        Items.Count > 0 && Items.All(i => i.Done) &&
        Total == Pre + Cash + Ecash + Post + Provision &&
        Date is not null;

    public double Total => RoundedTotal ? 
        Math.Floor(Items.Sum(i => i.Sum)) : 
        Math.Round(Items.Sum(i => i.Sum), 2);
}
