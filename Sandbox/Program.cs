using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using Tesla.Net;
using Tesla.Web.HttpHandlers;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            StaticServeHttpHandler.BasePath = Directory.GetCurrentDirectory();
            StaticServeHttpHandler.NextHandler = PathActivatorHttpHandler.Handle;
            PathActivatorHttpHandler.Bindings.Add("/admin", async context =>
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
            });

            var httpserver = new HttpServer(StaticServeHttpHandler.Handle, "http://*:8080/");

            httpserver.Start();
            Console.WriteLine("Http server started.");
            Console.ReadLine();
        }
    }
}
