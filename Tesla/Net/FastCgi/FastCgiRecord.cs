using System;
using System.IO;
using System.Text;

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

        private const int HeaderLength = 8;
        private const int MaxDataSize = 30000;

        public byte Version;
        public RecordType Type;
        public ushort RequestId;
        public ushort ContentLength;
        public byte PaddingLength;
        public byte Reserved;
        public byte[] ContentData;
        public byte[] PaddingData;

        public byte[] GetEndRequestPacket()
        {
            return new byte[] {
                0x01,
                (byte) RecordType.EndRequest,
                (byte) ((RequestId & 0xFF00) >> 8),
                (byte) ((RequestId & 0xFF)),
                (byte) (( /* Data Length */ 8 & 0xFF00) << 8),
                (byte) (( /* Data Length */ 8 & 0xFF)),
                0x00,
                0x00,
                /* Data */
                0x00,
                0x00,
                0x00,
                0x00,
                0x00, /* Request Complete */
                0x00, /* Reserved */
                0x00, /* Reserved */
                0x00  /* Reserved */
            };
        }

        public byte[] GetResultPackets(string data, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            using (var ms = new MemoryStream())
            using (var w = new BinaryWriter(ms, encoding))
            {
                var dataBytes = encoding.GetBytes(data);
                var totalLen = dataBytes.Length;
                ushort length;
                var dataBytesCursor = 0;

                // Write packets
                for (var i = 0; i < totalLen; i += MaxDataSize)
                {
                    length = (ushort) (totalLen - i);

                    if (length > MaxDataSize)
                        length = MaxDataSize;
                    
                    w.Write((byte)0x01);
                    w.Write((byte)RecordType.StdOut);
                    w.Write((byte)((RequestId & 0xFF00) >> 8));
                    w.Write((byte)(RequestId & 0x00FF));
                    w.Write((byte)((length & 0xFF00) >> 8));
                    w.Write((byte)(length & 0x00FF));
                    w.Write((ushort)0x0000);
                    w.Write(dataBytes, dataBytesCursor, length);
                }

                // Write stream EOF
                w.Write(new byte[] {
                    0x01,
                    (byte)RecordType.StdOut,
                    (byte)((RequestId & 0xFF00) >> 8),
                    (byte)(RequestId & 0x00FF),
                    0x00, 0x00, 0x00, 0x00
                });

                // Write END_REQUEST record
                w.Write(GetEndRequestPacket());

                return ms.ToArray();
            }
        }

        public static bool TryParse(Stream stream, out FastCgiRecord record)
        {
            var result = new FastCgiRecord();

            using (var r = new BinaryReader(stream, Encoding.ASCII, true))
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
