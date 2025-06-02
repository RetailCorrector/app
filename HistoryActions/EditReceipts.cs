using RetailCorrector.Wizard.Contexts;

namespace RetailCorrector.Wizard.HistoryActions
{
    public readonly struct EditReceipts(int index) : IHistoryAction
    {
        public string DisplayName => "Редактирование чека";
        private readonly Receipt original = WizardDataContext.Receipts[index];

        public void Undo()
        {
            WizardDataContext.Receipts[index] = original;
        }
    }
}
