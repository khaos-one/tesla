using System;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x0A)]
    public struct Int32
        : IEquatable<System.Int32>,
          IRecord
    {
        private System.Int32 _value;

        public Int32(System.Int32 value)
        {
            _value = value;
        }

        public System.Int32 Value { get { return _value; } }

        public static implicit operator Int32(System.Int32 v)
        {
            return new Int32(v);
        }

        public static implicit operator System.Int32(Int32 v)
        {
            return v._value;
        }

        public bool Equals(int other)
        {
            return _value.Equals(other);
        }

        public byte RecordId { get { return 0x0A; } }

        public void SerializeToWriter(System.IO.BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(System.IO.BinaryReader reader)
        {
            _value = reader.ReadInt32();
        }
    }
}
