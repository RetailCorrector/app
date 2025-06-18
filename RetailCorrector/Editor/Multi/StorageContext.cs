using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace RetailCorrector.Editor.Multi
{
    public class StorageContext: DbContext
    {
        public DbSet<Models.Receipt> Receipts { get; set; }

        private SqliteConnection connection;

        public StorageContext()
        {
            SQLitePCL.Batteries.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            connection = new SqliteConnection("Data Source=:memory:");
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
