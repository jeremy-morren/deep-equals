namespace DeepEqualsGenerator.Framework;

public static class LinqHelpers
{
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, 
        Func<T, TKey> selector,
        IEqualityComparer<TKey> comparer)
    {
        var set = new HashSet<TKey>(comparer);
        foreach (var e in source)
            if (set.Add(selector(e)))
                yield return e;
    }
}