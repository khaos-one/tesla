using System;
using System.IO;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x33)]
    public struct UnixTimestamp
        : IEquatable<DateTime>,
          IEquatable<ulong>,
          IRecord
    {
        private ulong _value;

        public UnixTimestamp(ulong value)
        {
            _value = value;
        }

        public UnixTimestamp(DateTime dt)
        {
            _value = DateTimeToUnixTimestamp(dt);
        }

        public ulong Value { get { return _value; } }

        public static implicit operator DateTime(UnixTimestamp ut)
        {
            return UnixTimestampToDateTime(ut._value);
        }

        public static implicit operator UnixTimestamp(DateTime dt)
        {
            return new UnixTimestamp(dt);
        }

        public static implicit operator ulong(UnixTimestamp ut)
        {
            return ut._value;
        }

        public static implicit operator UnixTimestamp(ulong v)
        {
            return new UnixTimestamp(v);
        }

        private static DateTime UnixTimestampToDateTime(ulong timestamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timestamp).ToLocalTime();
            return dtDateTime;
        }

        private static ulong DateTimeToUnixTimestamp(DateTime dt)
        {
            return (ulong) (dt - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        public bool Equals(DateTime other)
        {
            return UnixTimestampToDateTime(_value).Equals(other);
        }

        public bool Equals(ulong other)
        {
            return _value.Equals(other);
        }

        public byte RecordId { get { return 0x33; } }
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
