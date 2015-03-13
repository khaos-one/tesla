using System;
using System.Collections;
using System.IO;
using Tesla;
using Tesla.Net;
using Tesla.Net.HttpHandlers;
using Tesla.Types;

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
            var b = new BitArray(24);
            b.SetValue(4, 4, 3);
        }
    }
}
