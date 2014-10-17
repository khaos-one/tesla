using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashMD5
        : AbstractHash
    {
        public HashMD5(IEnumerable<byte> hash)
            : base(hash, 16)
        { }
    }
}
