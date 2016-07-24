using System;
using ServerHost;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new WebHost("http://localhost:80/"))
            {
                host.Start();
                Console.WriteLine(@"Press any key to exit the server");
                Console.ReadKey();
            }
        }
    }
}
