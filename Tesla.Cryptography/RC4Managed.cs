using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public sealed class RC4Managed
        : SymmetricAlgorithm
    {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new RC4ManagedCryptoTransform(rgbKey);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            return new RC4ManagedCryptoTransform(rgbKey);
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}
