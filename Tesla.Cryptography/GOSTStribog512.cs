using System;

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
