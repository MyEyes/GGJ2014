using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vest.utilities
{
    public static class CollectionHelpers
    {
        /// <summary>
        /// Looks up a value in a dictionary, or inserts the
        /// value if the key does not exist.
        /// </summary>
        /// <param name="valueFactory">
        /// Called if <paramref name="key"/> does not exist
        /// in <paramref name="dictionary"/>.  Returns the
        /// value to be inserted into
        /// <paramref name="dictionary"/>.
        /// </param>
        public static TValue GetOrSet<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            TValue value;
            if (dictionary.TryGetValue (key, out value))
                return value;

            value = valueFactory();
            dictionary[key] = value;
            return value;
        }

        /// <summary>
        /// Insert a value to a dictionary, updating it if a
        /// value already exists.
        /// </summary>
        public static void InsertWith<TKey, TValue> (
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value,
            Func<TValue, TValue, TValue> join)
        {
            TValue existingValue;
            if (dictionary.TryGetValue (key, out existingValue))
            {
                dictionary[key] = join(existingValue, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        public static HashSet<T> Intersect<T> (HashSet<T> xs, HashSet<T> ys)
        {
            // FIXME(strager): This isn't efficient because
            // the BCL HashSet doesn't have nice intersection
            // operations.
            var result = new HashSet<T>(xs);
            result.IntersectWith (ys);
            return result;
        }


        public static TValue GetValue<TKey, TValue> (
            this ConditionalWeakTable<TKey, TValue> table, TKey key) where TKey : class where TValue : class
        {
            TValue value;
            if (!table.TryGetValue (key, out value))
                throw new ArgumentOutOfRangeException ("key");
            return value;
        }
    }
}
