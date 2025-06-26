using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace RetailCorrector.Storage
{
    public partial class StorageContext: DbContext
    {
        public DbSet<Models.Receipt> Receipts { get; set; }
        private DbSet<Models.Position> Positions { get; set; }
        private DbSet<Models.PositionCode> PositionCodes { get; set; }
        private DbSet<Models.IndustryData> IndustryData { get; set; }

        public static StorageContext Instance { get; private set; } = null!;
        [NotifyUpdated] private bool _useSandbox = false;

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
                SqliteConnection conn;
                StorageContext ctx;
                if (UseSandbox)
                {
                    ctx = new StorageContext();
                    MigrateTo(ctx);
                    conn = ctx.connection;
                }
                else
                    conn = connection;
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

        private void MigrateTo(StorageContext other)
        {
            PositionCodes.ToList().ForEach(i => other.PositionCodes.Add(i));
            IndustryData.ToList().ForEach(i => other.IndustryData.Add(i));
            Positions.ToList().ForEach(i => other.Positions.Add(i));
            Receipts.ToList().ForEach(i => other.Receipts.Add(i));
            other.SaveChanges();
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
