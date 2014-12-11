using System;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x01)]
    public class UInt16
        : IEquatable<System.UInt16>,
          IRecord
    {
        private System.UInt16 _value;

        public UInt16(System.UInt16 value)
        {
            _value = value;
        }

        public System.UInt16 Value { get { return _value; } }

        public static implicit operator UInt16(System.UInt16 v)
        {
            return new UInt16(v);
        }

        public static implicit operator System.UInt16(UInt16 v)
        {
            return v._value;
        }

        public bool Equals(ushort other)
        {
            return _value.Equals(other);
        }

        public byte RecordId { get { return 0x01; } }

        public void SerializeToWriter(System.IO.BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(System.IO.BinaryReader reader)
        {
            _value = reader.ReadUInt16();
        }
    }
}
