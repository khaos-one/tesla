using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public static class AesHelper {
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv) {
            using (var aes = new RijndaelManaged()) {
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

                using (var transform = aes.CreateEncryptor()) {
                    return transform.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv) {
            using (var aes = new RijndaelManaged()) {
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;

                using (var transform = aes.CreateDecryptor()) {
                    return transform.TransformFinalBlock(data, 0, data.Length);
                }
            }
        }
    }
}