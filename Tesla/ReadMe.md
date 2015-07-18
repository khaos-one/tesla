Fast TCP/UDP and HTTP server
============================

Routed HTTP server usage example
--------------------------------

```
using System;
using System.Net;
using Tesla.Net;

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
            server.AddRoute("/isee/*", HandlerFunc);
            server.Start();
            Console.WriteLine("Server started...");
            Console.ReadLine();
        }
    }
}
```
