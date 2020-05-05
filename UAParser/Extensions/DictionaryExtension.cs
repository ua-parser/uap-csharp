using System;
using System.Collections.Generic;

namespace UAParser
{
    internal static class DictionaryExtension
    {
        public static TValue Find<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
            return dictionary.TryGetValue(key, out var result) ? result : default;
        }
    }
}
