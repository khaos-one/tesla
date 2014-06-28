using System;
using System.IO;
using System.Text;
using Tesla.Net;
using Tesla.Net.HttpHandlers;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new StaticServeHttpHandler(Directory.GetCurrentDirectory())
            {
                NextHandler = new PathActivatorHttpHandler()
                {
                    {
                        "/home",
                        async context =>
                        {
                            var template = new Sample
                            {
                                Model = new
                                {
                                    hello = "This is a string from the code!"
                                }
                            };

                            var responseBytes = Encoding.UTF8.GetBytes(template.TransformText());
                            context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                }
            };

            var httpServer = new HttpServer(handler, "http://*:8080/");

            httpServer.Start();
            Console.WriteLine("Http server started.");
            Console.ReadLine();
            httpServer.Stop();
        }
    }
}
