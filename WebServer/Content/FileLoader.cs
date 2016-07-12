using System;
using System.IO;
using System.Net;

namespace WebServer.Content
{
    internal class FileLoader : IContentLoader
    {
        public Stream LoadContent(HttpListenerContext context, string path, string ext)
        {
            throw new NotImplementedException();
        }
    }
}
