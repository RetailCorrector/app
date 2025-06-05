using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct DuplicateReceipts(int[] indexes) : IHistoryAction
    {
        public string DisplayName => $"Дублирование {indexes.Length} чека(ов)";

        public void Redo()
        {
            foreach(var index in indexes.Reverse())
            {
                var receipt = WizardDataContext.Receipts[index].Clone();
                WizardDataContext.Receipts.Insert(index, receipt);
            }
        }

        public void Undo()
        {
            foreach (var index in indexes)
                WizardDataContext.Receipts.RemoveAt(index);
        }
    }
}
