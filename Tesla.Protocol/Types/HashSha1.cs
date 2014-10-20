using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x27)]
    public sealed class HashSha1
        : AbstractHash
    {
        public HashSha1()
            : base(20)
        { }

        public HashSha1(IEnumerable<byte> hash)
            : base(hash, 20)
        { }

        public override byte RecordId
        {
            get { return 0x27; }
        }
    }
}
