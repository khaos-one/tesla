using System;
using System.Collections.Generic;
using System.Linq;
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

            return DecodeZigZag(result);
        }

        public long NeededBits
        {
            get
            {
                var pure = Math.Ceiling((decimal) BigInteger.Log(BigInteger.Abs(_value), 2) + 1);
                return (long) (pure + Math.Ceiling(pure/7) - 1);
            }
        }

        public long NeededBytes
        {
            get { return (long) (Math.Ceiling((decimal) NeededBits/8)); }
        }

        public IEnumerable<byte> ToBytes()
        {
            //var value = _value < 0 ? EncodeZigZag(_value, (int) NeededBits) : _value;
            var value = EncodeZigZag(_value, (int) NeededBits);

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

        public byte[] ToByteArray()
        {
            return ToBytes().ToArray();
        }

        private static BigInteger EncodeZigZag(BigInteger value, int bitLength)
        {
            return (value << 1) ^ (value >> (bitLength - 1));
        }

        private static BigInteger DecodeZigZag(BigInteger value)
        {
            if ((value & 0x1) == 0x1)
            {
                return (-1*((value >> 1) + 1));
            }

            return value >> 1;
        }

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
