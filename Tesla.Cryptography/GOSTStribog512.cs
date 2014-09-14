using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tesla.Cryptography
{
    public abstract class GOSTStribog512
        : GOSTStribog
    {
        new public static GOSTStribog512 Create()
        {
            return new GOSTStribog512Managed();
        }

        new public static GOSTStribog512 Create(string name)
        {
            throw new NotSupportedException();
        }
    }
}
