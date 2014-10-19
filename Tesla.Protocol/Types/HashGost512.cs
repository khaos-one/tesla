using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashGost512
        : AbstractHash
    {
        public HashGost512()
            : base(64)
        { }

        public HashGost512(IEnumerable<byte> hash)
            : base(hash, 64)
        { }
    }
}
