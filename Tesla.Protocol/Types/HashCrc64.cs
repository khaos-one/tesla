using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x23)]
    public sealed class HashCrc64
        : AbstractHash
    {
        public HashCrc64()
            : base(8)
        { }

        public HashCrc64(IEnumerable<byte> hash)
            : base(hash, 8)
        { }

        public override byte RecordId
        {
            get { return 0x23; }
        }
    }
}
