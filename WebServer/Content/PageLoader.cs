using System;
using System.IO;
using System.Net;
using WebServer;

namespace WebServer.Content
{
    internal class PageLoader : IContentLoader
    {
        public Stream LoadContent(HttpListenerContext context, string path, string ext)
        {
            throw new NotImplementedException();
        }
    }
}
