using System;
using WebServer.Routing;

namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public abstract class Application : IApplication
    {
        #region Private Members

        private readonly RouteCollection _routes;
        private readonly ModuleCollection _modules;
        private bool _disposed;

        #endregion

        #region Constuctors

        protected Application()
        {
            var locator = new ApplicationLocator();
            _modules = (ModuleCollection) locator.FindModules();
            _routes = new RouteCollection();
            Routes = new RouteCollection();
        }

        #endregion

        #region Methods

        public RouteCollection Routes { get; }

        internal ModuleCollection Modules { get; set; }

        public virtual void Startup()
        {
            throw new NotImplementedException();
        }

        public virtual void GetEnvironment()
        {
            throw new NotImplementedException();
        }

        public void HandleRequest(IWebRequest request)
        {
            Console.WriteLine(@"Handle the request.");
            // Route the request.
            // Call the module.
            // Return the result.
        }

        #endregion

        #region Helpers


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
        }

        #endregion
    }
}