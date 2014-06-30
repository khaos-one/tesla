using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}
