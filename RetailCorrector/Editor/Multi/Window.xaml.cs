using System.Collections.ObjectModel;
using System.Windows;

namespace RetailCorrector.Editor.Multi
{
    public partial class Window : System.Windows.Window
    {
        public Visibility EditVisibility => TypeId == 1 ? 0 : Visibility.Collapsed;
        public bool EditIsEnabled => _selectedEditId > -1;
        public bool FilterIsEnabled => _selectedFilterId > -1;
        public bool ApplyIsEnabled => EditorItems.Any();
        public string[] CurrentProperties => _targetId == 1 ? PositionProperties : ReceiptProperties;

        public ObservableCollection<FilterItem> FilterItems { get; } = [];
        public ObservableCollection<EditorItem> EditorItems { get; } = [];

        [NotifyUpdated(nameof(EditVisibility))] private int _typeId = 1;
        [NotifyUpdated] private int _targetId;
        [NotifyUpdated(nameof(FilterIsEnabled))] private int _selectedFilterId;
        [NotifyUpdated(nameof(EditIsEnabled))] private int _selectedEditId;

        private static readonly string[] ReceiptProperties = [
            "Тип",
            "Итог",
            "Наличная",
            "Безналичная",
            "Предоплата",
            "Постоплата",
            "ВП",
            ];

        private static readonly string[] PositionProperties = [
            "Название",
            "Цена",
            "Количество",
            "Стоимость",
            "ЕИ",
            "Тип",
            "Оплата",
            "НДС",
            ];

        public Window()
        {
            EditorItems.CollectionChanged += (_, _) => 
                OnPropertyChanged(nameof(ApplyIsEnabled));
            PropertyChanged += (_, e) =>
            {
                if(e.PropertyName == nameof(TargetId))
                {
                    FilterItems.Clear();
                    EditorItems.Clear();
                }
                if(e.PropertyName == nameof(TypeId))
                {
                    EditorItems.Clear();
                }
            };
            InitializeComponent();
        }

        public void AddFilter(object? s, RoutedEventArgs e)
        {
            FilterItems.Add(new FilterItem(FilterItems.Count + 1, CurrentProperties));
        }

        public void RemoveFilter(object? s, RoutedEventArgs e)
        {
            for (var i = _selectedFilterId; i < FilterItems.Count; i++)
                FilterItems[i].Number--;
            FilterItems.RemoveAt(_selectedFilterId);            
        }

        public void AddEdit(object? s, RoutedEventArgs e)
        {
            EditorItems.Add(new EditorItem(CurrentProperties));
        }

        public void RemoveEdit(object? s, RoutedEventArgs e)
        {
            EditorItems.RemoveAt(_selectedEditId);
        }
    }
}
