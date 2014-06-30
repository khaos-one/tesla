using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tesla.IO;

namespace Tesla.Net
{
    public static class HttpListenerResponseExtensions
    {
        public static void Write(this HttpListenerResponse response, string str, Encoding encoding)
        {
            var encoded = str.ToBytes(encoding);
            response.ContentLength64 += encoded.Length;
            response.OutputStream.Write(encoded, 0, encoded.Length);
        }

        public static async Task WriteAsync(this HttpListenerResponse response, string str, Encoding encoding)
        {
            var encoded = str.ToBytes(encoding);
            response.ContentLength64 += encoded.Length;
            await response.OutputStream.WriteAsync(encoded, 0, encoded.Length);
        }

        public static void Write(this HttpListenerResponse response, string str)
        {
            Write(response, str, Encoding.UTF8);
        }

        public static async Task WriteAsync(this HttpListenerResponse response, string str)
        {
            await WriteAsync(response, str, Encoding.UTF8);
        }

        public static void RemoveCookie(this HttpListenerResponse response, string cookieName)
        {
            response.SetCookie(new Cookie(cookieName, string.Empty) { Expired = true });
        }
    }
}
