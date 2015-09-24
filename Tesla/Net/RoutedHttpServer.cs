using System.Collections.Generic;
using System.Net;

namespace Tesla.Net {
    public class RoutedHttpServer
        : HttpServerBase {
        protected Dictionary<string, HttpHandlerFunc> Routes
            = new Dictionary<string, HttpHandlerFunc>();

        public RoutedHttpServer(string[] uriPrefixes)
            : base(uriPrefixes) {}

        public RoutedHttpServer(string uriPrefix)
            : base(uriPrefix) {}

        public RoutedHttpServer() {}

        public void AddRoute(string pattern, HttpHandlerFunc handler) {
            Routes.Add(pattern, handler);
        }

        protected override void HandleRequest(HttpListenerContext context) {
            var absPath = context.Request.Url.AbsolutePath;

            foreach (var route in Routes) {
                var param = absPath.SplitWildcard(route.Key);

                if (param == null)
                    continue;

                route.Value(context, param);
                return;
            }

            throw new HttpException(HttpStatusCode.NotFound, "Not Found");
        }
    }
}