using System;
using System.Net;
using System.Runtime.Serialization;

namespace Tesla.Net
{
    [Serializable]
    public class HttpException
        : Exception
    {
        public HttpStatusCode HttpCode { get; set; }

        public HttpException(HttpStatusCode httpCode)
            : base(null)
        {
            HttpCode = httpCode;
        }

        public HttpException(HttpStatusCode httpCode, string message)
            : base(message)
        {
            HttpCode = httpCode;
        }

        public HttpException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        internal static string FormatErrorCode(Int32 httpCode, string description)
        {
            return
                string.Format(
                    "<html><head><meta charset=\"utf-8\"/><title>Error {0}</title></head><body><h1>{0} &mdash; {1}</h1><hr /><em>Tesla/1.0</em></body></html>",
                    httpCode, description);
        }
    }
}
