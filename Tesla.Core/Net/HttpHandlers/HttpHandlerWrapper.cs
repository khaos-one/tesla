using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net.HttpHandlers
{
    public class HttpHandlerWrapper
        : HttpHandlerBase
    {
        protected HttpServer.HandlerFunc Handler;

        public HttpHandlerWrapper(HttpServer.HandlerFunc handler)
        {
            Handler = handler;
        }

        public override async Task Handle(HttpListenerContext context)
        {
            await Handler(context);
        }
    }
}
