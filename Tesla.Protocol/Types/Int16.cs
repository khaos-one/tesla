﻿using System;
using System.IO;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x09)]
    public struct Int16
        : IEquatable<System.Int16>,
          IBinarySerializable
    {
        private System.Int16 _value;

        public Int16(System.Int16 value)
        {
            _value = value;
        }

        public System.Int16 Value { get { return _value; } }

        public static implicit operator Int16(System.Int16 v)
        {
            return new Int16(v);
        }

        public static implicit operator System.Int16(Int16 v)
        {
            return v._value;
        }

        public bool Equals(short other)
        {
            return _value.Equals(other);
        }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            _value = reader.ReadInt16();
        }
    }
}
