using System;
using System.Collections.Generic;
using System.Net;
using Tesla;
using Tesla.Net;
using Tesla.SocialApi.Vk;

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
            var vk = new VkApi(4996626, AccessScopeExtensions.AllWithNoHttps);
            var authResult = vk.Authorize("89684394141", "65198700");

            if (authResult == AuthorizationResult.Ok)
            {
                var result = vk.Raw("users.get", new Dictionary<string, string> { { "user_id", "66748" } });
            }

            //var server = new RoutedHttpServer();
            //server.AddRoute("/forward/isee/*", HandlerFunc);
            //server.Start();
            //Console.WriteLine("Server started...");
            //Console.ReadLine();
        }
    }
}
