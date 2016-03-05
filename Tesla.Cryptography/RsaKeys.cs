using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public sealed class RsaKeys {
        public RSAParameters FullParameters { get; private set; }
        public string PrivateKeyXml { get; private set; }
        public string PublicKeyXml { get; private set; }

        public byte[] DerivePublicKeyMaterial(int keyLengthInBits) {
            var result = new byte[keyLengthInBits/8];
            Buffer.BlockCopy(FullParameters.Modulus, 0, result, 0, result.Length);
            return result;
        }

        public static RsaKeys CreateFromPublicKeyXml(string publicKeyXml) {
            var result = new RsaKeys();

            using (var rsa = new RSACryptoServiceProvider(2048)) {
                try {
                    rsa.FromXmlString(publicKeyXml);
                    result.FullParameters = rsa.ExportParameters(false);
                    result.PublicKeyXml = publicKeyXml;
                }
                finally {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }

        public static RsaKeys CreateFromPrivateKeyXml(string privateKeyXml) {
            var result = new RsaKeys();

            using (var rsa = new RSACryptoServiceProvider(2048)) {
                try {
                    rsa.FromXmlString(privateKeyXml);
                    result.FullParameters = rsa.ExportParameters(true);
                    result.PrivateKeyXml = privateKeyXml;
                    result.PublicKeyXml = rsa.ToXmlString(false);
                }
                finally {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }

        public static RsaKeys Generate() {
            var result = new RsaKeys();

            using (var rsa = new RSACryptoServiceProvider(2048)) {
                try {
                    result.FullParameters = rsa.ExportParameters(true);
                    result.PrivateKeyXml = rsa.ToXmlString(true);
                    result.PublicKeyXml = rsa.ToXmlString(false);
                }
                finally {
                    rsa.PersistKeyInCsp = false;
                }
            }

            return result;
        }
    }
}
