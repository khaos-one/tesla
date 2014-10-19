namespace Tesla.Protocol.Types
{
    public sealed class HashCrc8
        : AbstractHash
    {
        public HashCrc8()
            : base(1)
        { }

        public HashCrc8(byte hash)
            : base(new[] {hash}, 1)
        { }
    }
}
