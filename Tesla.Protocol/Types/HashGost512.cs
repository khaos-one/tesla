using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x25)]
    public sealed class HashGost512
        : AbstractHash
    {
        public HashGost512()
            : base(64)
        { }

        public HashGost512(IEnumerable<byte> hash)
            : base(hash, 64)
        { }

        public override byte RecordId
        {
            get { return 0x25; }
        }
    }
}
