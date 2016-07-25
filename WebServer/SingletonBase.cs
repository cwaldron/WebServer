using System;

namespace WebServer
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        #region Member Variables

        private static SingletonBase<T> _singleton;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Create singleton instance.
        /// </summary>
        protected SingletonBase()
        {
            // Set singleton.
            _singleton = this;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        protected static T Instance
        {
            get
            {
                if (_singleton != null) return (T) _singleton;
                lock (SyncRoot)
                {
                    _singleton = (T) Activator.CreateInstance(typeof(T), true);
                }

                return (T)_singleton;
            }
        }

        #endregion
    }
}