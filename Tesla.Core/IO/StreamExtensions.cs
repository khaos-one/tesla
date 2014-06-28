using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tesla.IO
{
    public static class StreamExtension
    {
        public static byte[] ReadToTimeout(this Stream stream)
        {
            var bytes = new List<byte>();

            try
            {
                while (true)
                {
                    var read = stream.ReadByte();

                    if (read == -1)
                    {
                        break;
                    }

                    bytes.Add((byte)read);
                }
            }
            catch (TimeoutException) { }
            catch (IOException) { }

            return bytes.ToArray();
        }

        public static void Write(this Stream stream, string str, Encoding encoding)
        {
            var encoded = encoding.GetBytes(str);
            stream.Write(encoded, 0, encoded.Length);
        }

        public static void Write(this Stream stream, string str)
        {
            Write(stream, str, Encoding.UTF8);
        }
    }
}
