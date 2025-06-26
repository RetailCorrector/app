using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RetailCorrector.Storage
{
    public partial class StorageContext: DbContext
    {
        public DbSet<Models.Receipt> Receipts { get; set; }
        public static StorageContext Instance { get; private set; } = null!;

        private readonly SqliteConnection connection;

        public StorageContext()
        {
            SQLitePCL.Batteries.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            connection = new SqliteConnection("Data Source=:memory:");
            Database.EnsureCreated();
        }
        public static void Init() => Instance = new StorageContext();

        public DataTable? ExecuteSQL(string query)
        {
            try
            {
                var conn = Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                var table = new DataTable();
                using var reader = cmd.ExecuteReader();
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                Alert.Error("При выполнении запроса возникла ошибка...", ex);
                return null;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            connection.Open();
            optionsBuilder.UseSqlite(connection);
        }

        public override void Dispose()
        {
            connection.Dispose();
            base.Dispose();
        }
    }
}
