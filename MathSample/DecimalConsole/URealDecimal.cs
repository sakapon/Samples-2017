using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
                return (0 <= i && i < Digits.Length) ? Digits[i] : 0;
            }
        }

        internal URealDecimal(byte[] digits, int? degree = null)
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

        URealDecimal(string value)
        {
            var match = Regex.Match(value, @"^([0-9]+)(\.([0-9]+))?$");
            if (!match.Success) throw new ArgumentException("Parse Error.");

            var intPart = match.Groups[1].Value.TrimStart('0');
            var decPart = match.Groups[3].Success ? match.Groups[3].Value.TrimEnd('0') : "";

            if (intPart == "")
            {
                if (decPart == "")
                {
                    _digits = null;
                    Degree = null;
                }
                else
                {
                    var i = 0;
                    for (; i < decPart.Length; i++)
                    {
                        if (decPart[i] != '0') break;
                    }

                    _digits = decPart.Substring(i)
                        .Select(c => DigitsMap[c])
                        .ToArray();
                    Degree = -i - 1;
                }
            }
            else
            {
                _digits = (intPart + decPart).TrimEnd('0')
                    .Select(c => DigitsMap[c])
                    .ToArray();
                Degree = intPart.Length - 1;
            }
        }

        public override string ToString()
        {
            // 0
            if (!Degree.HasValue)
                return "0";

            // x < 1
            if (Degree.Value < 0)
                return $"0.{new string('0', -Degree.Value - 1)}{Digits.ToSimpleString()}";

            // Integer
            if (Degree.Value >= Digits.Length - 1)
                return $"{Digits.ToSimpleString()}{new string('0', Degree.Value - Digits.Length + 1)}";

            var split = Digits.Split(Degree.Value + 1);
            return $"{split[0].ToSimpleString()}.{split[1].ToSimpleString()}";
        }

        public override bool Equals(object obj) =>
            obj is URealDecimal d && this == d;

        public override int GetHashCode()
        {
            if (!Degree.HasValue) return 0;

            var sum = 0;
            var p = 1;
            var count = Math.Min(Digits.Length, 9);
            for (var i = 0; i < count; i++)
            {
                sum += Digits[Digits.Length - 1 - i] * p;
                p *= 10;
            }
            return sum;
        }

        public static implicit operator URealDecimal(string value) => new URealDecimal(value);

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

        public static URealDecimal operator <<(URealDecimal d, int shift) =>
            d.Degree.HasValue ? new URealDecimal(d.Digits, d.Degree.Value + shift) : d;

        public static URealDecimal operator >>(URealDecimal d, int shift) =>
            d << -shift;

        public static bool operator ==(URealDecimal d1, URealDecimal d2) =>
            d1.Degree == d2.Degree && d1.Digits.ArrayEquals(d2.Digits);

        public static bool operator !=(URealDecimal d1, URealDecimal d2) =>
            !(d1 == d2);

        public static bool operator <(URealDecimal d1, URealDecimal d2)
        {
            if (!d2.Degree.HasValue) return false;
            if (!d1.Degree.HasValue) return true;
            if (d1.Degree.Value < d2.Degree.Value) return true;

            var digitsCount = Math.Min(d1.Digits.Length, d2.Digits.Length);
            for (var i = 0; i < digitsCount; i++)
            {
                if (d1.Digits[i] < d2.Digits[i]) return true;
            }
            return d1.Digits.Length < d2.Digits.Length;
        }

        public static bool operator >(URealDecimal d1, URealDecimal d2)
        {
            if (!d1.Degree.HasValue) return false;
            if (!d2.Degree.HasValue) return true;
            if (d1.Degree.Value > d2.Degree.Value) return true;

            var digitsCount = Math.Min(d1.Digits.Length, d2.Digits.Length);
            for (var i = 0; i < digitsCount; i++)
            {
                if (d1.Digits[i] > d2.Digits[i]) return true;
            }
            return d1.Digits.Length > d2.Digits.Length;
        }

        public static bool operator <=(URealDecimal d1, URealDecimal d2) =>
            !(d1 > d2);

        public static bool operator >=(URealDecimal d1, URealDecimal d2) =>
            !(d1 < d2);
    }
}
