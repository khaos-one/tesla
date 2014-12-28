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
    {
        private readonly byte[] _originalKey;
        private readonly byte[] _s;
        private byte[] _encoded;

        public RC4Managed(byte[] key)
        {
            _originalKey = key;
            _s = new byte[256];
            _encoded = null;

            Reset();
        }

        private void Swap(IList<byte> array, int idx1, int idx2)
        {
            var t = array[idx1];
            array[idx1] = array[idx2];
            array[idx2] = t;
        }

        private byte KeyWord(int i, int j)
        {
            i = (i + 1)%256;
            j = (j + _s[i])%256;

            Swap(_s, i, j);

            return _s[(_s[i] + _s[j])%256];
        }

        public byte[] TransformFull(IList<byte> array)
        {
            return TransformFull(array, 0, array.Count);
        }

        public byte[] TransformFull(IList<byte> array, int ibStart, int cbSize)
        {
            Reset();

            var ciphertext = new byte[cbSize];

            for (int i = ibStart, j = 0; i < ibStart + cbSize; i++)
            {
                ciphertext[i] = (byte) (array[i] ^ KeyWord(i, j));
            }

            return ciphertext;
        }

        public byte[] TransformBlock(IList<byte> array)
        {
            return TransformBlock(array, 0, array.Count);
        }

        public byte[] TransformBlock(IList<byte> array, int ibStart, int cbSize)
        {
            var ciphertext = new byte[cbSize];

            for (int i = ibStart, j = 0; i < ibStart + cbSize; i++)
            {
                ciphertext[i] = (byte)(array[i] ^ KeyWord(i, j));
            }

            return ciphertext;
        }

        public void Reset()
        {
            for (var i = 0; i < 256; i++)
            {
                _s[i] = (byte) i;
            }

            var j = 0;

            for (var i = 0; i < 256; i++)
            {
                j = (j + _s[i] + _originalKey[i%_originalKey.Length])%256;
                Swap(_s, i, j);
            }
        }
    }
}
