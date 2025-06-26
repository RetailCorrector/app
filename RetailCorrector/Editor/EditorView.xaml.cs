using RetailCorrector.Storage;
using RetailCorrector.Utils;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace RetailCorrector.Editor
{
    public partial class EditorView : UserControl
    {
        [NotifyUpdated] private string _queryText = "";
        [NotifyUpdated] private DataTable _table = new();
        public static EditorView Instance { get; private set; }

        public EditorView()
        {
            Instance = this;
            InitializeComponent();
            StorageContext.Init();
        }

        private void Run(object sender, RoutedEventArgs e) =>
            Table = StorageContext.Instance.ExecuteSQL(QueryText)?.Locale();
        }
    }