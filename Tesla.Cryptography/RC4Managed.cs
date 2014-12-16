using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace Tesla.Cryptography
{
    public sealed class RC4Managed
        : HashAlgorithm
    {
        private byte[] _originalKey;
        private byte[] _key;
        private byte[] _encoded;

        public RC4Managed(byte[] key)
        {
            _originalKey = key;
            _encoded = null;
        }

        public override bool CanReuseTransform
        {
            get { return true; }
        }

        public override bool CanTransformMultipleBlocks
        {
            get { return false; }
        }

        private void Swap(IList<byte> array, int idx1, int idx2)
        {
            var t = array[idx1];
            array[idx1] = array[idx2];
            array[idx2] = t;
        }

        private byte PseudorandomByte()
        {
            int i = 0, j = 0;

            i = (i + 1)%256;
            j = (j + _key[i])%256;

            Swap(_key, i, j);

            return _key[(_key[i] + _key[j])%256];
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            _encoded = new byte[cbSize];

            for (var i = ibStart; i < ibStart + cbSize; i++)
            {
                _encoded[i] = (byte) (_encoded[i] ^ PseudorandomByte());
            }
        }

        protected override byte[] HashFinal()
        {
            return _encoded;
        }

        public override void Initialize()
        {
            for (var i = 0; i < 256; i++)
            {
                _key[i] = (byte) i;
            }

            var j = 0;

            for (var i = 0; i < 256; i++)
            {
                j = (j + _key[i] + _originalKey[i%_originalKey.Length])%256;
                Swap(_key, i, j);
            }
        }
    }
}
