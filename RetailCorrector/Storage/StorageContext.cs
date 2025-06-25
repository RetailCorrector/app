using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace RetailCorrector.Storage
{
    public class StorageContext: DbContext
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
