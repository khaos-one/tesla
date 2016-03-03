using System;
using System.Collections;

namespace Tesla.Extensions {
    public static class BitArrayExtensions {
        public static void SetValue(this BitArray self, int startIndex, int value, int length) {
            if (startIndex + length > self.Length) {
                throw new ArgumentException("startIndex + length");
            }

            for (var i = 0; i < length; i++) {
                var v = ((1 << i) & value) != 0;
                self.Set(startIndex + i, v);
            }
        }
    }
}