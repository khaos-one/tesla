using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public class StrongNumberProvider
    {
        private static readonly RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();

        public static int Int32 { get { return GetInt32(); } }
        public static uint UInt32 { get { return GetUInt32(); } }
        public static float Single { get { return GetSingle(); } }
        public static long Int64 { get { return GetInt64(); } }
        public static ulong UInt64 { get { return GetUInt64(); } }

        public uint NextUInt32()
        {
            var res = new byte[sizeof(UInt32)];
            csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public int NextInt()
        {
            var res = new byte[sizeof(Int32)];
            csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public float NextSingle()
        {
            float numerator = NextUInt32();
            float denominator = uint.MaxValue;
            return numerator / denominator;
        }

        public long NextInt64()
        {
            var res = new byte[sizeof(Int64)];
            csp.GetBytes(res);
            return BitConverter.ToInt64(res, 0);
        }

        public ulong NextUInt64()
        {
            var res = new byte[sizeof(UInt64)];
            csp.GetBytes(res);
            return BitConverter.ToUInt64(res, 0);
        }

        public byte[] NextBytes(int count)
        {
            var result = new byte[count];
            csp.GetBytes(result);
            return result;
        }

        public static uint GetUInt32()
        {
            var res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public static int GetInt32()
        {
            var res = new byte[4];
            csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public static float GetSingle()
        {
            float numerator = GetUInt32();
            float denominator = uint.MaxValue;
            return numerator / denominator;
        }

        public static long GetInt64()
        {
            var res = new byte[sizeof(Int64)];
            csp.GetBytes(res);
            return BitConverter.ToInt64(res, 0);
        }

        public static ulong GetUInt64()
        {
            var res = new byte[sizeof(UInt64)];
            csp.GetBytes(res);
            return BitConverter.ToUInt64(res, 0);
        }

        public static byte[] GetBytes(int count)
        {
            var result = new byte[count];
            csp.GetBytes(result);
            return result;
        }
    }
}
