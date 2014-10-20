using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x24)]
    public sealed class HashGost256
        : AbstractHash
    {
        public HashGost256()
            : base(32)
        { }

        public HashGost256(IEnumerable<byte> hash)
            : base(hash, 32)
        { }

        public override byte RecordId
        {
            get { return 0x24; }
        }
    }
}
