using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x22)]
    public sealed class HashCrc32
        : AbstractHash
    {
        public HashCrc32()
            : base(4)
        { }

        public HashCrc32(IEnumerable<byte> hash)
            : base(hash, 4)
        { }

        public override byte RecordId
        {
            get { return 0x22; }
        }
    }
}
