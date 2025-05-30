using RetailCorrector.Wizard.Contexts;
using RetailCorrector.Wizard.Extensions;
using RetailCorrector.Wizard.HistoryActions;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Wizard.UserControls
{
    public partial class ReceiptPanel : ItemsControl
    {
        public ReceiptPanel()
        {
            InitializeComponent();
        }

        private ReceiptView[] Views
        {
            get
            {
                var rawItems = new Receipt[Items.Count];
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

        public void Delete()
        {
            var items = Views;
            var indexes = items.FindAllIndex(i => i.IsSelected);
            WizardDataContext.History.Push(new RemoveReceipts([.. indexes]));
            indexes.Reverse();
            foreach (var index in indexes)
                WizardDataContext.Receipts.RemoveAt(index);
        }

        public void InvertSelect()
        {
            var items = Views;
            var indexes = items.FindAllIndex(i => i.IsSelected);
            foreach(var i in items.Where(i => !i.IsSelected))
                i.IsSelected = true;
            foreach(var i in indexes) items[i].IsSelected = false;
        }
    }
}
