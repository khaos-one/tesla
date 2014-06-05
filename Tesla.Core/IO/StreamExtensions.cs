using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
