using System;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x08)]
    public struct Int8
        : IEquatable<sbyte>,
          IBinarySerializable
    {
        private sbyte _value;

        public Int8(sbyte value)
        {
            _value = value;
        }

        public sbyte Value { get { return _value; } }

        public static implicit operator Int8(sbyte v)
        {
            return new Int8(v);
        }

        public static implicit operator sbyte(Int8 v)
        {
            return v._value;
        }
        public bool Equals(sbyte other)
        {
            return _value.Equals(other);
        }

        public void SerializeToWriter(System.IO.BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(System.IO.BinaryReader reader)
        {
            _value = reader.ReadSByte();
        }
    }
}
