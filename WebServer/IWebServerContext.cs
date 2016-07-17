using System;
using System.Collections.Generic;
using System.Net;
using WebServer.Sessions;

namespace WebServer
{
    public interface IWebServerContext
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        HttpListenerContext HttpContext { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IPAddress GetEndpointAddress();

        /// <summary> 
        /// Returns the request header collection.
        /// </summary>
        IReadOnlyDictionary<string, string> GetRequestHeaders();

        /// <summary> 
        /// Returns the verb of the request: GET, POST, PUT, DELETE, and so forth.
        /// </summary>
        string GetRequestVerb();

        /// <summary> 
        /// Returns a dictionary of the parameters on the URL.
        /// </summary>
        IReadOnlyDictionary<string, string> GetQueryParameters();

        /// <summary>
        /// Returns the session object.
        /// </summary>
        /// <returns>session object</returns>
        Session GetSession();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repsonse"></param>
        void SendResponseText(string repsonse);
    }
}
