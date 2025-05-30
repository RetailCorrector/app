using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.Extensions;
using System.ComponentModel;
using System.Windows;

namespace RetailCorrector.Wizard.Windows
{
    public partial class ReceiptWizard : Window, INotifyPropertyChanged
    {
        public ReceiptWizardContext? Context { get; init; }
        public KeyValuePair<Operation, string>[] Operations { get; init; } = EnumExtensions.GetDisplayNames<Operation>();
        public Receipt Data { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReceiptWizard()
        {
            Context = new();
            InitializeComponent();
        }

        public ReceiptWizard(Receipt receipt)
        {
            Context = new(receipt);
            InitializeComponent();
        }

        public void AddPosition(object? s, RoutedEventArgs e) =>
            Context!.Items.Add(new ReceiptWizardContext.Position(Context));

        public void Save(object? s, RoutedEventArgs e)
        {
            Data = new Receipt
            {
                ActNumber = " ",
                CorrectionType = CorrType.ByYourself,
                Created = (DateTime)Context!.Date!,
                FiscalSign = Context.Fiscal,
                Operation = Context.Operation,
                RoundedSum = (uint)Math.Round(Context.Total * 100),
                Payment = new Payment
                {
                    Cash = (uint)Math.Round(Context.Cash * 100),
                    ECash = (uint)Math.Round(Context.ECash * 100),
                    Post = (uint)Math.Round(Context.Post * 100),
                    Pre = (uint)Math.Round(Context.Pre * 100),
                    Provision = (uint)Math.Round(Context.Provision * 100)
                },
                Items = [..Context.Items.Select(i => new Position
                {
                    Name = i.Name,
                    Price = (uint)Math.Round(i.Price * 100),
                    Quantity = (uint)Math.Round(i.Quantity* 1000),
                    TaxRate = i.Tax,
                    MeasureUnit = i.Measure,
                    PayType = i.Pay,
                    PosType = i.Type,
                    TotalSum = (uint)Math.Round(i.Sum * 100)
                })]
            };
            DialogResult = true;
            Close();
        }
    }
}
