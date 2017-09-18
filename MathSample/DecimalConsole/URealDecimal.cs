using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    struct URealDecimal
    {
        static readonly IDictionary<char, byte> DigitsMap = Enumerable.Range(0, 10).ToDictionary(i => i.ToString()[0], i => (byte)i);
        static readonly byte[] _digits_empty = new byte[0];

        byte[] _digits;
        byte[] Digits => _digits ?? _digits_empty;

        public int? Degree { get; }

        public double this[int index]
        {
            get
            {
                if (!Degree.HasValue) return 0;
                var i = Degree.Value - index;
                return (0 <= i || i < Digits.Length) ? Digits[i] : 0;
            }
        }

        URealDecimal(byte[] digits, int? degree = null)
        {
            if (digits == null || digits.Length == 0)
            {
                if (degree.HasValue) throw new ArgumentException("");
            }
            else
            {
                if (digits[0] == 0) throw new ArgumentException("");
                if (digits[digits.Length - 1] == 0) throw new ArgumentException("");
                if (!degree.HasValue) throw new ArgumentException("");
            }

            _digits = digits;
            Degree = degree;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static URealDecimal operator +(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static URealDecimal operator -(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static URealDecimal operator *(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static URealDecimal operator /(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static URealDecimal operator ^(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator <<(URealDecimal d, int shift)
        {
            throw new NotImplementedException();
        }

        public static bool operator >>(URealDecimal d, int shift)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator !=(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator >(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator <=(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static bool operator >=(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }
    }
}
