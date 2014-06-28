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
        public static void Write(this HttpListenerResponse response, String str, Encoding encoding)
        {
            response.OutputStream.Write(str, encoding);
        }

        public static void Write(this HttpListenerResponse response, String str)
        {
            response.OutputStream.Write(str);
        }
    }
}
