using System;
using System.Collections.Concurrent;

namespace WebServer.Sessions
{
    public class SessionManager
    {
        #region Private Members

        private readonly ConcurrentDictionary<Guid, Session> _sessions;

        #endregion

        #region Constants

        internal const string SessionCookieKey = "__SESSION_COOKIE__";

        #endregion

        #region Constructors

        public SessionManager()
        {
            _sessions = new ConcurrentDictionary<Guid, Session>();
        }

        #endregion

        #region Methods

        public Session CreateSession()
        {
            var session = new Session();
            if (!_sessions.TryAdd(session.Id, session))
            {
                Console.WriteLine(@"Adding new session failed when it should have succeeded");
                return null;
            }
            return session;
        }

        /// <summary>
        /// Gets the session based on the session id.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>session</returns>
        public Session GetSession(Guid sessionId)
        {
            Session session;
            return (_sessions.TryGetValue(sessionId, out session)) ? session : null;
        }

        #endregion
    }
}
