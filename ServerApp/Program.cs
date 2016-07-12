using System;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ws = new WebServer.WebServer(new RequestProcessor(), "http://localhost:80/"))
            {
                ws.Run();
                Console.WriteLine("Press any key to exit the server");
                Console.ReadKey();
            }
        }
    }
}
