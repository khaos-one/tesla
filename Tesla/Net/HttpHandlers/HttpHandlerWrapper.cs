using System;
using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net.HttpHandlers
{
    [Obsolete]
    public class HttpHandlerWrapper
        : HttpHandlerBase
    {
        protected HttpHandlerFunc Handler;

        public HttpHandlerWrapper(HttpHandlerFunc handler)
        {
            Handler = handler;
        }

        public override async Task Handle(HttpListenerContext context)
        {
            await Handler(context);
        }
    }
}
