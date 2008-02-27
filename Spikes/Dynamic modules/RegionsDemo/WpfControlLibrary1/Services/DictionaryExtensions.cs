using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherModule.Services
{
    public static class DictionaryExtensions
    {


        public static TValue GetItem<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : IEnumerable, new()
        {

            TValue newValue = new TValue();
            TValue value;
            bool Found = dictionary.TryGetValue(key, out value);

            if (!Found)
                value = newValue;

            return value;
        }

        public static TValue EnsureGetItem<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue) where TValue : new()
        {
            TValue value;
            bool found = dictionary.TryGetValue(key, out value);
            if (!found)
            {
                value = newValue;
                dictionary[key] = newValue;
            }
            return value;
        }

        public static TValue EnsureGetItem<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue value;
            bool found = dictionary.TryGetValue(key, out value);
            if (!found)
            {
                value = new TValue();
                dictionary[key] = value;
            }
            return value;

        }

    }

}
