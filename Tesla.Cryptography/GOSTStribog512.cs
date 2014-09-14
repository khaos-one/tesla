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
            throw new NotImplementedException();
        }

        new public static GOSTStribog512 Create(string name)
        {
            throw new NotImplementedException();
        }
    }
}
