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
    public sealed class StrongNumberProvider {
        private static readonly RNGCryptoServiceProvider Csp = new RNGCryptoServiceProvider();

        public static int Int32 => GetInt32();

        public static uint UInt32 => GetUInt32();

        public static float Single => GetSingle();

        public static long Int64 => GetInt64();

        public static ulong UInt64 => GetUInt64();

        public static float Float => GetFloat();

        public uint NextUInt32() {
            var res = new byte[sizeof (uint)];
            Csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public int NextInt() {
            var res = new byte[sizeof (int)];
            Csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public float NextSingle() {
            float numerator = NextUInt32();
            float denominator = uint.MaxValue;
            return numerator/denominator;
        }

        public long NextInt64() {
            var res = new byte[sizeof (long)];
            Csp.GetBytes(res);
            return BitConverter.ToInt64(res, 0);
        }

        public ulong NextUInt64() {
            var res = new byte[sizeof (ulong)];
            Csp.GetBytes(res);
            return BitConverter.ToUInt64(res, 0);
        }

        public byte[] NextBytes(int count) {
            var result = new byte[count];
            Csp.GetBytes(result);
            return result;
        }

        public static uint GetUInt32() {
            var res = new byte[4];
            Csp.GetBytes(res);
            return BitConverter.ToUInt32(res, 0);
        }

        public static int GetInt32() {
            var res = new byte[4];
            Csp.GetBytes(res);
            return BitConverter.ToInt32(res, 0);
        }

        public static float GetSingle() {
            float numerator = GetUInt32();
            float denominator = uint.MaxValue;
            return numerator/denominator;
        }

        public static long GetInt64() {
            var res = new byte[sizeof (long)];
            Csp.GetBytes(res);
            return BitConverter.ToInt64(res, 0);
        }

        public static ulong GetUInt64() {
            var res = new byte[sizeof (ulong)];
            Csp.GetBytes(res);
            return BitConverter.ToUInt64(res, 0);
        }

        public static byte[] GetBytes(int count) {
            var result = new byte[count];
            Csp.GetBytes(result);
            return result;
        }

        public static float GetFloat() {
            var res = new byte[sizeof (float)];
            Csp.GetBytes(res);
            return BitConverter.ToSingle(res, 0);
        }

        public static string GetString(int length) {
            var res = new byte[length*sizeof (char)];
            Csp.GetBytes(res);
            var chars = new char[length];

            for (var i = 0; i < chars.Length; i++) {
                chars[i] = (char) res[i*sizeof (char)];
            }

            return new string(chars);
        }
    }
}
