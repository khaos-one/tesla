using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Tesla.Protocol.Types
{
    public abstract class AbstractHash
        : ReadOnlyCollection<byte>
    {
        protected AbstractHash(IEnumerable<byte> list, int length)
            : base(list.Take(length).ToList())
        { }
    }
}
