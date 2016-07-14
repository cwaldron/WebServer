namespace WebServer.Sessions
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        #region Member Variables

        private static SingletonBase<T> _singleton;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Create instance of the PackageService class.
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
        protected static T Instance
        {
            get
            {
                if (_singleton != null) return (T)_singleton;
                lock (SyncRoot)
                {
                    _singleton = new T();
                }

                return (T)_singleton;
            }
        }

        #endregion
    }
}