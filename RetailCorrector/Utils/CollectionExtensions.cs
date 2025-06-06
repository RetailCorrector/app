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

        public static T GetValueOrDefault<T>(this IList<T> coll, Index index, T defaultValue) =>
            coll.Count <= index.Value ? defaultValue : coll[index];

        public static bool TryFirstPop<T>(this List<T> coll, Predicate<T> cond, out T? value)
            where T : struct
        {
            var index = coll.FindIndex(cond);
            if (index == -1)
            {
                value = null;
                return false;
            }
            value = coll[index];
            coll.RemoveAt(index);
            return true;
        }

    }
}
