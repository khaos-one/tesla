using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Tesla.Net
{
    public static class HttpListenerRequestExtensions
    {
        public static bool IsGet(this HttpListenerRequest request)
        {
            return request.HttpMethod == "GET";
        }

        public static bool IsPost(this HttpListenerRequest request)
        {
            return request.HttpMethod == "POST";
        }

        public static NameValueCollection FormData(this HttpListenerRequest request)
        {
            var result = new NameValueCollection();

            if (request.HasEntityBody)
            {
                if (request.ContentType != null &&
                    request.ContentType.ToLowerInvariant() == "application/x-www-form-urlencoded")
                {
                    string body;

                    using (var sr = new StreamReader(request.InputStream, request.ContentEncoding))
                        body = sr.ReadToEnd();

                    result = HttpUtility.ParseQueryString(body);
                }
            }

            return result;
        }

        public static async Task<NameValueCollection> FormDataAsync(this HttpListenerRequest request)
        {
            var result = new NameValueCollection();

            if (request.HasEntityBody)
            {
                if (request.ContentType != null &&
                    request.ContentType.ToLowerInvariant() == "application/x-www-form-urlencoded")
                {
                    string body;

                    using (var sr = new StreamReader(request.InputStream, request.ContentEncoding))
                        body = await sr.ReadToEndAsync();

                    result = HttpUtility.ParseQueryString(body);
                }
            }

            return result;
        }
    }
}
