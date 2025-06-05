namespace RetailCorrector.History.Actions
{
    public readonly struct EditReceipts(int index, Receipt edited) : IHistoryAction
    {
        public string DisplayName => "Редактирование чека";
        private readonly Receipt original = Env.Receipts[index];

        public void Undo() =>
            Env.Receipts[index] = original;

        public void Redo() =>
            Env.Receipts[index] = edited;
    }
}
