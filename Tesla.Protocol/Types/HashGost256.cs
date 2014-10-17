using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashGost256
        : AbstractHash
    {
        public HashGost256(IEnumerable<byte> hash)
            : base(hash, 32)
        { }
    }
}
