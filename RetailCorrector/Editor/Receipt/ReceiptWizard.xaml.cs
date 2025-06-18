using RetailCorrector.Utils;
using System.Windows;
using System.Windows.Input;

namespace RetailCorrector.Editor.Receipt
{
    public partial class ReceiptWizard : Window
    {
        public ReceiptViewModel Context { get; init; } = null!;
        public RetailCorrector.Receipt Data { get; private set; }

        public bool IsCreate { get; }

        public ReceiptWizard()
        {
            CommandBindings.Add(new CommandBinding(Commands.ExitDialog, (_, _) => Close()));
            IsCreate = true;
            Context = new();
            InitializeComponent();
        }

        public ReceiptWizard(RetailCorrector.Receipt receipt)
        {
            CommandBindings.Add(new CommandBinding(Commands.ExitDialog, (_, _) => Close()));
            IsCreate = false;
            Context = new(receipt);
            InitializeComponent();
        }

        public void AddPosition(object? s, RoutedEventArgs e) =>
            Context.Items.Add(new PositionViewModel(Context));

        public void ShowReceiptIndustry(object? s, RoutedEventArgs e) =>
            new IndustryEditor(Context).ShowDialog();

        public void Save(object? s, RoutedEventArgs e)
        {
            Data = new RetailCorrector.Receipt
            {
                Created = (DateTime)Context.Date!,
                ActNumber = string.IsNullOrWhiteSpace(Context.Act) ? null : Context.Act,
                CorrectionType = Context.CorrType,
                FiscalSign = Context.Fiscal,
                Operation = Context.Operation,
                TotalSum = (uint)Math.Round(Context.Total * 100),
                Payment = new Payment
                {
                    Cash = (uint)Math.Round(Context.Cash * 100),
                    ECash = (uint)Math.Round(Context.Ecash * 100),
                    Post = (uint)Math.Round(Context.Post * 100),
                    Pre = (uint)Math.Round(Context.Pre * 100),
                    Provision = (uint)Math.Round(Context.Provision * 100)
                },
                IndustryData = [..Context.Industry],
                Items = [..Context.Items.Select(i => new Position
                {
                    Name = i.Name,
                    Price = (uint)Math.Round(i.Price * 100),
                    Quantity = (uint)Math.Round(i.Quantity* 1000),
                    TaxRate = i.Tax,
                    MeasureUnit = i.Measure,
                    PayType = i.Pay,
                    PosType = i.Type,
                    TotalSum = (uint)Math.Round(i.Sum * 100),
                    IndustryData = [..i.Industry],
                    Codes = [],
                })]
            };
            DialogResult = true;
            Close();
        }
    }
}
