using System.IO;
using System.Net;

namespace WebServer
{
    public interface IContentLoader
    {
        Stream LoadContent(HttpListenerContext context, string path, string ext);
    }
}
