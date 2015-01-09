using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Tesla.Net
{
    [Obsolete]
    public abstract class HttpHandlerBase
        : IHttpHandler
    {
        public IHttpHandler NextHandler;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
