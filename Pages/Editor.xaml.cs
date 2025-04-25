using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetailCorrector.Wizard.Pages
{
    public partial class Editor : Grid
    {
        public ObservableCollection<EditorItem> EditorItems { get; } = [];
        private EditorItem[] EditorReceipts => [.. EditorItems.Where(e => e.DataTypeId == 0)];
        private EditorItem[] EditorPositions => [.. EditorItems.Where(e => e.DataTypeId == 1)];

        public void RefreshPreview()
        {
            App.Receipts.Edited.Clear();
            foreach (var rec in App.Receipts.Filtered.Where(r => r.Item1).Select(r => r.Item2))
                App.Receipts.Edited.Add(Edit(rec));
        }

        private Receipt Edit(Receipt origin)
        {
            var res = origin.Clone();
            foreach(var e in EditorReceipts)
                if (e.Check(origin)) 
                    e.Edit(ref res);
            for(var i = 0; i < res.Items.Length; i++)
            {
                var pos = res.Items[i].Clone();
                foreach(var e in EditorPositions)
                    if (e.Check(res.Items[i]))
                        e.Edit(ref pos);
                res.Items[i] = pos;
            }
            return res;
        }

        public Editor() => InitializeComponent();
        private void Add(object sender, RoutedEventArgs e)
        {
            var item = new EditorItem();
            item.PropertyChanged += (_,_) => RefreshPreview();
            EditorItems.Add(item);
            RefreshPreview();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem i || i.Parent is not ContextMenu c || c.PlacementTarget is not DataGrid d || d.SelectedIndex == -1) return;
            EditorItems.RemoveAt(d.SelectedIndex);
            RefreshPreview();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => RefreshPreview();
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                preview.SelectedIndex = -1;
            base.OnKeyDown(e);
        }
    }
}
