using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public class StrongNumberProvider
    {
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

        public uint NextUInt32()
        {
            var res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public int NextInt()
        {
            var res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public Single NextSingle()
        {
            float numerator = NextUInt32();
            float denominator = uint.MaxValue;
            return numerator / denominator;
        }

        public byte[] NextBytes(int count)
        {
            var result = new byte[count];
            csp.GetBytes(result);
            return result;
        }
    }
}
