using System;
using System.Collections.Generic;
using System.Net;
using WebServer.Sessions;
using Cookie = WebServer.Sessions.Cookie;

namespace WebServer
{
    public interface IWebServerContext
    {
        /// <summary>
        /// Context id.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Current http context.
        /// </summary>
        HttpListenerContext HttpContext { get; }

        /// <summary>
        /// Get cookie collection.
        /// </summary>
        IReadOnlyDictionary<string, Cookie> Cookies { get; }

        /// <summary>
        /// Current session object.
        /// </summary>
        Session Session { get; }

        /// <summary>
        /// Adds a cookie to the context.
        /// </summary>
        /// <param name="name">cookie name</param>
        /// <returns>existing cookie or adds new cookie.</returns>
        Cookie AddCookie(string name);

        /// <summary>
        /// Gets a cookie from the context.
        /// </summary>
        /// <param name="name">cookie name</param>
        /// <returns>cookie if successful and null otherwise.</returns>
        Cookie GetCookie(string name);

        /// <summary>
        /// Sets a cookie.
        /// </summary>
        /// <param name="cookie">cookie</param>
        void SetCookie(Cookie cookie);

        /// <summary>
        /// Returns the endpoint address.
        /// </summary>
        /// <returns></returns>
        IPAddress GetEndpointAddress();

        /// <summary> 
        /// Returns the request header collection.
        /// </summary>
        IReadOnlyDictionary<string, string> GetRequestHeaders();

        /// <summary> 
        /// Returns a dictionary of the parameters on the URL.
        /// </summary>
        IReadOnlyDictionary<string, string> GetQueryParameters();

        /// <summary> 
        /// Returns the method name of the request: GET, POST, PUT, DELETE, and so forth.
        /// </summary>
        string GetRequestMethod();

        /// <summary> 
        /// Returns the web request.
        /// </summary>
        IWebRequest GetRequest();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repsonse"></param>
        void SendResponseText(string repsonse);
    }
}
