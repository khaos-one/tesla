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
            var len = encoding.GetByteCount(str);
            var encoded = ProgramBuffer.Manager.TakeBuffer(len);
            Array.Clear(encoded, 0, encoded.Length);
            encoding.GetBytes(str, 0, str.Length, encoded, 0);
            stream.Write(encoded, 0, encoded.Length);
            ProgramBuffer.Manager.ReturnBuffer(encoded);
        }

        public static void Write(this Stream stream, string str)
        {
            Write(stream, str, Encoding.UTF8);
        }

        public static void WriteFile(this Stream stream, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fs.CopyTo(stream);
        }

        public static async void WriteFileAsync(this Stream stream, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                await fs.CopyToAsync(stream);
        }
    }
}
