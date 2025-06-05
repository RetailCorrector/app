namespace RetailCorrector.Wizard
{
    public interface IHistoryAction
    {
        string DisplayName { get; }
        void Undo();
        void Redo();
    }
}
