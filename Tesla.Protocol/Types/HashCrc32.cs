using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashCrc32
        : AbstractHash
    {
        public HashCrc32(IEnumerable<byte> hash)
            : base(hash, 4)
        { }
    }
}
