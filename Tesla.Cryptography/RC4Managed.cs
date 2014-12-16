﻿using System;
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
        }

        private void Swap(IList<byte> array, int idx1, int idx2)
        {
            var t = array[idx1];
            array[idx1] = array[idx2];
            array[idx2] = t;
        }

        private byte PseudorandomWord()
        {
            int i = 0, j = 0;

            i = (i + 1)%256;
            j = (j + _s[i])%256;

            Swap(_s, i, j);

            return _s[(_s[i] + _s[j])%256];
        }

        public byte[] Transform(IList<byte> array)
        {
            return Transform(array, 0, array.Count);
        }

        public byte[] Transform(IList<byte> array, int ibStart, int cbSize)
        {
            Initialize();

            var ciphertext = new byte[cbSize];

            for (var i = ibStart; i < ibStart + cbSize; i++)
            {
                ciphertext[i] = (byte) (array[i] ^ PseudorandomWord());
            }

            return ciphertext;
        }

        private void Initialize()
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
