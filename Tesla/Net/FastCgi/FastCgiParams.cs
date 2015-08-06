using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Net.FastCgi
{
    public sealed class FastCgiParams
        : Dictionary<string, string>
    {
        public bool EndReached { get; set; }

        public FastCgiParams()
        { }

        public void AddRecord(FastCgiRecord record)
        {
            if (record.Type != FastCgiRecord.RecordType.Params)
                throw new ArgumentException("record");

            if (record.ContentLength == 0)
            {
                EndReached = true;
                return;
            }

            using (var ms = new MemoryStream(record.ContentData, false))
            using (var r = new BinaryReader(ms, Encoding.ASCII))
            {
                var nameLength = ReadLengthBytes(r);
                var valueLength = ReadLengthBytes(r);
                var nameBytes = r.ReadBytes((int)nameLength);
                var valueBytes = r.ReadBytes((int)valueLength);
                Add(Encoding.ASCII.GetString(nameBytes), Encoding.ASCII.GetString(valueBytes));
            }
        }

        /// <summary>
        /// Reads variable length value.
        /// </summary>
        /// <param name="r">Reader to read from.</param>
        /// <returns>Read length.</returns>
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
