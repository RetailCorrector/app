using Microsoft.EntityFrameworkCore;
using RetailCorrector.Storage;
using RetailCorrector.Utils;
using System.Data;
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

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var conn = StorageContext.Instance.Database.GetDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = _queryText;
            cmd.CommandType = CommandType.Text;
            var table = new DataTable();
            using var reader = cmd.ExecuteReader();
            table.Load(reader);
            Table = table.Locale();
        }
    }
}