using System.IO;

namespace Tesla.Protocol.Types
{
    public class DataArray<TI, TL>
        : AbstractRecord
        where TI : struct
        where TL : struct
    {
        public TI Id { get; set; }
        public TL Length { get; set; }
        public byte[] Data { get; set; }

        public DataArray()
        { }

        public DataArray(TI id, TL length)
        {
            Id = id;
            Length = length;
        }

        public DataArray(TI id, TL length, byte[] data)
            : this(id, length)
        {
            Data = data;
        }

        // Note: В итоге в качестве решения эти методы были написаны явно для
        // каждого подкласса. В будущем решение можно универсализировать.
        //public void SerializeToWriter(BinaryWriter writer)
        //{
        //    var id = ObjectMarshal.BytesFromStructure(Id);
        //    var length = ObjectMarshal.BytesFromStructure(Length);

        //    writer.Write(id);
        //    writer.Write(length);
        //    writer.Write(Data);
        //}

        //public void DeserializeFromReader(BinaryReader reader)
        //{
        //    var len1 = Marshal.SizeOf(Id);
        //    var len2 = Marshal.SizeOf(Length);

        //    var buf1 = reader.ReadBytes(len1);
        //    Id = ObjectMarshal.StructureFromBytes<TI>(buf1);
        //    var buf2 = reader.ReadBytes(len2);
        //    Length = ObjectMarshal.StructureFromBytes<TL>(buf2);

        //    Data = reader.ReadBytes(/* FAIL */);
        //}
    }
    
    [ProtocolType(0x10)]
    public class DataArrayI1L1
        : DataArray<byte, byte>,
          IRecord
    {
        public byte RecordId { get { return 0x10; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadByte();
            Length = reader.ReadByte();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x11)]
    public class DataArrayI1L2
        : DataArray<byte, System.UInt16>,
          IRecord
    {
        public byte RecordId { get { return 0x11; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadByte();
            Length = reader.ReadUInt16();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x12)]
    public class DataArrayI1L4
        : DataArray<byte, System.UInt32>,
          IRecord
    {
        public byte RecordId { get { return 0x12; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadByte();
            Length = reader.ReadUInt32();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x13)]
    public class DataArrayI1L8
        : DataArray<byte, System.UInt64>,
            IRecord
    {
        public byte RecordId { get { return 0x13; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadByte();
            Length = reader.ReadUInt64();
            Data = reader.ReadBytes((int)Length);
        }
    }

    [ProtocolType(0x14)]
    public class DataArrayI2L1
        : DataArray<System.UInt16, byte>,
          IRecord
    {
        public byte RecordId { get { return 0x14; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt16();
            Length = reader.ReadByte();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x15)]
    public class DataArrayI2L2
        : DataArray<System.UInt16, System.UInt16>,
          IRecord
    {
        public byte RecordId { get { return 0x15; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt16();
            Length = reader.ReadUInt16();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x16)]
    public class DataArrayI2L4
        : DataArray<System.UInt16, System.UInt32>,
          IRecord
    {
        public byte RecordId { get { return 0x16; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt16();
            Length = reader.ReadUInt32();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x17)]
    public class DataArrayI2L8
        : DataArray<System.UInt16, System.UInt64>,
          IRecord
    {
        public byte RecordId { get { return 0x17; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt16();
            Length = reader.ReadUInt64();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x18)]
    public class DataArrayI4L1
        : DataArray<System.UInt32, byte>,
          IRecord
    {
        public byte RecordId { get { return 0x18; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Length = reader.ReadByte();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x19)]
    public class DataArrayI4L2
        : DataArray<System.UInt32, System.UInt16>,
          IRecord
    {
        public byte RecordId { get { return 0x19; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Length = reader.ReadUInt16();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x1A)]
    public class DataArrayI4L4
        : DataArray<System.UInt32, System.UInt32>,
          IRecord
    {
        public byte RecordId { get { return 0x1A; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Length = reader.ReadUInt32();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x1B)]
    public class DataArrayI4L8
        : DataArray<System.UInt32, System.UInt64>,
            IRecord
    {
        public byte RecordId { get { return 0x1B; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt32();
            Length = reader.ReadUInt64();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x1C)]
    public class DataArrayI8L1
        : DataArray<System.UInt64, byte>,
          IRecord
    {
        public byte RecordId { get { return 0x1C; }}

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt64();
            Length = reader.ReadByte();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x1D)]
    public class DataArrayI8L2
        : DataArray<System.UInt64, System.UInt16>,
            IRecord
    {
        public byte RecordId { get { return 0x1D; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt64();
            Length = reader.ReadUInt16();
            Data = reader.ReadBytes(Length);
        }
    }

    [ProtocolType(0x1E)]
    public class DataArrayI8L4
        : DataArray<System.UInt64, System.UInt32>,
            IRecord
    {
        public byte RecordId { get { return 0x1E; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt64();
            Length = reader.ReadUInt32();
            Data = reader.ReadBytes((int) Length);
        }
    }

    [ProtocolType(0x1F)]
    public class DataArrayI8L8
        : DataArray<System.UInt64, System.UInt64>,
            IRecord
    {
        public byte RecordId { get { return 0x1F; } }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Length);
            writer.Write(Data);
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            Id = reader.ReadUInt64();
            Length = reader.ReadUInt64();
            Data = reader.ReadBytes((int)Length);
        }
    }
}
