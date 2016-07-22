using System;
using System.Collections.Generic;

namespace WebServer
{
    public static class Extensions
    {
        public static string ToUtcString(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("r");
        }

        /// <summary>
        /// Add value to the dictionary.
        /// </summary>
        /// <typeparam name="TK">key type</typeparam>
        /// <typeparam name="TV">value type</typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryAdd<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value)
        {
            try
            {
                dictionary.Add(key, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Dictionary.GetOrAdd
        /// </summary>
        /// <typeparam name="TK">key type</typeparam>
        /// <typeparam name="TV">value type</typeparam>
        /// <param name="dictionary">dictionary</param>
        /// <param name="key">key value</param>
        /// <param name="value">value</param>
        /// <returns>value</returns>
        public static TV GetOrAdd<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }

            return dictionary[key];
        }

        /// <summary>
        /// Adds a key/value pair to the dictionary.  
        /// </summary>
        /// <typeparam name="TK">key type</typeparam>
        /// <typeparam name="TV">value type</typeparam>
        /// <param name="dictionary">dictionary</param>
        /// <param name="key">key value</param>
        /// <param name="value">value</param>
        /// <param name="updateFunc">update function</param>
        /// <returns>The value for the key.</returns>
        /// <remarks>
        /// Add or updates the key/value pair to the dictionary.
        /// </remarks>
        public static TV AddOrUpdate<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value, Func<TK, TV, TV> updateFunc)
        {
            return updateFunc(key, dictionary.GetOrAdd(key, value));
        }

        /// <summary>
        /// Adds a key/value pair to the dictionary.  
        /// </summary>
        /// <typeparam name="TK">key type</typeparam>
        /// <typeparam name="TV">value type</typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="addFunc">add function</param>
        /// <param name="updateFunc">update function</param>
        /// <returns>The new value for the key.</returns>
        /// <remarks>
        /// Add or updates the key/value pair to the dictionary.
        /// </remarks>
        public static TV AddOrUpdate<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, Func<TK, TV> addFunc, Func<TK, TV, TV> updateFunc)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, addFunc(key));
            }
            else
            {
                dictionary[key] = updateFunc(key, dictionary[key]);
            }

            return dictionary[key];
        }
    }
}
