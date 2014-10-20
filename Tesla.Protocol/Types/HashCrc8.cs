namespace Tesla.Protocol.Types
{
    [ProtocolType(0x20)]
    public sealed class HashCrc8
        : AbstractHash
    {
        public HashCrc8()
            : base(1)
        { }

        public HashCrc8(byte hash)
            : base(new[] {hash}, 1)
        { }

        public override byte RecordId
        {
            get { return 0x20; }
        }
    }
}
