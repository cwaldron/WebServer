using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;

namespace WebServer.Sessions
{
    public class Session : DynamicObject, IEnumerable<KeyValuePair<string, object>>
    {
        #region Private Variables

        private readonly ConcurrentDictionary<string, object> _properties;

        #endregion

        #region Automatic Properties

        /// <summary>
        /// Session identifier
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Last time the session was connected.
        /// </summary>
        public DateTime LastConnection { get; set; }

        /// <summary>
        /// Determines whether the user is authorized
        ///  </summary>
        public bool IsAuthorized { get; set; }

        /// <summary>
        /// Determines whether the sesssion has expired.
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// Determines whether there are any session objects.
        /// </summary>
        public bool IsEmpty => _properties.IsEmpty;

        /// <summary>
        /// Get the session object keys.
        /// </summary>
        public IReadOnlyCollection<string> Keys => (IReadOnlyCollection<string>) _properties.Keys;

        #endregion

        #region Constructors

        internal Session()
        {
            _properties = new ConcurrentDictionary<string, object>();
            Id = Guid.NewGuid();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Object collection getter with type conversion.
        /// Note that if the object does not exist in the session, the default
        /// value is returned.
        /// Therefore, session objects like "isAdmin" or "isAuthenticated"
        /// should always be true for their "yes" state. 
        /// </summary>
        public T GetObject<T>(string objectKey)
        {
            object val;
            T ret;
            if (!_properties.TryGetValue(objectKey, out val)) return default(T);
            try
            {
                ret = (T)val;
            }
            catch (Exception)
            {
                ret = default(T);
            }

            return ret;
        }

        // Indexer for accessing session objects. If an object isn't found,
        // null is returned. 
        public object this[string objectKey]
        {
            get
            {
                object val;
                _properties.TryGetValue(objectKey, out val);
                return val;
            }

            set
            {
                _properties[objectKey] = value;
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Overrides

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            return true;
        }

        #endregion
    }
}
