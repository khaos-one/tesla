using System.IO;

namespace Tesla.Protocol
{
    public interface IRecord
    {
        byte RecordId { get; }
        void SerializeToWriter(BinaryWriter writer);
        void DeserializeFromReader(BinaryReader reader);
    }
}
