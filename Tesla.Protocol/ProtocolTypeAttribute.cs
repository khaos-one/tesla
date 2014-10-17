using System;
using System.IO;

namespace Tesla.Protocol
{
    public sealed class ProtocolTypeAttribute
        : Attribute
    {
        public byte Id { get; private set; }

        public ProtocolTypeAttribute(byte id)
        {
            Id = id;
        }
    }
}
