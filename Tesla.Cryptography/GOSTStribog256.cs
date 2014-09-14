using System;

namespace Tesla.Cryptography
{
    public abstract class GOSTStribog256
        : GOSTStribog
    {
        new public static GOSTStribog256 Create()
        {
            return new GOSTStribog256Managed();
        }

        new public static GOSTStribog256 Create(string name)
        {
            throw new NotSupportedException();
        }
    }
}
