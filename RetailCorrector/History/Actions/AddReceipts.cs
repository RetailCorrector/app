namespace RetailCorrector.History.Actions
{
    public readonly struct AddReceipts(params Receipt[] receipts) : IHistoryAction
    {
        public string DisplayName => $"Добавление {receipts.Length} чека(ов) в позицию";
        private readonly int index = Env.Receipts.Count;

        public void Redo()
        {
            foreach (var receipt in receipts)
                Env.Receipts.Add(receipt);
        }

        public void Undo()
        {
            for(var _ = 0; _ < receipts.Length; _++)
                Env.Receipts.RemoveAt(index);
        }
    }
}
