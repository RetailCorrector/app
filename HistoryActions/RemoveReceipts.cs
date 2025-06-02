using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct RemoveReceipts : IHistoryAction
    {
        public string DisplayName { get; }
        private readonly (int, Receipt)[] _data;

        public RemoveReceipts(int[] indexes)
        {
            DisplayName = $"Удаление {indexes.Length} чека(ов)";
            indexes = [.. indexes.OrderByDescending(i => i)];
            _data = new (int, Receipt)[indexes.Length];
            for (var i = 0; i < indexes.Length; i++)
            {
                var index = indexes[indexes.Length - i - 1];
                _data[i] = (index, WizardDataContext.Receipts[index]);
            }
        }

        public void Undo()
        {
            foreach (var (index, receipt) in _data)
                WizardDataContext.Receipts.Insert(index, receipt);
        }
    }
}
