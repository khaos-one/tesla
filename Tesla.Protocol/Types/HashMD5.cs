using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x26)]
    public sealed class HashMD5
        : AbstractHash
    {
        public HashMD5()
            : base(16)
        { }

        public HashMD5(IEnumerable<byte> hash)
            : base(hash, 16)
        { }

        public override byte RecordId
        {
            get { return 0x26; }
        }
    }
}
