using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Tesla.Collections;
using Tesla.Extensions;
using Tesla.IO;

namespace Tesla.SocialApi {
    public sealed class HttpClient
        : IDisposable {
        public const string UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
        public const string Accepts = "text/html";

        public HttpClient() {
            Cookies = new CookieContainer();
        }

        public CookieContainer Cookies { get; }

        public void Dispose() {}

        private HttpWebRequest CreateRequest(string uri, bool followRedirect = true) {
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.UserAgent = UserAgent;
            request.Accept = Accepts;
            request.CookieContainer = Cookies;
            request.AllowAutoRedirect = followRedirect;

            return request;
        }

        private HttpWebResponse GetQuery(string uri, bool followRedirect = true) {
            return (HttpWebResponse) CreateRequest(uri, followRedirect).GetResponse();
        }

        private HttpClientResult FormatClientResult(HttpWebResponse response) {
            var result = new HttpClientResult {
                Cookies = Cookies,
                Headers = response.Headers,
                Status = response.StatusCode,
                ContentType = response.ContentType,
                Uri = response.ResponseUri
            };

            if (
                response.StatusCode == HttpStatusCode.OK &&
                (
                    response.ContentType.StartsWith("text/html") ||
                    response.ContentType.StartsWith("text/plain") ||
                    response.ContentType.StartsWith("application/json")
                    )
                ) {
                result.Encoding = Encoding.GetEncoding(response.CharacterSet);

                using (var s = response.GetResponseStream())
                    result.Content = s.ReadString(result.Encoding);
            }

            return result;
        }

        public HttpClientResult Get(string uri, bool followRedirect = true) {
            var response = GetQuery(uri, followRedirect);
            return FormatClientResult(response);
        }

        public HttpClientResult Post(string uri, Dictionary<string, string> parameters = null, Encoding encoding = null,
            bool followRedirect = true) {
            var request = CreateRequest(uri, followRedirect);

            if (parameters != null) {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                if (encoding == null)
                    encoding = Encoding.UTF8;

                var encodedParams = parameters
                    .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value)}")
                    .JoinString("&");
                var paramsBytes = encodedParams.ToBytes(encoding);

                request.ContentLength = paramsBytes.Length;

                using (var s = request.GetRequestStream())
                    s.Write(paramsBytes);
            }

            var response = (HttpWebResponse) request.GetResponse();
            return FormatClientResult(response);
        }
    }
}