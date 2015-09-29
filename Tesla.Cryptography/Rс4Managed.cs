//-----------------------------------------------------------------------------------------
// <author>Egor 'khaos' Zelensky <i@khaos.su></author>
// <description>
//    This file originates from 
//    <a href="https://github.com/khaos-one/tesla/tree/master/Tesla.Cryptography">Tesla</a>
//    library.
// </description>
//-----------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public sealed class Rс4Managed
        : SymmetricAlgorithm {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV) {
            return new Rc4ManagedCryptoTransform(rgbKey);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV) {
            return new Rc4ManagedCryptoTransform(rgbKey);
        }

        public override void GenerateIV() {
            throw new NotImplementedException();
        }

        public override void GenerateKey() {
            throw new NotImplementedException();
        }
    }
}