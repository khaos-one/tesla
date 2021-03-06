﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesla.Extensions;

namespace Tesla.IO {
    public static class StreamExtension {
        public static byte[] ReadToTimeout(this Stream stream) {
            var bytes = new List<byte>();

            try {
                while (true) {
                    var read = stream.ReadByte();

                    if (read == -1) {
                        break;
                    }

                    bytes.Add((byte) read);
                }
            }
            catch (TimeoutException) {}
            catch (IOException) {}

            return bytes.ToArray();
        }

        public static byte[] ReadToTimeout(this Stream stream, int maxCount) {
            var bytes = new List<byte>(maxCount);
            var count = 0;

            try {
                do {
                    var read = stream.ReadByte();

                    if (read == -1) {
                        break;
                    }

                    bytes.Add((byte) read);
                    count++;
                } while (count < maxCount);
            }
            catch (TimeoutException) {}
            catch (IOException) {}

            return bytes.ToArray();
        }

        public static int ReadToTimeout(this Stream stream, [In, Out] byte[] buffer, int offset = 0) {
            var count = 0;

            try {
                while (count + offset < buffer.Length) {
                    var read = stream.ReadByte();

                    if (read == -1)
                        break;

                    buffer[count + offset] = (byte) read;
                    count++;
                }
            }
            catch (TimeoutException) {}
            catch (IOException) {}

            return count;
        }

        public static void Write(this Stream stream, byte[] bytes) {
            stream.Write(bytes, 0, bytes.Length);
        }

        public static void Write(this Stream stream, string str, Encoding encoding) {
            //var len = encoding.GetByteCount(str);
            //var encoded = ProgramBuffer.Manager.TakeBuffer(len);
            //Array.Clear(encoded, 0, encoded.Length);
            //encoding.GetBytes(str, 0, str.Length, encoded, 0);
            //stream.Write(encoded, 0, encoded.Length);
            //ProgramBuffer.Manager.ReturnBuffer(encoded);

            var encoded = str.ToBytes(encoding);
            stream.Write(encoded, 0, encoded.Length);
        }

        public static async Task WriteAsync(this Stream stream, string str, Encoding encoding) {
            //var len = encoding.GetByteCount(str);
            //var encoded = ProgramBuffer.Manager.TakeBuffer(len);
            //Array.Clear(encoded, 0, encoded.Length);
            //encoding.GetBytes(str, 0, str.Length, encoded, 0);
            //await stream.WriteAsync(encoded, 0, encoded.Length);
            //ProgramBuffer.Manager.ReturnBuffer(encoded);

            var encoded = str.ToBytes(encoding);
            await stream.WriteAsync(encoded, 0, encoded.Length);
        }

        public static void Write(this Stream stream, string str) {
            Write(stream, str, Encoding.UTF8);
        }

        public static async Task WriteAsync(this Stream stream, string str) {
            await WriteAsync(stream, str, Encoding.UTF8);
        }

        public static void WriteFile(this Stream stream, string filePath) {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                fs.CopyTo(stream);
        }

        public static async Task WriteFileAsync(this Stream stream, string filePath) {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                await fs.CopyToAsync(stream);
        }

        public static string ReadString(this Stream stream, Encoding encoding) {
            string result;

            using (var sr = new StreamReader(stream, encoding))
                result = sr.ReadToEnd();

            return result;
        }

        public static string ReadString(this Stream stream) {
            string result;

            using (var sr = new StreamReader(stream, Encoding.UTF8))
                result = sr.ReadToEnd();

            return result;
        }

        public static async Task<string> ReadStringAsync(this Stream stream) {
            string result;

            using (var sr = new StreamReader(stream, Encoding.UTF8))
                result = await sr.ReadToEndAsync();

            return result;
        }

        public static int Read(this Stream stream, [In, Out] byte[] buffer, int offset = 0) {
            return stream.Read(buffer, offset, buffer.Length - offset);
        }

        public static async Task<int> ReadAsync(this Stream stream, [In, Out] byte[] buffer, int offset = 0) {
            return await stream.ReadAsync(buffer, offset, buffer.Length - offset);
        }

        public static byte[] ReadBytes(this Stream stream, int count, int offset = 0) {
            var buffer = new byte[count];
            stream.Read(buffer, offset);
            return buffer;
        }

        public static async Task<byte[]> ReadBytesAsync(this Stream stream, int count, int offset = 0) {
            var buffer = new byte[count];
            await stream.ReadAsync(buffer, offset);
            return buffer;
        }

        public static int ReadInt32(this Stream stream) {
            return BitConverter.ToInt32(stream.ReadBytes(sizeof (int)), 0);
        }

        public static async Task<int> ReadInt32Async(this Stream stream) {
            return BitConverter.ToInt32(await stream.ReadBytesAsync(sizeof (int)), 0);
        }
    }
}