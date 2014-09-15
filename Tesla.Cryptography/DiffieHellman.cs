using System;
using System.Dynamic;
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
        protected BigInt PrivateKey;
        protected BigInt PublicKey;

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

            PrivateKey = BigInt.GenPseudoPrime(NumberOfBits, 30, StrongRng);
        }

        ~DiffieHellman()
        {
            Dispose();
        }

        protected void Write(BigInt value)
        {
            var buffer = value.GetBytes();
            var lengthBuffer = BitConverter.GetBytes(buffer.Length);

            InnerStream.Write(lengthBuffer, 0, lengthBuffer.Length);
            InnerStream.Write(buffer, 0, buffer.Length);
        }

        protected BigInt Read()
        {
            var lengthBuffer = new byte[sizeof (Int32)];
            InnerStream.Read(lengthBuffer, 0, lengthBuffer.Length);
            var length = BitConverter.ToUInt32(lengthBuffer, 0);
            var buffer = new byte[length];
            InnerStream.Read(buffer, 0, buffer.Length);

            return new BigInt(buffer);
        }

        protected void SendRequest()
        {
            PublicPrime = BigInt.GenPseudoPrime(NumberOfBits, 30, StrongRng);
            PublicBase = 5;
            PublicKey = PublicBase.ModPow(PrivateKey, PublicPrime);

            Write(PublicPrime);
            Write(PublicBase);
            Write(PublicKey);
        }

        protected void HandleRequest()
        {
            PublicPrime = Read();
            PublicBase = Read();
            PublicKey = PublicBase.ModPow(PrivateKey, PublicPrime);

            using (var otherPublicKey = Read())
            {
                Write(PublicKey);

                using (var key = otherPublicKey.ModPow(PrivateKey, PublicPrime))
                {
                    Key = HashAlgorithm.ComputeHash(key.GetBytes());
                }
            }
        }

        protected void HandleResponse()
        {
            using (var otherPublicKey = Read())
            {
                using (var key = otherPublicKey.ModPow(PrivateKey, PublicPrime))
                {
                    Key = HashAlgorithm.ComputeHash(key.GetBytes());
                }
            }
        }

        public void NegotiateAsClient()
        {
            SendRequest();
            HandleResponse();
        }

        public void NegotiateAsServer()
        {
            HandleRequest();
        }

        public void Dispose()
        {
            if (!ReferenceEquals(PublicPrime, null))
            {
                PublicPrime.Dispose();
                PublicPrime = null;
            }

            if (!ReferenceEquals(PublicBase, null))
            {
                PublicBase.Dispose();
                PublicBase = null;
            }

            if (!ReferenceEquals(PrivateKey, null))
            {
                PrivateKey.Dispose();
                PrivateKey = null;
            }

            if (!ReferenceEquals(HashAlgorithm, null))
            {
                HashAlgorithm.Dispose();
                HashAlgorithm = null;
            }

            GC.Collect();
            GC.Collect();
        }
    }
}
