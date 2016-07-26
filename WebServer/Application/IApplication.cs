using System;
using WebServer.Routing;

namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public interface IApplication : IDisposable
    {
        /// <summary>
        /// Routes
        /// </summary>
        RouteCollection Routes { get; }

        /// <summary>
        /// Start the application.
        /// </summary>
        void Startup();

        /// <summary>
        /// Get the environment instance.
        /// </summary>
        void GetEnvironment();

        /// <summary>
        /// Get the environment instance.
        /// </summary>
        void HandleRequest(IWebRequest request);
    }
}