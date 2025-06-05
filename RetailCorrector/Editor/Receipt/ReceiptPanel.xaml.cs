using System.Printing;
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
