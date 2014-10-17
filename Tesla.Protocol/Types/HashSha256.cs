using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashSha256
        : AbstractHash
    {
        public HashSha256(IEnumerable<byte> hash)
            : base(hash, 32)
        { }
    }
}
