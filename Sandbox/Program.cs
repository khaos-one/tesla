using System;
using System.Collections.Generic;
using System.Net;
using Tesla;
using Tesla.Net;
using Tesla.SocialApi.Vk;
using Tesla.Cryptography;

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
            var vk = new VkApi(4996626, AccessScopeExtensions.All);
            var authResult = vk.Authorize("89684394141", "6519568700");

            if (authResult == AuthorizationResult.Ok)
            {
                var result = vk.Raw("messages.send", new Dictionary<string, string> { { "user_id", "10919704" }, { "message", "Some awesome message" }, { "guid", StrongNumberProvider.UInt32.ToString() } });
                result = vk.Raw("messages.send", new Dictionary<string, string> { { "user_id", "10919704" }, { "message", "Some awesome message" }, { "guid", StrongNumberProvider.UInt32.ToString() } });
            }

            //var server = new RoutedHttpServer();
            //server.AddRoute("/forward/isee/*", HandlerFunc);
            //server.Start();
            //Console.WriteLine("Server started...");
            //Console.ReadLine();
        }
    }
}
