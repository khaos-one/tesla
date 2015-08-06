using System;
using System.Net;
using Tesla.Net;
using Tesla.Net.FastCgi;

namespace Sandbox
{
    class Program
    {
        static void HandlerFunc(HttpListenerContext context, string[] param)
        {
            context.Response.Write(string.Format("I see {0} too!", param[0]));
        }

        static void Main(string[] args)
        {
            var server = new RoutedHttpServer();
            server.AddRoute("*/isee/*", HandlerFunc);
            server.Start();
            var fastcgi = new FastCgiServer(9000);
            fastcgi.Start();
            Console.WriteLine("Server started...");
            Console.ReadLine();
        }
    }
}
