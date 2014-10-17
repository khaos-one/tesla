using System.IO;

namespace Tesla.Protocol
{
    public interface IBinarySerializable
    {
        void SerializeToWriter(BinaryWriter writer);
        void DeserializeFromReader(BinaryReader reader);
    }
}
