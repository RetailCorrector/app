using System.Windows;
using System.Windows.Controls;
using RetailCorrector.History;
using RetailCorrector.History.Actions;
using RetailCorrector.Utils;

namespace RetailCorrector.Editor.Receipt
{
    public partial class ReceiptPanel : ItemsControl
    {
        private static ReceiptPanel _instance;
        private static Thickness _defaultMarginItems = new(5, 7, 5, 7);
        
        [NotifyUpdated] private Thickness _marginItems = _defaultMarginItems;

        public ReceiptPanel()
        {
            _instance = this;
            InitializeComponent();
        }

        private ReceiptView[] Views
        {
            get
            {
                var rawItems = new RetailCorrector.Receipt[Items.Count];
                var items = new ReceiptView[Items.Count];
                Items.CopyTo(rawItems, 0);
                for (var i = 0; i < Items.Count; i++)
                {
                    var container = (FrameworkElement)ItemContainerGenerator.ContainerFromItem(Items[i]);
                    if (container is ContentPresenter presenter)
                    {
                        presenter.ApplyTemplate();
                        items[i] = (ReceiptView)presenter.ContentTemplate.FindName("1_T", presenter)!;
                    }
                }
                return items;
            }
        }

        public static void UpdateMargin()
        {
            var count = (int)_instance.ActualWidth / 190;
            if (count > 0)
            {
                var lr = ((int)_instance.ActualWidth - count * 190) / count;
                _instance.MarginItems = new Thickness(lr / 2.0, 7, lr / 2.0, 7);
            }
            else _instance.MarginItems = _defaultMarginItems;
        }
        public static void Delete() =>
            HistoryController.Add(new RemoveReceipts([.. _instance.Views.FindAllIndex(i => i.IsSelected)]));

        public static void InvertOperation() =>
            HistoryController.Add(new InvertOperation([.. _instance.Views.FindAllIndex(i => i.IsSelected)]));

        public static void Duplicate() =>
            HistoryController.Add(new DuplicateReceipts([.. _instance.Views.FindAllIndex(i => i.IsSelected)]));

        public static void InvertSelect()
        {
            var items = _instance.Views;
            var indexes = items.FindAllIndex(i => i.IsSelected);
            foreach(var i in items.Where(i => !i.IsSelected))
                i.IsSelected = true;
            foreach(var i in indexes) items[i].IsSelected = false;
        }
    }
}
