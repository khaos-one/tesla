using System;

namespace Tesla.Cryptography {
    public abstract class GOSTStribog512
        : GOSTStribog {
        public new static GOSTStribog512 Create() {
            return new GOSTStribog512Managed();
        }

        public new static GOSTStribog512 Create(string name) {
            throw new NotSupportedException();
        }

        public static byte[] Compute(byte[] data) {
            using (var h = Create()) {
                return h.ComputeHash(data);
            }
        }

        public static byte[] Compute(byte[] data, int offset, int count) {
            using (var h = Create()) {
                return h.ComputeHash(data, offset, count);
            }
        }
    }
}