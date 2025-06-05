namespace RetailCorrector.Wizard.Extensions
{
    public static class CollectionExtensions
    {
        public static List<int> FindAllIndex<T>(this IList<T> values, Predicate<T> filter)
        {
            var indexes = new List<int>();
            for(var i = 0; i < values.Count; i++)
                if (filter(values[i]))
                    indexes.Add(i);
            return indexes;
        }

        public static T? Pop<T>(this ICollection<T> coll)
            where T : class
        {
            if (coll.Count == 0) return null!;
            var item = coll.Last();
            coll.Remove(item);
            return item;
        }
    }
}
