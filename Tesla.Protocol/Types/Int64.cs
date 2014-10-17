﻿using System;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x0B)]
    public struct Int64
        : IEquatable<System.Int64>,
          IBinarySerializable
    {
        private System.Int64 _value;

        public Int64(System.Int64 value)
        {
            _value = value;
        }

        public System.Int64 Value { get { return _value; } }

        public static implicit operator Int64(System.Int64 v)
        {
            return new Int64(v);
        }

        public static implicit operator System.Int64(Int64 v)
        {
            return v._value;
        }

        public bool Equals(long other)
        {
            return _value.Equals(other);
        }

        public void SerializeToWriter(System.IO.BinaryWriter writer)
        {
            writer.Write(_value);
        }

        public void DeserializeFromReader(System.IO.BinaryReader reader)
        {
            _value = reader.ReadInt64();
        }
    }
}
