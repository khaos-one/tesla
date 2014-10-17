using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    public sealed class HashCrc16
        : AbstractHash
    {
        public HashCrc16(IEnumerable<byte> hash)
            : base(hash, 2)
        { }
    }
}
