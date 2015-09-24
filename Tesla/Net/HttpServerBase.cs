using System;
using System.Net;
using Tesla.Logging;

namespace Tesla.Net {
    //using HandlerFunc = Func<HttpListenerContext, Task>;

    public abstract class HttpServerBase
        : ThreadedServerBase {
        protected HttpListener Listener;

        protected HttpServerBase(string[] uriPrefixes) {
            Listener = new HttpListener();
            Array.ForEach(uriPrefixes, x => Listener.Prefixes.Add(x));
        }

        protected HttpServerBase(string uriPrefix)
            : this(new[] {uriPrefix}) {}

        protected HttpServerBase()
            : this("http://localhost:8080/") {}

        protected abstract void HandleRequest(HttpListenerContext context);

        protected override void OnStart() {
            Listener.Start();
        }

        protected override object AcceptClient() {
            return Listener.GetContext();
        }

        protected override void HandleClient(object state) {
            var context = (HttpListenerContext) state;

            try {
                HandleRequest(context);
            }
            catch (HttpException e) {
                context.Response.StatusCode = (int) e.HttpCode;
                context.Response.Write(HttpException.FormatErrorCode(context.Response.StatusCode,
                    context.Response.StatusDescription));
            }
            catch (Exception e) {
                Log.Entry(Priority.Warning, "HTTP handler exception: {0}.", e);

                context.Response.StatusCode = 500;
                context.Response.Write(HttpException.FormatErrorCode(context.Response.StatusCode,
                    context.Response.StatusDescription));
            }
            finally {
                context.Response.OutputStream.Close();
            }
        }

        protected override void OnStop() {
            Listener.Stop();
        }

        public new void Dispose() {
            base.Dispose();

            if (Listener != null) {
                Listener.Close();
            }
        }
    }
}