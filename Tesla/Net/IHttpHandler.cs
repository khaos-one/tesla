using System.Net;

namespace Tesla.Net {
    public delegate void HttpHandlerFunc(HttpListenerContext context, string[] param = null);

    public interface IHttpHandler {
        void Handle(HttpListenerContext context);
    }
}