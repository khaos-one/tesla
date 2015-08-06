using System;
using System.IO;
using System.Text;

namespace Tesla.Net.FastCgi
{
    /// <summary>
    /// Represents data for BEGIN_REQUEST records.
    /// <see cref="!:http://www.fastcgi.com/devkit/doc/fcgi-spec.html#S5.1"/>
    /// </summary>
    public sealed class FastCgiBeginRequest
    {
        public FastCgiRole Role;
        public byte Flags;
        public byte[] Reserved;

        public FastCgiBeginRequest(FastCgiRecord record)
        {
            if (record.Type != FastCgiRecord.RecordType.BeginRequest)
                throw new ArgumentException("record");

            using (var ms = new MemoryStream(record.ContentData, false))
            using (var r = new BinaryReader(ms, Encoding.ASCII))
            {
                ushort a, b;
                a = r.ReadByte();
                b = r.ReadByte();

                Role = (FastCgiRole)(ushort)(((a << 8) & (ushort)(0xff00)) | b);
                Flags = r.ReadByte();
                Reserved = r.ReadBytes(5);
            }
        }
    }
}
