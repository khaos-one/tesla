using System;
using System.IO;
using System.Text;

namespace Tesla.Net.FastCgi
{
    /// <summary>
    /// Basic FastCGI protocol's name-value pair.
    /// </summary>
    public sealed class FastCgiNameValuePair
    {
        public string Name;
        public string Value;
        public bool IsEnd;

        public FastCgiNameValuePair(FastCgiRecord record)
        {
            if (record.Type != FastCgiRecord.RecordType.Params)
                throw new ArgumentException("record");

            if (record.ContentLength == 0)
            {
                IsEnd = true;
                return;
            }

            using (var ms = new MemoryStream(record.ContentData, false))
            using (var r = new BinaryReader(ms, Encoding.ASCII))
            {
                var nameLength = ReadLengthBytes(r);
                var valueLength = ReadLengthBytes(r);
                var nameBytes = r.ReadBytes((int)nameLength);
                var valueBytes = r.ReadBytes((int)valueLength);
                Name = Encoding.ASCII.GetString(nameBytes);
                Value = Encoding.ASCII.GetString(valueBytes);
            }
        }

        private static uint ReadLengthBytes(BinaryReader r)
        {
            byte first = r.ReadByte();

            if ((first & 0x80) > 0)
            {
                byte b, c, d;
                b = r.ReadByte();
                c = r.ReadByte();
                d = r.ReadByte();
                return (uint)(((uint)((first & 0x7f) << 24) & 0xff000000) | ((uint)(b << 16) & 0x00FF0000) | ((uint)(c << 8) & 0x0000ff00) | (uint)d);
            }

            return first;
        }
    }
}
