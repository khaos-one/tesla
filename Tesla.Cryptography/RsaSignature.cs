using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public static class RsaSignature {
        public static byte[] Create(byte[] data, string privateKeyXml, string hashAlgorithmName) {
            byte[] hash;

            using (var rsa = new RSACryptoServiceProvider()) {
                try {
                    rsa.FromXmlString(privateKeyXml);
                    hash = rsa.SignData(data, hashAlgorithmName);
                }
                finally {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return hash;
        }

        public static bool Verify(byte[] data, byte[] hash, string publicKeyXml, string hashAlgorithmName) {
            bool result;

            using (var rsa = new RSACryptoServiceProvider()) {
                try {
                    rsa.FromXmlString(publicKeyXml);
                    result = rsa.VerifyData(data, hashAlgorithmName, hash);
                }
                finally {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }

        public static byte[] CreateSha256(byte[] data, string privateKeyXml) {
            return Create(data, privateKeyXml, "SHA256");
        }

        public static bool VerifySha256(byte[] data, byte[] hash, string publicKeyXml) {
            return Verify(data, hash, publicKeyXml, "SHA256");
        }
    }
}