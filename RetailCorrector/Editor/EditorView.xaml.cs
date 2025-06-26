using RetailCorrector.Storage;
using RetailCorrector.Utils;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RetailCorrector.Editor
{
    public partial class EditorView : UserControl
    {
        [NotifyUpdated] private string _queryText = "";
        [NotifyUpdated] private DataTable _table = new();
        [NotifyUpdated] private Brush _btnSandboxBackground = Brushes.White;

        public static EditorView Instance { get; private set; }

        public EditorView()
        {
            Instance = this;
            InitializeComponent();
            StorageContext.Init();
        }

        private void ToggleSandbox(object sender, RoutedEventArgs e)
        {
            StorageContext.Instance.UseSandbox = !StorageContext.Instance.UseSandbox;
            BtnSandboxBackground  = StorageContext.Instance?.UseSandbox 
                ?? false ? Brushes.AntiqueWhite : Brushes.White;
        }

        private void Run(object sender, RoutedEventArgs e) =>
            Table = StorageContext.Instance.ExecuteSQL(QueryText)?.Locale();
    }
}