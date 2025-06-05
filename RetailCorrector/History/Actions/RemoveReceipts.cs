namespace RetailCorrector.History.Actions
{
    public readonly struct RemoveReceipts(int[] indexes) : IHistoryAction
    {
        public string DisplayName { get; } = $"Удаление {indexes.Length} чека(ов)";
        private readonly (int, Receipt)[] _data = InitData(indexes);

        public void Undo()
        {
            foreach (var (index, receipt) in _data)
                Env.Receipts.Insert(index, receipt);
        }

        public void Redo()
        {
            foreach (var index in indexes.Reverse())
                Env.Receipts.RemoveAt(index);
        }

        private static (int, Receipt)[] InitData(int[] indexes)
        {
            var res = new (int, Receipt)[indexes.Length];
            for (var i = 0; i < indexes.Length; i++)
            {
                var index = indexes[indexes.Length - i - 1];
                res[i] = (index, Env.Receipts[index]);
            }
            return res;
        }
    }
}
