using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Otherworld
{
    public static class EnumerableHelper
    {
        public static T Sum<T> (this IEnumerable<T> self, T seed, Func<T, T, T> func)
        {
            T result = seed;
            foreach (var item in self)
                seed = func (seed, item);

            return result;
        }

        public static IEnumerable<T> Append<T> (this IEnumerable<T> xs, IEnumerable<T> ys)
        {
            foreach (var x in xs) {
                yield return x;
            }
            foreach (var y in ys) {
                yield return y;
            }
        }

        public static IEnumerable<T> Singleton<T> (T value)
        {
            yield return value;
        }

        public static T Random<T> (this IEnumerable<T> xs, Random rand)
        {
            T result = default (T);

            if (xs is IList<T>)
            {
                IList<T> list = (IList<T>)xs;
                if (list.Count > 0)
                {
                    int index = rand.Next (0, list.Count);
                    result = list[index];
                }
            }
            else
            {
                int count = xs.Count ();
                if (count > 0)
                    result = xs.Skip (rand.Next (0, count)).First ();
            }

            return result;
        }
    }
}
