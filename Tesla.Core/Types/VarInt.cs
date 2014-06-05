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

        public VarInt(BigInteger v)
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

        public IEnumerable<byte> ToByteArray()
        {
            //var neededBytes = (long) (BigInteger.Log(_value, 2) + 1) >> 3;
            var value = _value;

            unchecked
            {
                do
                {
                    var tmp = value & 0x7f;
                    value >>= 7;

                    if (value != 0)
                    {
                        tmp |= 0x80;
                    }

                    yield return (byte) tmp;
                } while (value != 0);
            }
        }

        // TODO: Implement ZigZag encoding/decoding.

        public int CompareTo(object obj)
        {
            return _value.CompareTo(((VarInt) obj)._value);
        }

        public int CompareTo(VarInt other)
        {
            return _value.CompareTo(other._value);
        }

        public bool Equals(VarInt other)
        {
            return _value.Equals(other._value);
        }

        public static VarInt operator +(VarInt a, VarInt b)
        {
            return new VarInt(a._value + b._value);
        }

        public static VarInt operator -(VarInt a, VarInt b)
        {
            return new VarInt(a._value - b._value);
        }
    }
}
