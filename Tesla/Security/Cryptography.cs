using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Security {
    public static class Cryptography {
        public static byte[] ComputeSha256Hash(byte[] target, int offset = 0) {
            using (var h = SHA256.Create())
                return h.ComputeHash(target, offset, target.Length - offset);
        }

        public static byte[] ComputeSha256Hash(string target, Encoding encoding) {
            return ComputeSha256Hash(target.ToBytes(encoding));
        }

        public static byte[] ComputeSha256Hash(string target) {
            return ComputeSha256Hash(target.ToBytes());
        }

        public static byte[] DiffieHellmanNegotiate(Stream s) {
            using (var dh = new ECDiffieHellmanCng()) {
                dh.KeySize = 256;
                dh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                dh.HashAlgorithm = CngAlgorithm.Sha256;

                var publicKeyEccBlob = dh.PublicKey.ToByteArray();
                var otherPublicKeyEccBlob = new byte[publicKeyEccBlob.Length];

                s.Write(publicKeyEccBlob, 0, publicKeyEccBlob.Length);
                s.Read(otherPublicKeyEccBlob, 0, otherPublicKeyEccBlob.Length);

                var key = CngKey.Import(otherPublicKeyEccBlob, CngKeyBlobFormat.EccPublicBlob);
                return dh.DeriveKeyMaterial(key);
            }
        }

        public static async Task<byte[]> DiffieHellmanNegotiateAsync(Stream s) {
            using (var dh = new ECDiffieHellmanCng()) {
                dh.KeySize = 256;
                dh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                dh.HashAlgorithm = CngAlgorithm.Sha256;

                var publicKeyEccBlob = dh.PublicKey.ToByteArray();
                var otherPublicKeyEccBlob = new byte[publicKeyEccBlob.Length];

                await s.WriteAsync(publicKeyEccBlob, 0, publicKeyEccBlob.Length);
                await s.ReadAsync(otherPublicKeyEccBlob, 0, otherPublicKeyEccBlob.Length);

                var key = CngKey.Import(otherPublicKeyEccBlob, CngKeyBlobFormat.EccPublicBlob);
                return dh.DeriveKeyMaterial(key);
            }
        }
    }
}