using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x28)]
    public sealed class HashSha256
        : AbstractHash
    {
        public HashSha256()
            : base(32)
        { }

        public HashSha256(IEnumerable<byte> hash)
            : base(hash, 32)
        { }

        public override byte RecordId
        {
            get { return 0x28; }
        }
    }
}
