using Microsoft.EntityFrameworkCore;
using RetailCorrector.Storage;

namespace RetailCorrector.History.Actions
{
    public readonly struct MultiEditReceipts(string query) : IHistoryAction
    {
        public string DisplayName => "Массовое редактирование чеков";
        private readonly Receipt[] original = [.. Env.Receipts];

        public void Undo()
        {
            Env.Receipts.Clear();
            foreach (var r in original)
                Env.Receipts.Add(r);
        }

        public void Redo()
        {
            using var db = new StorageContext();
            db.Receipts.AddRange([.. Env.Receipts]);
            db.SaveChanges();
            db.Database.ExecuteSqlRaw(query);
            db.ChangeTracker.Clear();
            Env.Receipts.Clear();
            foreach (var r in db.Receipts.Include(r => r.Items).Include(r => r.IndustryData).AsNoTracking())
                Env.Receipts.Add(r);
        }
    }
}
