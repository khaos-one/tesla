using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesla.Net;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpserver = new HttpServer(async context =>
            {
                var responseBytes = Encoding.UTF8.GetBytes("<html><body><pre>It's working!</pre></body></html>");
                context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
            }, "http://*:8080/");

            httpserver.Start();
            Console.WriteLine("Http server started.");
            Console.ReadLine();
        }
    }
}
