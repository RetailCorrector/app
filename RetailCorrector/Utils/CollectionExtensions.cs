namespace RetailCorrector.Utils
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
    }
}
