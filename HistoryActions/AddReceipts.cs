using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct AddReceipts(int index, int count) : IHistoryAction
    {
        public string DisplayName => $"Добавление {count} чека(ов) в позицию";
        public void Undo()
        {
            for(var _ = 0; _ < count; _++)
                WizardDataContext.Receipts.RemoveAt(index);
        }
    }
}
