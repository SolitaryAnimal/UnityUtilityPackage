using System.Collections;
using System.Collections.Generic;

namespace Sanimal
{
    public static class CollectionUtility
    {
        public static void AddRange<T>(this ISet<T> dictionary, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                dictionary.Add(item);
        }
    }
}