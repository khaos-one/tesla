using System;
using System.IO;

namespace Tesla.Net.FastCgi
{
    /// <summary>
    /// Base record structure.
    /// <see cref="!:http://www.fastcgi.com/devkit/doc/fcgi-spec.html#S3.3"/>
    /// </summary>
    public sealed class FastCgiRecord
    {
        public enum RecordType
        {
            BeginRequest = 1,
            AbortRequest = 2,
            EndRequest = 3,
            Params = 4,
            StdIn = 5,
            StdOut = 6,
            StdErr = 7,
            Data = 8,
            GetValues = 9,
            GetValuesResult = 10,
            Unknown = 11,

            BlankRecord = 99
        }

        public const int HeaderLength = 8;
        public byte Version;
        public RecordType Type;
        public ushort RequestId;
        public ushort ContentLength;
        public byte PaddingLength;
        public byte Reserved;
        public byte[] ContentData;
        public byte[] PaddingData;

        public FastCgiNameValuePair BlankRecordBody;

        public static bool TryParse(Stream stream, out FastCgiRecord record)
        {
            var result = new FastCgiRecord();

            using (var r = new BinaryReader(stream))
            {
                result.Version = r.ReadByte();

                if (result.Version > 0)
                {
                    result.Type = (RecordType)r.ReadByte();
                    // Ensuring endianness
                    ushort a, b;
                    a = r.ReadByte();
                    b = r.ReadByte();
                    result.RequestId = (ushort)(((a << 8) & 0xFF00) | b);
                    a = r.ReadByte();
                    b = r.ReadByte();
                    result.ContentLength = (ushort)(((a << 8) & (0xFF00)) | b);
                    result.PaddingLength = r.ReadByte();
                    result.Reserved = r.ReadByte();

                    // Read the data
                    result.ContentData = r.ReadBytes(result.ContentLength);
                    result.PaddingData = r.ReadBytes(result.PaddingLength);

                    record = result;
                    return true;
                }
                else
                {
                    // Ignore other unsupported record versions
                    record = null;
                    return false;
                }
            }
        }
    }
}
