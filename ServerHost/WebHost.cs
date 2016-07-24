using System;

namespace ServerHost
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class WebHost : IDisposable
    {
        private readonly WebServer.WebServer _server;
        private bool _disposed;

        public WebHost(params string[] urls)
        {
            _server = new WebServer.WebServer(new RequestProcessor(), urls);
        }

        public void Start()
        {
            _server.Run();
        }

        public void Stop()
        {
            _server.Stop();
        }

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