using System;
using System.IO;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x03)]
    public struct UInt64
        : IEquatable<System.UInt64>,
          IRecord
    {
        private System.UInt64 _value;

        public UInt64(System.UInt64 value)
        {
            _value = value;
        }

        public System.UInt64 Value { get { return _value; } }

        public static implicit operator UInt64(System.UInt64 v)
        {
            return new UInt64(v);
        }

        public static implicit operator System.UInt64(UInt64 v)
        {
            return v._value;
        }

        public bool Equals(ulong other)
        {
            return _value.Equals(other);
        }

        public byte RecordId { get { return 0x03; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            _value = reader.ReadUInt64();
        }
    }
}
