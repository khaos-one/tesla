using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tesla.Protocol.Types
{
    public class CompressedRcsDateTime
        : AbstractRecord
    {
        private uint _value;

        public CompressedRcsDateTime(BinaryReader reader)
            : base(reader)
        { }

        public CompressedRcsDateTime(uint value)
        {
            _value = value;
        }

        public CompressedRcsDateTime(DateTime dt)
        {
            
        }

        public override void SerializeToWriter(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void DeserializeFromReader(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
