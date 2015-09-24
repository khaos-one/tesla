using System;

namespace Tesla.Protocol {
    public sealed class ProtocolTypeAttribute
        : Attribute {
        public ProtocolTypeAttribute(byte id) {
            Id = id;
        }

        public byte Id { get; private set; }
    }
}