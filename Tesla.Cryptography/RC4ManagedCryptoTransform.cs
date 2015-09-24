//-----------------------------------------------------------------------------------------
// <author>Egor 'khaos' Zelensky <i@khaos.su></author>
// <description>
//    This file originates from 
//    <a href="https://github.com/khaos-one/tesla/tree/master/Tesla.Cryptography">Tesla</a>
//    library.
// </description>
//-----------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Cryptography;

namespace Tesla.Cryptography {
    public sealed class RC4ManagedCryptoTransform
        : ICryptoTransform {
        private byte[] _key;
        private byte[] _s = new byte[256];

        public RC4ManagedCryptoTransform(byte[] key) {
            _key = key;

            KeySchedule();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
            int outputOffset) {
            for (int i = 0, j = 0; i < inputCount; i++) {
                outputBuffer[i + outputOffset] = (byte) (inputBuffer[i + inputOffset] ^ GetKeyWord(i, j));
            }

            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {
            var output = new byte[inputCount];

            for (int i = 0, j = 0; i < inputCount; i++) {
                output[i] = (byte) (inputBuffer[i + inputOffset] ^ GetKeyWord(i, j));
            }

            KeySchedule();
            return output;
        }

        public int InputBlockSize {
            get { return 1; }
        }

        public int OutputBlockSize {
            get { return 1; }
        }

        public bool CanTransformMultipleBlocks {
            get { return true; }
        }

        public bool CanReuseTransform {
            get { return true; }
        }

        public void Dispose() {
            _key = null;
            _s = null;
        }

        private static void Swap(IList<byte> arr, int i1, int i2) {
            var t = arr[i1];
            arr[i1] = arr[i2];
            arr[i2] = t;
        }

        private void KeySchedule() {
            for (var i = 0; i < 256; i++)
                _s[i] = (byte) i;

            for (int i = 0, j = 0; i < 256; i++) {
                j = (j + _s[i] + _key[i%_key.Length])%256;
                Swap(_s, i, j);
            }
        }

        private byte GetKeyWord(int i, int j) {
            i = (i + 1)%256;
            j = (j + _s[i])%256;
            Swap(_s, i, j);

            return _s[(_s[i] + _s[j])%256];
        }
    }
}