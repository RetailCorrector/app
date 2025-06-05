namespace RetailCorrector.History
{
    public interface IHistoryAction
    {
        string DisplayName { get; }
        void Undo();
        void Redo();
    }
}
