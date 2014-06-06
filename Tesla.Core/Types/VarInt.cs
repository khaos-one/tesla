using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tesla.Types
{
    public struct VarInt
        : IFormattable,
          IComparable,
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
            var endReached = false;

            unchecked
            {
                foreach (var b in v)
                {
                    var tmp = (byte) (b & 0x7f);
                    result |= (BigInteger) tmp << shift;

                    if ((b & 0x80) == 0)
                    {
                        endReached = true;
                        break;
                    }

                    shift += 7;
                }
            }

            if (!endReached)
            {
                throw new ArgumentException("VarInt byte array was truncated.");
            }

            return result;
        }

        public long NeededBytes
        {
            get { return (long) (BigInteger.Log(_value, 2) + 1) >> 3; }
        }

        public IEnumerable<byte> ToBytes()
        {
            //var neededBytes = (long) (BigInteger.Log(_value, 2) + 1) >> 3;
            var value = _value;

            unchecked
            {
                do
                {
                    var tmp = (byte) (value & 0x7f);
                    value >>= 7;

                    if (value != 0)
                    {
                        tmp |= 0x80;
                    }

                    yield return tmp;
                } while (value != 0);
            }
        }

        // TODO: Implement ZigZag encoding/decoding.

        #region Implicit Conversion Operators

        public static implicit operator VarInt(Byte[] v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(BigInteger v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(Byte v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(Int16 v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(Int32 v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(Int64 v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(SByte v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(UInt16 v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(UInt32 v)
        {
            return new VarInt(v);
        }

        public static implicit operator VarInt(UInt64 v)
        {
            return new VarInt(v);
        }

        #endregion

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

        #region Math operators

        public static VarInt operator +(VarInt a, VarInt b)
        {
            return new VarInt(a._value + b._value);
        }

        public static VarInt operator -(VarInt a, VarInt b)
        {
            return new VarInt(a._value - b._value);
        }

        public static VarInt operator *(VarInt a, VarInt b)
        {
            return new VarInt(a._value*b._value);
        }

        public static VarInt operator /(VarInt a, VarInt b)
        {
            return new VarInt(a._value/b._value);
        }

        public static VarInt operator &(VarInt a, VarInt b)
        {
            return new VarInt(a._value & b._value);
        }

        public static VarInt operator |(VarInt a, VarInt b)
        {
            return new VarInt(a._value | b._value);
        }

        public static VarInt operator ^(VarInt a, VarInt b)
        {
            return new VarInt(a._value ^ b._value);
        }

        public static VarInt operator <<(VarInt a, int b)
        {
            return new VarInt(a._value << b);
        }

        public static VarInt operator >>(VarInt a, int b)
        {
            return new VarInt(a._value >> b);
        }

        #endregion

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return _value.ToString(format, formatProvider);
        }
    }
}
