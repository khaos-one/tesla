using System;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public abstract class GOSTStribog
        : HashAlgorithm
    {
        protected GOSTStribog()
            : base()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Initialize();
        }

        new public static GOSTStribog Create()
        {
            return new GOSTStribog512Managed();
        }

        new public static GOSTStribog Create(string name)
        {
            switch (name)
            {
                case "GOST512":
                case "GOST-512":
                    return new GOSTStribog512Managed();

                case "GOST256":
                case "GOST-256":
                    return new GOSTStribog256Managed();
                    
                default:
                    throw new ArgumentException("No supported cipher found.");
            }
        }

        public static GOSTStribog Create(UInt16 length)
        {
            switch (length)
            {
                case 512:
                    return new GOSTStribog512Managed();
                    
                case 256:
                    return new GOSTStribog256Managed();

                default:
                    throw new ArgumentException("Only 256 and 512 bit ciphers are supported.");
            }
        }
    }
}
