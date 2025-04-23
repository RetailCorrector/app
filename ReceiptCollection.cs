using System.Collections.ObjectModel;

namespace RetailCorrector.Wizard
{
    public class ReceiptCollection
    {
        public ObservableCollection<Receipt> Parsed { get; } = [];
        public ObservableCollection<Tuple<bool, Receipt>> Filtered { get; } = [];
        public ObservableCollection<Tuple<Receipt, Receipt>> Edited { get; } = [];
    }
}
