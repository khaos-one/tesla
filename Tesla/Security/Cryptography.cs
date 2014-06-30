using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Security
{
    public static class Cryptography
    {
        public static byte[] ComputeSha256Hash(byte[] target, int offset = 0)
        {
            using (var h = SHA256.Create())
                return h.ComputeHash(target, offset, target.Length - offset);
        }

        public static byte[] ComputeSha256Hash(string target, Encoding encoding)
        {
            return ComputeSha256Hash(encoding.GetBytes(target));
        }

        public static byte[] ComputeSha256Hash(string target)
        {
            return ComputeSha256Hash(Encoding.UTF8.GetBytes(target));
        }
    }
}
