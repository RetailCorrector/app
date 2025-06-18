using Microsoft.EntityFrameworkCore;

namespace RetailCorrector.Storage
{
    public class StorageContext: DbContext
    {
        public DbSet<Models.Receipt> Receipts { get; set; }

        public StorageContext() {
            SQLitePCL.Batteries.Init();
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite("Data Source=test.db");//:memory:
    }
}
