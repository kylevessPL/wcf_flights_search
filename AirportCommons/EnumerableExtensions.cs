using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportCommons
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _();

            IEnumerable<TSource> _()
            {
                foreach (var item in source)
                {
                    yield return item;
                    if (predicate(item))
                        yield break;
                }
            }
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
        {
            return self.Select((item, index) => (item, index));
        }

        public static IEnumerable<TSource> IfEmpty<TSource>(this IEnumerable<TSource> source,
            Action<IEnumerable<TSource>> action)
        {
            var list = source.ToList();
            if (!list.Any()) action(list);

            return list;
        }
    }
}