using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            //var len = encoding.GetByteCount(str);
            //var encoded = ProgramBuffer.Manager.TakeBuffer(len);
            //Array.Clear(encoded, 0, encoded.Length);
            //encoding.GetBytes(str, 0, str.Length, encoded, 0);
            //stream.Write(encoded, 0, encoded.Length);
            //ProgramBuffer.Manager.ReturnBuffer(encoded);

            var encoded = str.ToBytes(encoding);
            stream.Write(encoded, 0, encoded.Length);
        }

        public static async Task WriteAsync(this Stream stream, string str, Encoding encoding)
        {
            //var len = encoding.GetByteCount(str);
            //var encoded = ProgramBuffer.Manager.TakeBuffer(len);
            //Array.Clear(encoded, 0, encoded.Length);
            //encoding.GetBytes(str, 0, str.Length, encoded, 0);
            //await stream.WriteAsync(encoded, 0, encoded.Length);
            //ProgramBuffer.Manager.ReturnBuffer(encoded);

            var encoded = str.ToBytes(encoding);
            await stream.WriteAsync(encoded, 0, encoded.Length);
        }

        public static void Write(this Stream stream, string str)
        {
            Write(stream, str, Encoding.UTF8);
        }

        public static async Task WriteAsync(this Stream stream, string str)
        {
            await WriteAsync(stream, str, Encoding.UTF8);
        }

        public static void WriteFile(this Stream stream, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fs.CopyTo(stream);
        }

        public static async Task WriteFileAsync(this Stream stream, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                await fs.CopyToAsync(stream);
        }

        public static string ReadString(this Stream stream, Encoding encoding)
        {
            string result;

            using (var sr = new StreamReader(stream, encoding))
                result = sr.ReadToEnd();

            return result;
        }

        public static async Task<string> ReadStringAsync(this Stream stream)
        {
            string result;

            using (var sr = new StreamReader(stream, Encoding.UTF8))
                result = await sr.ReadToEndAsync();

            return result;
        }
    }
}
