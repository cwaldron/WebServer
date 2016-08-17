using System;
using WebServer.Application;

namespace ServerHost
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class WebHost : IDisposable
    {
        #region Private Variables

        private readonly WebServer.WebServer _server;
        private bool _disposed;

        #endregion

        #region Constructors

        public WebHost(params string[] urls)
            : this(new ApplicationLocator().FindApplication(), urls)
        {
        }

        public WebHost(IApplication app, params string[] urls)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            _server = new WebServer.WebServer(new RequestProcessor(app), urls);
        }

        #endregion

        #region Methods

        public void Start()
        {
            _server.Run();
        }

        public void Stop()
        {
            _server.Stop();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the concurrent queue.
        /// </summary>
        public void Dispose()
        {
            Dispose(!_disposed);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose concurrent queue.
        /// </summary>
        /// <param name="disposing">indicate whether the queue is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            _server.Stop();
        }

        #endregion
    }
}