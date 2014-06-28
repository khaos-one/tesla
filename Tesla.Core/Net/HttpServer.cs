using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Tesla.IO;

namespace Tesla.Net
{
    //using HandlerFunc = Func<HttpListenerContext, Task>;
    
    public class HttpServer
        : ServerBase
    {
        public delegate Task HandlerFunc(HttpListenerContext context);

        protected HttpListener Listener;
        protected HandlerFunc Handler;

        public HttpServer(HandlerFunc handlerFunc, string[] uriPrefixes)
        {
            Handler = handlerFunc;
            Listener = new HttpListener();
            Array.ForEach(uriPrefixes, x => Listener.Prefixes.Add(x));
        }

        public HttpServer(HandlerFunc handlerFunc, string uriPrefix)
            : this(handlerFunc, new[] {uriPrefix})
        { }

        public HttpServer(HandlerFunc handlerFunc)
            : this(handlerFunc, "http://*:80/")
        { }

        public HttpServer(IHttpHandler handler, string[] uriPrefixes)
            : this(handler.Handle, uriPrefixes)
        { }

        public HttpServer(IHttpHandler handler, string uriPrefix)
            : this(handler.Handle, new[] { uriPrefix })
        { }

        public HttpServer(IHttpHandler handler)
            : this(handler.Handle, "http://*:80/")
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
                    await Handler(context);
                }
                catch (HttpException e)
                {
                    context.Response.StatusCode = (Int32) e.HttpCode;
                    context.Response.OutputStream.Write(HttpException.FormatErrorCode(context.Response.StatusCode,
                        context.Response.StatusDescription));
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
