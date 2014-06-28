using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Tesla.Net
{
    using HandlerFunc = Func<HttpListenerContext, Task>;

    public class HttpServer
        : ServerBase
    {
        protected HttpListener Listener;
        protected HandlerFunc HandlerFunc;

        public HttpServer(HandlerFunc handlerFunc, string[] uriPrefixes)
        {
            HandlerFunc = handlerFunc;
            Listener = new HttpListener();
            Array.ForEach(uriPrefixes, x => Listener.Prefixes.Add(x));
        }

        public HttpServer(HandlerFunc handlerFunc, string uriPrefix)
            : this(handlerFunc, new[] {uriPrefix})
        { }

        public HttpServer(HandlerFunc handlerFunc)
            : this(handlerFunc, "http://*:80")
        { }

        protected override void OnStart()
        {
            Listener.Start();
        }

        protected override async Task<Action> AcceptClient()
        {
            var context = await Listener.GetContextAsync();
            return async () =>
            {
                try
                {
                    await HandlerFunc(context);
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("HTTP Handler exception: {0}.", e);
                }

                context.Response.OutputStream.Close();
            };
        }

        protected override void OnStop()
        {
            Listener.Stop();
        }

        public new void Dispose()
        {
            base.Dispose();

            if (Listener != null)
            {
                Listener.Close();
            }
        }
    }
}
