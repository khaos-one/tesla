using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x30)]
    public class RcsDateTime
        : IEquatable<DateTime>,
          IRecord
    {
        private uint _year;
        private uint _month;
        private uint _day;
        private uint _hour;
        private uint _minute;
        private uint _second;

        public RcsDateTime(DateTime dt)
        {
            _year = (uint) dt.Year;
            _month = (uint) dt.Month;
            _day = (uint) dt.Day;
            _hour = (uint) dt.Hour;
            _minute = (uint) dt.Minute;
            _second = (uint) dt.Second;
        }

        public RcsDateTime(byte[] data)
        {
            _year = BitConverter.ToUInt32(data, 0);
            _month = BitConverter.ToUInt32(data, 4);
            _day = BitConverter.ToUInt32(data, 8);
            _hour = BitConverter.ToUInt32(data, 12);
            _minute = BitConverter.ToUInt32(data, 16);
            _second = BitConverter.ToUInt32(data, 20);
        }

        public uint Year { get { return _year; } }
        public uint Month { get { return _month; } }
        public uint Day { get { return _day; } }
        public uint Hour { get { return _hour; } }
        public uint Minute { get { return _minute; } }
        public uint Second { get { return _second; } }

        private void FromByteArray(byte[] data)
        {
            _year = BitConverter.ToUInt32(data, 0);
            _month = BitConverter.ToUInt32(data, 4);
            _day = BitConverter.ToUInt32(data, 8);
            _hour = BitConverter.ToUInt32(data, 12);
            _minute = BitConverter.ToUInt32(data, 16);
            _second = BitConverter.ToUInt32(data, 20);
        }

        public static implicit operator DateTime(RcsDateTime rt)
        {
            return new DateTime((int) rt._year, (int) rt._month, (int) rt._day, (int) rt._hour, (int) rt._minute,
                (int) rt._second);
        }

        public static implicit operator RcsDateTime(DateTime dt)
        {
            return new RcsDateTime(dt);
        }

        public static implicit operator byte[](RcsDateTime rt)
        {
            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                {
                    w.Write(rt._year);
                    w.Write(rt._month);
                    w.Write(rt._day);
                    w.Write(rt._hour);
                    w.Write(rt._hour);
                    w.Write(rt._minute);
                    w.Write(rt._second);
                }

                return ms.ToArray();
            }
        }

        public static implicit operator RcsDateTime(byte[] data)
        {
            return new RcsDateTime(data);
        }

        public bool Equals(DateTime other)
        {
            return (_year == other.Year) &&
                   (_month == other.Month) &&
                   (_day == other.Day) &&
                   (_hour == other.Hour) &&
                   (_minute == other.Minute) &&
                   (_second == other.Second);
        }

        public byte RecordId { get { return 0x30; } }
        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(this);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            FromByteArray(reader.ReadBytes(24));
        }
    }
}
