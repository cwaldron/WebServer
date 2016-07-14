using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using WebServer.Queueing;
using WebServer.Sessions;
using WebServer.Workflow;

namespace WebServer
{
    public class WebServer : IRequestProcessor<IWebServerContext>, IDisposable
    {
        #region Private Members

        private readonly HttpListener _listener;
        private readonly RequestManager<IWebServerContext> _requestManager;
        private readonly SessionManager _sessionManager;
        private Workflow<IWebServerContext> _workflow; 
        private bool _disposed;

        #endregion

        #region Properties

        /// <summary>
        /// Determines if the webserver is listening for requests.
        /// </summary>
        public bool IsListening => _listener.IsListening;

        #endregion

        #region Constructors

        /// <summary>
        /// WebServer constructor.
        /// </summary>
        /// <param name="prefixes">domain prefixes</param>
        /// <param name="requestProcessor">response builder</param>
        public WebServer(IReadOnlyCollection<string> prefixes, IRequestProcessor<IWebServerContext> requestProcessor = null)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("HttpListener not supported.");
            }

            // URI prefixes are required, for example: "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Count == 0)
            {
                throw new ArgumentException("prefixes");
            }

            // Setup session manager.
            _sessionManager = new SessionManager();

            // Setup abort and exception handlers.
            var processor = requestProcessor ?? this;
            var workflow = processor.GetWorkflow();
            if (!workflow.HasAbortHandler)
            {
                workflow.OnAbort(AbortHandler);
            }

            if (!workflow.HasExceptionHander)
            {
                workflow.OnException(ExceptionHandler);
            }

            // Setup listener.
            _listener = new HttpListener();
            _requestManager = new RequestManager<IWebServerContext>(processor);

            // Setup prefixes
            foreach (var s in prefixes)
            {
                GrantAccess(s, Environment.UserDomainName, Environment.UserName);
                _listener.Prefixes.Add(s);
            }

            // Start server.
            _requestManager.Start();
            _listener.Start();
        }

        /// <summary>
        /// WebServer constructor.
        /// </summary>
        /// <param name="responseBuilder">responseBuilder</param>
        /// <param name="prefixes">domain prefixes</param>
        public WebServer(IRequestProcessor<IWebServerContext> responseBuilder, params string[] prefixes)
            : this(prefixes, responseBuilder)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Run the web server.
        /// </summary>
        public void Run()
        {
            Task.Run(() =>
            {
                while (_listener.IsListening)
                {
                    try
                    {
                        _requestManager.Process(new WebServerContext(_listener.GetContext(), _sessionManager));
                    }
                    catch (HttpListenerException)
                    {
                    }
                }
            });
        }

        /// <summary>
        /// Stop the server.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
        }

        /// <summary>
        /// Gets the request process workflow.
        /// </summary>
        /// <returns>workflow</returns>
        public Workflow<IWebServerContext> GetWorkflow()
        {
            return _workflow ?? (_workflow = new Workflow<IWebServerContext>());
        }

        /// <summary>
        /// Default abort handler.
        /// </summary>
        /// <param name="context">workflow context</param>
        /// <param name="ex">workflow abort exception</param>
        public static void AbortHandler(IWorkflowContext<IWebServerContext> context, Exception ex)
        {
            // TODO: need send response to set status code.
            Console.WriteLine(@"Aborting request");
            context.Token.SendResponseText($"Web Server Abort: {ex.Message}"); 
        }

        /// <summary>
        /// Default exception handler.
        /// </summary>
        /// <param name="context">workflow context</param>
        /// <param name="ex">exception</param>
        public static void ExceptionHandler(IWorkflowContext<IWebServerContext> context, Exception ex)
        {
            // TODO: need send response to set status code.
            context.Token.SendResponseText($"Web Server Exception: {ex.Message}");
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
            _listener.Close();
            _requestManager.Dispose();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Grant permission to a domain on this machine.
        /// </summary>
        /// <param name="address">ip address</param>
        /// <param name="domain">domain</param>
        /// <param name="user">user</param>
        private static void GrantAccess(string address, string domain, string user)
        {
            string args = $@"http add urlacl url={address} user={domain}\{user}";
            var psi = new ProcessStartInfo("netsh", args)
            {
                Verb = "runas",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            };
            Process.Start(psi)?.WaitForExit();
        }

        #endregion
    }
}
