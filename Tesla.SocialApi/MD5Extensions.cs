using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tesla.SocialApi
{
    public static class MD5Extensions
    { 
        public static string HexDigest(string source, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (var t = new MD5CryptoServiceProvider())
                return BitConverter.ToString(t.ComputeHash(encoding.GetBytes(source))).Replace("-", "").ToLowerInvariant();
        }
    }
}
