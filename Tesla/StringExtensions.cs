using System.Text;

namespace Tesla
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        public static byte[] ToBytes(this string str)
        {
            return ToBytes(str, Encoding.UTF8);
        }
    }
}
