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
    }
}
