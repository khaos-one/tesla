using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x29)]
    public sealed class HashSha512
        : AbstractHash
    {
        public HashSha512()
            : base(64)
        { }

        public HashSha512(IEnumerable<byte> hash)
            : base(hash, 64)
        { }

        public override byte RecordId
        {
            get { return 0x29; }
        }
    }
}
