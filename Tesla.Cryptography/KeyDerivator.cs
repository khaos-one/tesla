using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public static class KeyDerivator {
        public static byte[] DeriveKeyRfc2898(int keySize, byte[] source, byte[] salt, int iterations = 100) {
            /*if (salt == null) {
                salt = StrongNumberProvider.GetBytes(16);
            }*/
            var rfc2898 = new Rfc2898DeriveBytes(source, salt, iterations);
            return rfc2898.GetBytes(keySize);
        }

        public static byte[] DeriveKeyRfc2898(int keySize, string source, byte[] salt, int iterations = 100) {
            /*if (salt == null) {
                salt = StrongNumberProvider.GetBytes(16);
            }*/
            var rfc2898 = new Rfc2898DeriveBytes(source, salt, iterations);
            return rfc2898.GetBytes(keySize);
        }

        public static byte[] DeriveKey(int keySize, byte[] source, byte[] salt = null, int iterations = 100) {
            if (salt == null) {
                salt = new byte[]
                {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f};
            }

            return DeriveKeyRfc2898(keySize, source, salt, iterations);
        }
    }
}
