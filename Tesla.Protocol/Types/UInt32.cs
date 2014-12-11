using System;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x02)]
    public class UInt32
        : IEquatable<System.UInt32>,
          IRecord
    {
        private System.UInt32 _value;

        public UInt32(System.UInt32 value)
        {
            _value = value;
        }

        public System.UInt32 Value { get { return _value; } }

        public static implicit operator UInt32(System.UInt32 v)
        {
            return new UInt32(v);
        }

        public static implicit operator System.UInt32(UInt32 v)
        {
            return v._value;
        }

        public bool Equals(uint other)
        {
            return _value.Equals(other);
        }

        public byte RecordId { get { return 0x02; } }

        public void SerializeToWriter(System.IO.BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(System.IO.BinaryReader reader)
        {
            _value = reader.ReadUInt32();
        }
    }
}
