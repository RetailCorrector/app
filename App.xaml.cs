using System.Windows;

namespace RetailCorrector.Wizard
{
    public partial class App : Application
    {
        public static ReceiptCollection Receipts { get; } = new ReceiptCollection();
    }
}
