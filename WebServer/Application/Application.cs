using System;
using System.Collections.Generic;
using System.Linq;
using WebServer.Routing;

namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public abstract class Application : IApplication
    {
        #region Private Members

        private readonly List<RouteCollection> _routes;
        private readonly ModuleCollection _modules;
        private bool _disposed;

        #endregion

        #region Constuctors

        protected Application()
        {
            var locator = new ApplicationLocator();
            _modules = (ModuleCollection) locator.FindModules();
            _routes = _modules.Select(module => new RouteCollection(module)).ToList();
            Routes = new RouteCollection();
        }

        #endregion

        #region Methods

        public RouteCollection Routes { get; }

        //internal ModuleCollection Modules { get; set; }

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
            var routes = FindRequestRoute(request);
            // Call the module.
            // Return the result.
        }

        #endregion

        #region Helpers

        // Find the route that matches the request.
        private RouteEntry FindRequestRoute(IWebRequest request)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var route in _routes.SelectMany(x => x))
            {
                var graph = new RouteGraph(route);
                if (graph.PatternMatch(request.Path))
                {
                    return graph.Route;
                }
            }
            return null;
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
        }

        #endregion
    }
}