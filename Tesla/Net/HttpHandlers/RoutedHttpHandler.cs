using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Tesla.Net.HttpHandlers
{
    [Obsolete]
    public class RoutedHttpHandler
        : IHttpHandler
    {
        protected Dictionary<string, HttpHandlerFunc> Routes = new Dictionary<string, HttpHandlerFunc>();
        protected HttpHandlerFunc DefaultHandlerFunc;

        public RoutedHttpHandler(HttpHandlerFunc defaultHandlerFunc = null)
        {
            DefaultHandlerFunc = defaultHandlerFunc;
        }

        public async Task Handle(HttpListenerContext context)
        {
            foreach (var kv in Routes)
            {
                if (Operators.LikeString(context.Request.RawUrl, kv.Key, CompareMethod.Text))
                {
                    await kv.Value(context);
                    return;
                }
            }

            if (DefaultHandlerFunc != null)
            {
                await DefaultHandlerFunc(context);
            }
            else
            {
                throw new HttpException(HttpStatusCode.NotFound);
            }
        }

        public void MapRoute(string pattern, HttpHandlerFunc handler)
        {
            Routes.Add(pattern, handler);
        }
    }
}
