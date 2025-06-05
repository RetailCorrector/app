namespace RetailCorrector.History.Actions
{
    public readonly struct InvertOperation(int[] indexes) : IHistoryAction
    {
        public string DisplayName => $"Инвертирование типа {indexes.Length} чека(ов)";
        public void Undo()
        {
            foreach (var index in indexes)
            {
                var receipt = Env.Receipts[index];
                receipt.Operation = (int)receipt.Operation % 2 == 0 ? receipt.Operation - 1 : receipt.Operation + 1;
                Env.Receipts[index] = receipt;
            }
        }

        public void Redo() => Undo();
    }
}
