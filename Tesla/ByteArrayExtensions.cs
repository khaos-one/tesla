using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Tesla
{
    public static class ByteArrayExtensions
    {
        public static string ToString(this byte[] array, Encoding encoding)
        {
            return encoding.GetString(array);
        }

        public static string ToString(this byte[] array)
        {
            return ToString(array, Encoding.UTF8);
        }

        public static string ToHexString(this byte[] array)
        {
            unchecked
            {
                var c = new char[array.Length*2];

                for (int bx = 0, cx = 0; bx < array.Length; ++bx, ++cx)
                {
                    var b = ((byte) (array[bx] >> 4));
                    c[cx] = (char) (b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                    b = ((byte) (array[bx] & 0x0F));
                    c[++cx] = (char) (b > 9 ? b + 0x37 + 0x20 : b + 0x30);
                }

                return new string(c);
            }
        }

        public static string ToHexString(this byte[] array, char delimeter)
        {
            unchecked
            {
                var l = array.Length*3;
                var c = new char[l];

                for (int bx = 0, cx = 0; bx < array.Length; ++bx, ++cx)
                {
                    var b = ((byte)(array[bx] >> 4));
                    c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                    b = ((byte)(array[bx] & 0x0F));
                    c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                    c[++cx] = delimeter;
                }

                return new string(c, 0, l - 1);
            }
        }

        public static void BlockCopyTo(this byte[] array, [In, Out] byte[] target, int sourceOffset = 0, int targetOffset = 0)
        {
            Buffer.BlockCopy(array, sourceOffset, target, targetOffset, array.Length);
        }

        public static void BlockCopyFrom(this byte[] array, [In, Out] byte[] target, int sourceOffset = 0,
            int targetOffset = 0)
        {
            Buffer.BlockCopy(target, targetOffset, array, sourceOffset, target.Length);
        }
    }
}
