using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace RetailCorrector.Storage
{
    public class StorageContext: DbContext
    {
        public DbSet<Models.Receipt> Receipts { get; set; }

        private readonly SqliteConnection connection;

        public StorageContext()
        {
            SQLitePCL.Batteries.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            Database.EnsureCreated();
            connection = new SqliteConnection("Data Source=:memory:");
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
