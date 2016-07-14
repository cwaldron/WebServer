using System;
using System.Collections.Concurrent;

namespace WebServer.Sessions
{
    public class SessionManager
    {
        #region Private Members

        private readonly ConcurrentDictionary<Guid, Session> _sessions;

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

        #endregion
    }
}
