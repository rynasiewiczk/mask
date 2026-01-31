namespace LazySloth.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable == null || !enumerable.Any<T>();

        public static T FirstOrDefault<T>(this IEnumerable<T> source, T alternate) => source.DefaultIfEmpty<T>(alternate).First<T>();

        public static int IndexOf<T>(this IEnumerable<T> list, T item) => list.Select<T, int>((Func<T, int, int>) ((x, index) => !EqualityComparer<T>.Default.Equals(item, x) ? -1 : index))
            .FirstOrDefault<int>((Func<int, bool>) (x => x != -1), -1);
        
        /// <summary>Perform an action on each item.</summary>
        /// <param name="source">The source.</param>
        /// <param name="action">The action to perform.</param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
            return source;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            var queue = new Queue<T>();
            foreach (var e in source)
            {
                queue.Enqueue(e);
            }

            return queue;
        }

        public static IEnumerable<T> MyAppend<T>(this IEnumerable<T> list, T item)
        {
            foreach (T obj in list)
            {
                T element = obj;
                yield return element;
                element = default(T);
            }

            yield return item;
        }


        public static IEnumerable<T> MyPrepend<T>(this IEnumerable<T> values, T value)
        {
            yield return value;
            foreach (T obj in values)
            {
                T item = obj;
                yield return item;
                item = default(T);
            }
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count = 1)
        {
            IEnumerator<T> enumerator = source.GetEnumerator();
            Queue<T> queue = new Queue<T>(count + 1);
            while (true)
            {
                if (enumerator.MoveNext())
                {
                    queue.Enqueue(enumerator.Current);
                    if (queue.Count > count)
                        yield return queue.Dequeue();
                }
                else
                    break;
            }
        }

        public static IEnumerable<TSource> SkipUntil<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            using (IEnumerator<TSource> iterator = source.GetEnumerator())
            {
#pragma warning disable 642
                do
                    ;
                while (iterator.MoveNext() && !predicate(iterator.Current));
#pragma warning restore 642
                while (iterator.MoveNext())
                    yield return iterator.Current;
            }
        }

        public static IEnumerable<T> Yield<T>(this T item)
        {
            if ((object) (T) item != null)
                yield return item;
        }

        public static void InsertRelative<T>(
            this IList<T> source,
            IList<T> reference,
            params T[] items)
        {
            foreach (T obj in items)
            {
                int num = reference.IndexOf(obj);
                int index = 0;
                while (index < source.Count<T>() && (num < 0 || num >= reference.IndexOf(source[index])))
                    ++index;
                source.Insert(index, obj);
            }
        }

        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TValue> defaultValueProvider)
        {
            TValue obj;
            return dictionary.TryGetValue(key, out obj) ? obj : defaultValueProvider();
        }

        public static bool ScrambledEquals<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            if (list2 == null || list1.Count<T>() != list2.Count<T>())
                return false;
            Dictionary<T, int> dictionary = new Dictionary<T, int>();
            foreach (T key in list1)
            {
                if (dictionary.ContainsKey(key))
                    dictionary[key]++;
                else
                    dictionary.Add(key, 1);
            }

            foreach (T key in list2)
            {
                if (!dictionary.ContainsKey(key))
                    return false;
                dictionary[key]--;
            }

            return dictionary.Values.All<int>((Func<int, bool>) (c => c == 0));
        }

        public static T FirstOrDefault<T>(
            this IEnumerable<T> source,
            Func<T, bool> predicate,
            T alternate)
        {
            return source.Where<T>(predicate).FirstOrDefault<T>(alternate);
        }
    }
}