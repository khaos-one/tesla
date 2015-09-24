using System.IO;

namespace Tesla.Protocol {
    public interface IRecord {
        void SerializeToWriter(BinaryWriter writer);
        void DeserializeFromReader(BinaryReader reader);
    }
}