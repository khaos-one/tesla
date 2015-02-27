using System.IO;

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
