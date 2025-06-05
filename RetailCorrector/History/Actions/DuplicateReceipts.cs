namespace RetailCorrector.History.Actions
{
    public readonly struct DuplicateReceipts(int[] indexes) : IHistoryAction
    {
        public string DisplayName => $"Дублирование {indexes.Length} чека(ов)";

        public void Redo()
        {
            foreach(var index in indexes.Reverse())
            {
                var receipt = Env.Receipts[index].Clone();
                Env.Receipts.Insert(index, receipt);
            }
        }

        public void Undo()
        {
            foreach (var index in indexes)
                Env.Receipts.RemoveAt(index);
        }
    }
}
