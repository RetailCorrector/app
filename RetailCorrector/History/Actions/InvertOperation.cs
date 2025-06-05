using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct InvertOperation(int[] indexes) : IHistoryAction
    {
        public string DisplayName => $"Инвертирование типа {indexes.Length} чека(ов)";
        public void Undo()
        {
            foreach (var index in indexes)
            {
                var receipt = WizardDataContext.Receipts[index];
                receipt.Operation = (int)receipt.Operation % 2 == 0 ? receipt.Operation - 1 : receipt.Operation + 1;
                WizardDataContext.Receipts[index] = receipt;
            }
        }

        public void Redo() => Undo();
    }
}
