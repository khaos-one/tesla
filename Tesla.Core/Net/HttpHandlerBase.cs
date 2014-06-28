using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net
{
    public abstract class HttpHandlerBase
        : IHttpHandler
    {
        protected IHttpHandler NextHandler;

        protected async void ToNextHandler(HttpListenerContext context)
        {
            if (NextHandler != null)
                await NextHandler.Handle(context);
            else
                throw new HttpException(HttpStatusCode.NotFound);
        }

        public abstract Task Handle(HttpListenerContext context);
    }
}
