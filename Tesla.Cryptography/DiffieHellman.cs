using System;
using System.IO;
using System.Security.Cryptography;

namespace Tesla.Cryptography
{
    public class DiffieHellman
        : IDisposable
    {
        protected static readonly StrongNumberProvider StrongRng = new StrongNumberProvider();

        protected Stream InnerStream;
        protected int NumberOfBits;
        protected bool CloseStream;

        protected BigInt PublicPrime;
        protected BigInt PublicBase;
        protected BigInt PrivatePrime;

        protected HashAlgorithm HashAlgorithm;

        public byte[] Key { get; protected set; }

        public DiffieHellman(Stream stream, int numberOfBits = 256, HashAlgorithm hashAlgorithm = null, bool closeStream = false)
        {
            if (!(stream.CanRead && stream.CanWrite))
            {
                throw new ArgumentException("Provided stream does not support Read and/or Write operations.");
            }

            InnerStream = stream;

            if ((numberOfBits & (numberOfBits - 1)) != 0)
            {
                throw new ArgumentException("numberOfBits is not a power of 2.");
            }

            NumberOfBits = numberOfBits;
            CloseStream = closeStream;

            if (ReferenceEquals(hashAlgorithm, null))
            {
                if (numberOfBits <= 128)
                {
                    HashAlgorithm = HashAlgorithm.Create("SHA1");
                }
                else if (numberOfBits <= 256)
                {
                    HashAlgorithm = GOSTStribog.Create("GOST256");
                }
                else if (numberOfBits <= 512)
                {
                    HashAlgorithm = GOSTStribog.Create("GOST512");
                }
                else
                {
                    throw new ArgumentException("numberOfBits length exceeds hashing possibilities.");
                }
            }
            else
            {
                if (numberOfBits > hashAlgorithm.HashSize)
                {
                    throw new ArgumentException("numberOfBits exceeds specified hash length.");
                }

                HashAlgorithm = hashAlgorithm;
            }

            PublicPrime = BigInt.GenPseudoPrime(numberOfBits, 30, StrongRng);
            PrivatePrime = BigInt.GenPseudoPrime(numberOfBits, 30, StrongRng);
            PublicBase = 5;
        }



        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
