using System;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Tesla.Protocol.Types
{
    public abstract class AbstractRecord
        : IRecord
    {
        public static byte GetRecordTypeId<T>()
            where T : AbstractRecord
        {
            var attr =
                typeof (T).GetCustomAttributes(false).First(x => x is ProtocolTypeAttribute) as ProtocolTypeAttribute;

            if (attr != null)
            {
                return attr.Id;
            }

            throw new InvalidOperationException("No attribute found.");
        }

        protected AbstractRecord(BinaryReader reader)
        {
            DeserializeFromReader(reader);
        }

        public abstract void SerializeToWriter(BinaryWriter writer);

        public abstract void DeserializeFromReader(BinaryReader reader);
    }
}
