using System;
using WebServer;
using WebServer.Application;
using WebServer.Workflow;

namespace ServerHost
{
    public class RequestProcessor : IRequestProcessor<IWebServerContext>
    {
        #region Private Members

        private bool _disposed;

        #endregion

        #region Automatic Properties

        public IApplication Application { get; }

        public Workflow<IWebServerContext> Workflow { get; }

        #endregion

        #region Constructors

        public RequestProcessor(IApplication application)
        {
            Application = application ?? ApplicationLocator.FindApplication();
            Workflow = new Workflow<IWebServerContext>()
                .Do(LogIpAddress)
                .Do(AuthenticateContext)
                .Do(WhiteList)
                .Do(SessionProvider)
                .Do(RouteProvider)
                .Do(Response);
        }

        #endregion

        #region Methods

        /// <summary>
        /// A workflow item, implementing a simple instrumentation of the client IP address, port, and URL.
        /// </summary>
        public static void LogIpAddress(IWorkflowContext<IWebServerContext> context)
        {
            Console.WriteLine("{remoteEndPoint} : {rawUrl}".Render(new { remoteEndPoint = context.Token.HttpContext.Request.RemoteEndPoint, rawUrl = context.Token.HttpContext.Request.RawUrl }));
            Console.WriteLine(context.Token.HttpContext.Request.RemoteEndPoint + @" : " + context.Token.HttpContext.Request.RawUrl);
        }

        /// <summary>
        /// A workflow item, implementing a simple instrumentation of the client IP address, port, and URL.
        /// </summary>
        public static void AuthenticateContext(IWorkflowContext<IWebServerContext> context)
        {
            Console.WriteLine(@"Authtenticate Context");
            Console.WriteLine(context.Token.HttpContext.Request.RemoteEndPoint + @" : " + context.Token.HttpContext.Request.RawUrl);
        }

        /// <summary>
        /// Only intranet IP addresses are allowed.
        /// </summary>
        public static void WhiteList(IWorkflowContext<IWebServerContext> context)
        {
            string url = context.Token.HttpContext.Request.RemoteEndPoint?.ToString();
            bool valid = url != null && (url.StartsWith("192.168") || url.StartsWith("127.0.0.1") || url.StartsWith("[::1]"));

            if (!valid)
                throw new WorkflowAbortException($"Invalid endpoint: {url}");
        }

        /// <summary>
        /// Only intranet IP addresses are allowed.
        /// </summary>
        public static void SessionProvider(IWorkflowContext<IWebServerContext> context)
        {
            var s = context.Token.Session;
            Console.WriteLine(s.Id);
            //Console.WriteLine(s.EndPoint);

            if (s.Expired)
            {
                throw new WorkflowException("Session has expired");
            }
        }

        /// <summary>
        /// Only intranet IP addresses are allowed.
        /// </summary>
        public static void RouteProvider(IWorkflowContext<IWebServerContext> context)
        {
            Console.WriteLine(@"In route provider");
        }

        /// <summary>
        /// Produce a response.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>workflow state</returns>
        public static void Response(IWorkflowContext<IWebServerContext> context)
        {
            Console.WriteLine(@"In Response");
            Console.WriteLine($"Request Verb = '{context.Token.GetRequestVerb()}'");
            context.Token.SendResponseText($"<HTML><BODY>My web page.<br>{DateTime.Now}</BODY></HTML>");
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
            Application.Dispose();
        }

        #endregion
    }
}
