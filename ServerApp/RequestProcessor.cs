using System;
using WebServer;
using WebServer.Workflow;

namespace ServerApp
{
    public class RequestProcessor : IRequestProcessor<IWebServerContext>
    {
        private WebContentLoader _loader;
        private readonly Workflow<IWebServerContext> _workflow;

        public RequestProcessor()
        {
            _loader = new WebContentLoader();
            _workflow = new Workflow<IWebServerContext>()
                .Do(LogIpAddress)
                .Do(AuthenticateContext)
                .Do(WhiteList)
                .Do(Response);
        }

        /// <summary>
        /// Gets the request process workflow.
        /// </summary>
        /// <returns>workflow</returns>
        public Workflow<IWebServerContext> GetWorkflow()
        {
            return _workflow;
        }

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

            string url = context.Token.HttpContext.Request.RemoteEndPoint?.ToString();
            bool valid = url != null && (url.StartsWith("192.168") || url.StartsWith("127.0.0.1") || url.StartsWith("[::1]"));

            if (!valid)
                throw new WorkflowAbortException($"Invalid endpoint: {url}");
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
    }
}
