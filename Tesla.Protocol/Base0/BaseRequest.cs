using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesla.Protocol.Base0
{
    public class BaseRequest
    {
        public ushort ProtocolVersion { get; set; }
        public string Function { get; set; }
    }
}
