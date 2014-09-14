using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public abstract class GOSTStribog
        : HashAlgorithm
    {
        protected GOSTStribog()
            : base()
        { }

        new public static GOSTStribog Create()
        {
            return new GOSTStribog512Managed();
        }

        new public static GOSTStribog Create(string name)
        {
            throw new NotSupportedException();
        }

        public static GOSTStribog Create(UInt16 length)
        {
            switch (length)
            {
                case 512:
                    return new GOSTStribog512Managed();
                    
                case 256:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentException("Only 256 and 512 bit ciphers are supported.");
            }
        }
    }
}
