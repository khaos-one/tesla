using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tesla.Protocol.Types
{
    public abstract class RecordList
        : AbstractRecord
    {
        protected RecordList(BinaryReader reader) 
            : base(reader)
        { }
    }
}
