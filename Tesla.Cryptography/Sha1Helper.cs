using System.Security.Cryptography;
using System.Text;

namespace Tesla.Cryptography {
    public static class Sha1Helper {
        public static byte[] ComputeSha1(byte[] input) {
            using (var sha1 = SHA1.Create()) {
                return sha1.ComputeHash(input);
            }
        }

        public static byte[] ComputeSha1(string input, Encoding encoding = null) {
            if (encoding == null) {
                encoding = Encoding.Default;
            }

            return ComputeSha1(encoding.GetBytes(input));
        }
    }
}
