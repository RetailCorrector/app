using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RetailCorrector.Utils;

namespace RetailCorrector.Editor.Receipt;

public class ReceiptWizardContext : INotifyPropertyChanged
{
    public CorrType CorrType
    {
        get => _corrType;
        set
        {
            _corrType = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private CorrType _corrType = CorrType.ByYourself;

    public string Act
    {
        get => _act;
        set
        {
            _act = value;
            OnPropertyChanged(nameof(Done));
            OnPropertyChanged();
        }
    }
    private string _act = " ";

    public Operation Operation
    {
        get => _operation;
        set
        {
            _operation = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private Operation _operation = Operation.Income;

    public string Fiscal
    {
        get => _fiscal;
        set
        {
            _fiscal = value;
            OnPropertyChanged(nameof(Done));
            OnPropertyChanged();
        }
    }
    private string _fiscal = "";

    public DateTime? Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged(nameof(Done));
            OnPropertyChanged();
        }
    }
    private DateTime? _date = null;

    public bool RoundedTotal
    {
        get => _roundedTotal;
        set
        {
            _roundedTotal = value;
            OnPropertyChanged(nameof(Done));
            OnPropertyChanged();
        }
    }
    private bool _roundedTotal = false;

    public ObservableCollection<Position> Items { get; set; } = [];

    public ReceiptWizardContext()
    {
        Items.CollectionChanged += (_, _) =>
            OnPropertyChanged(nameof(Done));
    }
    
    public ReceiptWizardContext(RetailCorrector.Receipt data) : this()
    {
        CorrType = data.CorrectionType;
        Act = data.ActNumber ?? " ";
        Operation = data.Operation;
        Fiscal = data.FiscalSign;
        Date = data.Created;
        Cash = data.Payment.Cash / 100.0;
        ECash = data.Payment.ECash / 100.0;
        Pre = data.Payment.Pre / 100.0;
        Post = data.Payment.Post / 100.0;
        Provision = data.Payment.Provision / 100.0;
        RoundedTotal = data.TotalSum != data.Items.Sum(i => i.TotalSum);
        foreach (var pos in data.Items)
            Items.Add(new Position(this, pos));
    }

    public double Cash
    {
        get => _cash;
        set
        {
            _cash = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private double _cash = 0;

    public double ECash
    {
        get => _ecash;
        set
        {
            _ecash = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private double _ecash = 0;

    public double Pre
    {
        get => _pre;
        set
        {
            _pre = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private double _pre = 0;

    public double Post
    {
        get => _post;
        set
        {
            _post = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private double _post = 0;

    public double Provision
    {
        get => _provision;
        set
        {
            _provision = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Done));
        }
    }
    private double _provision = 0;

    public bool Done =>
        Items.Count > 0 && Items.All(i => i.Done) &&
        Total == Pre + Cash + ECash + Post + Provision &&
        Date is not null;

    public double Total => RoundedTotal ? 
        Math.Floor(Items.Sum(i => i.Sum)) : 
        Math.Round(Items.Sum(i => i.Sum), 2);

    public class Position(ReceiptWizardContext parent) : INotifyPropertyChanged
    {
        public Position(ReceiptWizardContext parent, RetailCorrector.Position data) : this(parent)
        {
            Name = data.Name;
            Type = data.PosType;
            Pay = data.PayType;
            Tax = data.TaxRate;
            Measure = data.MeasureUnit;
            Price = data.Price / 100.0;
            Quantity = data.Quantity / 1000.0;
            Sum = data.TotalSum / 100.0;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private string _name = "";

        public PositionType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private PositionType _type = PositionType.Product;

        public PaymentType Pay
        {
            get => _pay;
            set
            {
                _pay = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private PaymentType _pay = PaymentType.FullPayment;

        public TaxRate Tax
        {
            get => _tax;
            set
            {
                _tax = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private TaxRate _tax = TaxRate.Tax10;

        public MeasureUnit Measure
        {
            get => _measure;
            set
            {
                _measure = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private MeasureUnit _measure = MeasureUnit.Default;

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
                parent.OnPropertyChanged(nameof(Done));
            }
        }
        private double _price = 0;

        public double Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                parent.OnPropertyChanged(nameof(Done));
                OnPropertyChanged();
            }
        }
        private double _quantity = 1;

        public double Sum
        {
            get => _sum;
            set
            {
                _sum = value;
                parent.OnPropertyChanged(nameof(Done));
                OnPropertyChanged();
            }
        }
        private double _sum = 0;

        public bool Done =>
            !string.IsNullOrWhiteSpace(Name) && 
            Price > 0 && Quantity > 0 && 
            Sum == Math.Round(Price * Quantity, 2);

        private readonly Command _remove =
            new(pos => parent.Items.Remove((Position)pos!));
        public Command Remove => _remove;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string property = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
}
