using System.Collections.Generic;

namespace Tesla.Protocol.Types
{
    [ProtocolType(0x21)]
    public sealed class HashCrc16
        : AbstractHash
    {
        public HashCrc16()
            : base(2)
        { }

        public HashCrc16(IEnumerable<byte> hash)
            : base(hash, 2)
        { }

        public override byte RecordId
        {
            get { return 0x21; }
        }
    }
}
