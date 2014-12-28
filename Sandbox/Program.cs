using System;
using System.IO;
using Tesla.Net;
using Tesla.Net.HttpHandlers;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Main2();

            var handler = new StaticServeHttpHandler(Directory.GetCurrentDirectory())
            {
                NextHandler = new PathActivatorHttpHandler
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

                            context.Response.Write(template.TransformText());
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

        static void Main2()
        {
            
        }
    }
}
