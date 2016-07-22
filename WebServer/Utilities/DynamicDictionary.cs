using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace WebServer.Utilities
{
    public class DynamicDictionary : DynamicObject, IDictionary<string, object>
    {
        // The inner dictionary.
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        // This property returns the number of elements 
        // in the inner dictionary. 
        public int Count => _dictionary.Count;

        /// <summary>
        /// Determines whether there are any objects.
        /// </summary>
        public bool IsEmpty => Count == 0;

        public bool IsReadOnly => _dictionary.IsReadOnly;

        public ICollection<string> Keys => _dictionary.Keys;

        public ICollection<object> Values => _dictionary.Values;

        public virtual object this[string key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets a member from the dynamic object
        /// </summary>
        /// <param name="binder">dynamic member binder</param>
        /// <param name="result">return result</param>
        /// <returns> true if successful otherwise false is returned.</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// Sets a member of the dynamic object
        /// </summary>
        /// <param name="binder">dynamic member binder</param>
        /// <param name="value">value to set</param>
        /// <returns> true if successful otherwise false is returned.</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        #region IDictionary Members

        public void Add(KeyValuePair<string, object> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dictionary.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        #endregion
    }
}
