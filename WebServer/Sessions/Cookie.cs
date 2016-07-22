using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Utilities;

namespace WebServer.Sessions
{
    public class Cookie : DynamicDictionary
    {
        #region Constants

        private const string DomainToken = "domain";
        private const string ExpiresToken = "expires";
        private const string MaxAgeToken = "max-age";
        private const string PathToken = "path";
        private const string SecureToken = "secure";
        private const string HttpOnlyToken = "httponly";
        private const string DefaultPath = "/"; 
        private const char KeyValueSeparatorChar = '&';

        #endregion

        #region Automatic Properties

        public string Comment { get; set; }

        public bool Discard { get; set; }

        public string Domain { get; set; }

        public bool Expired { get; set; }

        public DateTime? Expires { get; set; }

        public bool HttpOnly { get; set; }

        /// <summary>
        /// Get the session object keys.
        /// </summary>
        public new IReadOnlyCollection<string> Keys => Keys;

        public string Name { get; }

        public long? MaxAge { get; set; }

        public bool Secure { get; set; }

        public string Path { get; set; }

        public DateTime TimeStamp { get; }

        public int Version { get; }

        #endregion

        #region Constructors

        internal Cookie(string name)
        {
            Name = name;
            Comment = string.Empty;
            Path = DefaultPath;
            TimeStamp = DateTime.Now;
            Value = string.Empty;
        }

        public Cookie(string name, string value)
            : this(name)
        {
            Value = value;
        }

        /// <summary>
        /// Create cookie from listener cookie.
        /// </summary>
        /// <param name="cookie">listener cookie</param>
        internal Cookie(System.Net.Cookie cookie)
            : this(cookie.Name)
        {
            Comment = cookie.Comment;
            Domain = cookie.Domain;
            Expires = cookie.Expires;
            HttpOnly = cookie.HttpOnly;
            Path = cookie.Path;
            Secure = cookie.Secure;
            Version = cookie.Version;
            Value = cookie.Value;
            TimeStamp = cookie.TimeStamp;
            Discard = cookie.Discard;
            MaxAge = GetMaxAge(cookie);
        }

        #endregion

        #region Properties

        public string Value
        {
            get
            {
                return SerializeValue();
            }
            set
            {
                DeserializeValue(value);
            }
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
        public T Get<T>(string key)
        {
            try
            {
                if (typeof(T) != typeof(string) && this[key] is string)
                {
                    this[key] = Converter.Deserialize<T>(this[key].ToString());
                }
                return (T) this[key];
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the set-cookie response header.
        /// </summary>
        /// <returns>set=cookie response header.</returns>
        internal string GetHeaderValue()
        {
            var sb = new StringBuilder();

            sb.Append(Name);
            sb.Append('=');
            sb.Append(Value);

            if (Comment != null)
            {
                sb.Append($"; Comment={Domain}");
            }

            if (Domain != null)
            {
                sb.Append($"; {DomainToken}={Domain}");
            }

            if (Expires != null)
            {
                sb.Append($"; {ExpiresToken}={Expires.GetValueOrDefault().ToUtcString()}");
            }

            if (MaxAge != null)
            {
                sb.Append($"; {MaxAgeToken}={MaxAge}");
            }

            if (Path != null)
            {
                sb.Append($"; {PathToken}={Path}");
            }

            if (Secure)
            {
                sb.Append($"; {SecureToken}");
            }

            if (HttpOnly)
            {
                sb.Append($"; {HttpOnlyToken}");
            }

            return sb.ToString();
        }

        internal System.Net.Cookie GetResponseCookie()
        {
            return new System.Net.Cookie
            {
                Comment = Comment,
                Discard = Discard,
                Domain = Domain,
                Expired = Expired,
                Expires = Expires.GetValueOrDefault(),
                HttpOnly = HttpOnly,
                Name = Name,
                Path = Path,
                Secure = Secure,
                Value = Value,
                Version = Version
            };
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.Append('=');
            sb.Append(Value);
            return sb.ToString();
        }

        #endregion

        #region Indexers

        // Indexer for accessing session objects. If an object isn't found,
        // null is returned. 
        public override object this[string key]
        {
            get
            {
                object val;
                TryGetValue(key, out val);
                return val;
            }

            set
            {
                base[key] = value;
            }
        }

        #endregion

        #region Helpers

        private static long GetMaxAge(System.Net.Cookie cookie)
        {
            var maxAge = (long)cookie.Expires.Subtract(cookie.TimeStamp).TotalSeconds;
            if (maxAge < 0L)
            {
                maxAge = -1L;
            }

            return maxAge;
        }

        /// <summary>
        /// Deserialize cookie value
        /// </summary>
        /// <param name="value">cookie value</param>
        private void DeserializeValue(string value)
        {
            // Clear the properties.
            Clear();

            // Check value.
            if (string.IsNullOrEmpty(value)) return;

            // Convert value string to key-value collection.
            string[] pairs = value.Split(KeyValueSeparatorChar);
            foreach (var keyvalue in pairs.Select(pair => pair.Split('=')))
            {
                switch (keyvalue.Length)
                {
                    case 1:
                        this.AddOrUpdate(string.Empty, keyvalue[0], (k, v) => keyvalue[0]);
                        break;

                    case 2:
                        this.AddOrUpdate(keyvalue[0], keyvalue[1], (k, v) => keyvalue[1]);
                        break;

                    default:
                        throw new FormatException();
                }
            }
        }

        /// <summary>
        /// Serialize cookie value
        /// </summary>
        /// <returns>serialized cookie</returns>
        private string SerializeValue()
        {
            var sb = new StringBuilder();

            foreach (var pair in this)
            {
                sb.Append(string.IsNullOrEmpty(pair.Key)
                    ? string.Empty
                    : $"{pair.Key}=");

                sb.Append($"{pair.Value}{KeyValueSeparatorChar}");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        #endregion
    }
}