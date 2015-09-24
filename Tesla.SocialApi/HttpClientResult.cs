using System;
using System.Net;
using System.Text;

namespace Tesla.SocialApi {
    public sealed class HttpClientResult {
        public WebHeaderCollection Headers { get; set; }
        public CookieContainer Cookies { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ContentType { get; set; }

        public Encoding Encoding { get; set; }
        public string Content { get; set; }

        public Uri Uri { get; set; }
    }
}