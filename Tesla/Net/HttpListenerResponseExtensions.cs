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
            response.OutputStream.Write(str, encoding);
        }

        public static async Task WriteAsync(this HttpListenerResponse response, string str, Encoding encoding)
        {
            await response.OutputStream.WriteAsync(str, encoding);
        }

        public static void Write(this HttpListenerResponse response, string str)
        {
            response.OutputStream.Write(str);
        }

        public static async Task WriteAsync(this HttpListenerResponse response, string str)
        {
            await response.OutputStream.WriteAsync(str);
        }
    }
}
