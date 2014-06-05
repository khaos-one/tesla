using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesla.Types
{
    public struct VarInt
        : IComparable,
          IComparable<VarInt>,
          IEquatable<VarInt>
    {
        private BigInteger _value;


        #region Constructors

        public VarInt(IEnumerable<byte> v)
        {
            _value = FromBytes(v);
        }

        public VarInt(Byte v)
        {
            _value = v;
        }

        public VarInt(Int16 v)
        {
            _value = v;
        }

        public VarInt(Int32 v)
        {
            _value = v;
        }

        public VarInt(Int64 v)
        {
            _value = v;
        }

        public VarInt(SByte v)
        {
            _value = v;
        }

        public VarInt(UInt16 v)
        {
            _value = v;
        }

        public VarInt(UInt32 v)
        {
            _value = v;
        }

        public VarInt(UInt64 v)
        {
            _value = v;
        }

        #endregion


        private static BigInteger FromBytes(IEnumerable<byte> v)
        {
            BigInteger result = 0;
            var shift = 0;

            unchecked
            {
                foreach (var b in v)
                {
                    var tmp = (byte) (b & 0x7f);
                    result |= tmp << shift;

                    if ((b & 0x80) == 0)
                    {
                        break;
                    }

                    shift += 7;
                }
            }

            return result;
        }

        public byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(VarInt other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(VarInt other)
        {
            throw new NotImplementedException();
        }
    }
}
