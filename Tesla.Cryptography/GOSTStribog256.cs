using System;

namespace Tesla.Cryptography
{
    public abstract class GOSTStribog256
        : GOSTStribog
    {
        new public static GOSTStribog256 Create()
        {
            return new GOSTStribog256Managed();
        }

        new public static GOSTStribog256 Create(string name)
        {
            throw new NotSupportedException();
        }

        public static byte[] Compute(byte[] data)
        {
            using (var h = Create())
            {
                return h.ComputeHash(data);
            }
        }

        public static byte[] Compute(byte[] data, int offset, int count)
        {
            using (var h = Create())
            {
                return h.ComputeHash(data, offset, count);
            }
        }
    }
}
