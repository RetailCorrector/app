using RetailCorrector.Editor.Receipt.ViewModels;
using System.Collections.ObjectModel;

namespace RetailCorrector.Editor.Receipt;

public partial class ReceiptViewModel
{
    public ObservableCollection<PositionViewModel> Items { get; set; } = [];
    public ObservableCollection<IndustryViewModel> Industry { get; set; } = [];

    [NotifyChanged(nameof(Done))] private CorrType _corrType = CorrType.ByYourself;
    [NotifyChanged(nameof(Done))] private string _act = " ";
    [NotifyChanged(nameof(Done))] private Operation _operation = Operation.Income;
    [NotifyChanged(nameof(Done))] private string _fiscal = "";
    [NotifyChanged(nameof(Done))] private DateTime? _date = null;
    [NotifyChanged(nameof(Done))] private bool _roundedTotal = false;
    [NotifyChanged(nameof(Done))] private double _cash = 0;
    [NotifyChanged(nameof(Done))] private double _ecash = 0;
    [NotifyChanged(nameof(Done))] private double _pre = 0;
    [NotifyChanged(nameof(Done))] private double _post = 0;
    [NotifyChanged(nameof(Done))] private double _provision = 0;

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
        foreach (var i in data.IndustryData ?? [])
            Industry.Add(i);
    }

    public bool Done =>
        Items.Count > 0 && Items.All(i => i.Done) &&
        Total == Pre + Cash + Ecash + Post + Provision &&
        Date is not null;

    public double Total => RoundedTotal ? 
        Math.Floor(Items.Sum(i => i.Sum)) : 
        Math.Round(Items.Sum(i => i.Sum), 2);
}
