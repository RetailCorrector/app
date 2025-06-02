using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct AddReceipts(params Receipt[] receipts) : IHistoryAction
    {
        public string DisplayName => $"Добавление {receipts.Length} чека(ов) в позицию";
        private readonly int index = WizardDataContext.Receipts.Count;

        public void Redo()
        {
            foreach (var receipt in receipts)
                WizardDataContext.Receipts.Add(receipt);
        }

        public void Undo()
        {
            for(var _ = 0; _ < receipts.Length; _++)
                WizardDataContext.Receipts.RemoveAt(index);
        }
    }
}
