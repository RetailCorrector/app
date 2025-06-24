using System.Collections.ObjectModel;

namespace RetailCorrector.History
{
    public static class HistoryController
    {
        public static ObservableCollection<HistoryView> DisplayItems { get; } = [];
        private static int _currentIndex = -1;
        private static Item? _begin = null;
        private static Item? _current = null;

        public static void Undo()
        {
            if (_currentIndex == -1) return;
            Alert.Debug($"Отменяется действие: {_current!.Action.DisplayName}...");
            _current.Action.Undo();
            var _previous = _current.Previous;
            _current = _previous;
            DisplayItems[_currentIndex].SwitchDone(false);
            _currentIndex--;
        }

        public static void Redo()
        {
            if (_currentIndex == DisplayItems.Count - 1) return;
            var _next = (_current?.Next) ?? _begin;
            if (_next is null) return;
            Alert.Debug($"Выполняется действие: {_next.Action.DisplayName}...");
            _next.Action.Redo();
            _current = _next;
            _currentIndex++;
            DisplayItems[_currentIndex].SwitchDone(true);
        }

        public static void Add(IHistoryAction action)
        {
            Alert.Debug($"Выполняется действие: {action.DisplayName}...");
            action.Redo();
            if (_currentIndex == -1)
                _begin = _current = new Item(action);
            else
            {
                var curr = new Item(action) { Previous = _current };
                _current?.SetNext(curr);
                _current = curr;
            }
            _currentIndex++;
            while (DisplayItems.Count > _currentIndex)
                DisplayItems.RemoveAt(DisplayItems.Count - 1);
            DisplayItems.Add(new HistoryView(action));
        }

        public static void Clear()
        {
            Alert.Debug("Очистка истории...");
            _currentIndex = -1;
            _current = _begin = null;
            DisplayItems.Clear();
        }

        private class Item(IHistoryAction action)
        {
            public IHistoryAction Action { get; } = action;
            public Item? Previous { get; set; } = null;
            public Item? Next { get; private set; } = null;

            public void SetNext(Item next) => Next = next;
        }
    }
}
