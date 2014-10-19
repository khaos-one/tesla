using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashSha1
        : AbstractHash
    {
        public HashSha1()
            : base(20)
        { }

        public HashSha1(IEnumerable<byte> hash)
            : base(hash, 20)
        { }
    }
}
