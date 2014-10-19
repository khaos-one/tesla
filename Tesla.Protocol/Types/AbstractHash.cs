using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Tesla.Protocol.Types
{
    public abstract class AbstractHash
        : WriteProtectedList<byte>,
          IBinarySerializable
    {
        protected AbstractHash(int length)
            : base(length)
        { }

        protected AbstractHash(IEnumerable<byte> list, int length)
            : base(list.Take(length).ToList())
        { }

        public void SerializeToWriter(BinaryWriter writer)
        {
            writer.Write(ToArray());
        }

        public void DeserializeFromReader(BinaryReader reader)
        {
            var bytes = reader.ReadBytes(Capacity);

            for (var i = 0; i < Capacity; i++)
            {
                Insert(i, bytes[i]);
            }
        }
    }
}
