using System;
using System.Net;
using System.Runtime.Serialization;

namespace Tesla.Net {
    [Serializable]
    public class HttpException
        : Exception {
        public HttpException(HttpStatusCode httpCode)
            : base(null) {
            HttpCode = httpCode;
        }

        public HttpException(HttpStatusCode httpCode, string message)
            : base(message) {
            HttpCode = httpCode;
        }

        public HttpException(SerializationInfo info, StreamingContext context)
            : base(info, context) {}

        public HttpStatusCode HttpCode { get; set; }

        internal static string FormatErrorCode(int httpCode, string description) {
            return
                string.Format(
                    "<html><head><meta charset=\"utf-8\"/><title>Error {0}</title></head><body><h1>{0} &mdash; {1}</h1><hr /><em>Tesla/1.0</em></body></html>",
                    httpCode, description);
        }
    }
}