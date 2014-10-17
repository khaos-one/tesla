using System;
using System.IO;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x00)]
    public struct UInt8
        : IEquatable<byte>,
          IBinarySerializable
    {
        private byte _value;

        public UInt8(byte value)
        {
            _value = value;
        }

        public byte Value { get { return _value; } }

        public static implicit operator UInt8(byte v)
        {
            return new UInt8(v);
        }

        public static implicit operator byte(UInt8 v)
        {
            return v._value;
        }
        public bool Equals(byte other)
        {
            return _value.Equals(other);
        }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            _value = reader.ReadByte();
        }
    }
}
