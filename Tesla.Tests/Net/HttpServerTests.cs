using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tesla.Net;
using Tesla.Net.HttpHandlers;

namespace Tesla.Core.Tests.Net
{
    [TestClass]
    public sealed class HttpServerTests
    {
        [TestMethod]
        public void HttpServerGenericTest()
        {
            var httpHandler = new RoutedHttpHandler();
            httpHandler.MapRoute("*.ths", async context =>
            {
                await context.Response.WriteAsync("So wazzup bitches?");
            });

            var httpServer = new HttpServer(httpHandler, "http://localhost:8080/");

            httpServer.Start();
            Console.ReadLine();
            httpServer.Stop();
        }
    }
}
