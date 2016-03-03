using System.Collections.Generic;
using System.Net;
using Tesla.Extensions;

namespace Tesla.Net {
    public class RoutedHttpServer
        : HttpServerBase {
        protected List<KeyValuePair<string, HttpHandlerFunc>> Routes
            = new List<KeyValuePair<string, HttpHandlerFunc>>();

        public RoutedHttpServer(string[] uriPrefixes)
            : base(uriPrefixes) {}

        public RoutedHttpServer(string uriPrefix)
            : base(uriPrefix) {}

        public RoutedHttpServer() {}

        public void AddRoute(string pattern, HttpHandlerFunc handler) {
            Routes.Add(new KeyValuePair<string, HttpHandlerFunc>(pattern, handler));
            SortRoutes();
        }

        public void AddRoute(string[] patterns, HttpHandlerFunc handler) {
            foreach (var pattern in patterns) {
                Routes.Add(new KeyValuePair<string, HttpHandlerFunc>(pattern, handler));
            }
            SortRoutes();
        }

        protected void SortRoutes() {
            // Sort routes from by pattern length, longer patterns should always come first.
            // I.e. descending sort.
            Routes.Sort((x, y) => -x.Key.Length.CompareTo(y.Key.Length));
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