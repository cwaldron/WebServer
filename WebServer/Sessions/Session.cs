using System;

namespace WebServer.Sessions
{
    public sealed class Session : Cookie
    {
        #region Constants

        internal const string CookieName = "__SESSION_COOKIE__";
        private const string IdToken = "_id";
        private const string LastConnectionToken = "_lastConnection";
        private const string IsAuthorizedToken = "_isAuthorized";

        #endregion

        #region Constructors

        internal Session()
            : base(CookieName)
        {
            Id = Guid.NewGuid();
        }

        internal Session(System.Net.Cookie cookie)
            : base(cookie)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Session identifier
        /// </summary>
        public Guid Id
        {
            get { return Get<Guid>(IdToken); }
            private set { this[IdToken] = value; }
        }

        /// <summary>
        /// Last time the session was connected.
        /// </summary>
        public DateTime LastConnection
        {
            get { return Get<DateTime>(LastConnectionToken); }
            set { this[LastConnectionToken] = value; }
        }

        /// <summary>
        /// Determines whether the user is authorized
        ///  </summary>
        public bool IsAuthorized
        {
            get { return Get<bool>(IsAuthorizedToken); }
            set { this[IsAuthorizedToken] = value; }
        }

        #endregion

        #region Methods

        internal static Session NewSession(System.Net.Cookie cookie)
        {
            if (cookie == null) return new Session();

            if (cookie.Name != CookieName)
                throw new ArgumentException($"{cookie.Name} is not a session cookie");

            return new Session(cookie);
        }

        #endregion
    }
}
