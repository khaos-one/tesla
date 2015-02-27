using System;
using System.Diagnostics;
using System.Net;

namespace Tesla.Net
{
    //using HandlerFunc = Func<HttpListenerContext, Task>;
    
    public class HttpServer
        : ServerBase
    {
        protected HttpListener Listener;
        protected HttpHandlerFunc Handler;

        public HttpServer(HttpHandlerFunc handlerFunc, string[] uriPrefixes)
        {
            Handler = handlerFunc;
            Listener = new HttpListener();
            Array.ForEach(uriPrefixes, x => Listener.Prefixes.Add(x));
        }

        public HttpServer(HttpHandlerFunc handlerFunc, string uriPrefix)
            : this(handlerFunc, new[] {uriPrefix})
        { }

        public HttpServer(HttpHandlerFunc handlerFunc)
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

        protected override Action AcceptClient()
        {
            var context = Listener.GetContext();
            return async () =>
            {
                //context.Response.Headers.Add(HttpResponseHeader.Server, "Tesla/1.0");

                try
                {
                    await Handler(context);
                }
                catch (HttpException e)
                {
                    context.Response.StatusCode = (Int32) e.HttpCode;
                    context.Response.Write(HttpException.FormatErrorCode(context.Response.StatusCode,
                        context.Response.StatusDescription));
                }
                catch (Exception e)
                {
                    Trace.TraceWarning("HTTP Handler exception: {0}.", e);

                    context.Response.StatusCode = 500;
                    context.Response.Write(HttpException.FormatErrorCode(context.Response.StatusCode,
                        context.Response.StatusDescription));
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
