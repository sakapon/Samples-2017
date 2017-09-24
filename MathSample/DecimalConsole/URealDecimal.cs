using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DecimalConsole
{
    public struct URealDecimal
    {
        static readonly IDictionary<int, int> Position10Map = Enumerable.Range(0, 9).ToDictionary(i => i, i => (int)Math.Pow(10, i));
        static readonly IDictionary<char, byte> DigitsMap = Enumerable.Range(0, 10).ToDictionary(i => i.ToString()[0], i => (byte)i);
        static readonly byte[] _digits_empty = new byte[0];

        public static URealDecimal Zero { get; } = default(URealDecimal);

        byte[] _digits;
        public byte[] Digits => _digits ?? _digits_empty;

        public int? Degree { get; }
        public bool IsZero => !Degree.HasValue;

        public byte this[int index]
        {
            get
            {
                if (!Degree.HasValue) return 0;
                var i = Degree.Value - index;
                return (0 <= i && i < Digits.Length) ? Digits[i] : (byte)0;
            }
        }

        internal URealDecimal(byte[] digits, int? degree)
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

        public override int GetHashCode() => Digits
            .Reverse()
            .Take(9)
            .Select((v, i) => v * Position10Map[i])
            .Sum();

        static URealDecimal FromString(string value)
        {
            var match = Regex.Match(value, @"^([0-9]+)(\.([0-9]+))?$");
            if (!match.Success) throw new ArgumentException("Parse Error.");

            var intPart = match.Groups[1].Value.TrimStart('0');
            var decPart = match.Groups[3].Success ? match.Groups[3].Value.TrimEnd('0') : "";

            if (intPart == "")
            {
                if (decPart == "")
                {
                    return Zero;
                }
                else
                {
                    var i = 0;
                    for (; i < decPart.Length; i++)
                    {
                        if (decPart[i] != '0') break;
                    }

                    var digits = decPart.Substring(i)
                        .Select(c => DigitsMap[c])
                        .ToArray();
                    return new URealDecimal(digits, -i - 1);
                }
            }
            else
            {
                var digits = (intPart + decPart).TrimEnd('0')
                    .Select(c => DigitsMap[c])
                    .ToArray();
                return new URealDecimal(digits, intPart.Length - 1);
            }
        }

        static URealDecimal FromInt32(int value) => value.ToString();
        static URealDecimal FromDouble(double value) => value.ToString();

        public static implicit operator URealDecimal(string value) => FromString(value);
        public static implicit operator URealDecimal(int value) => FromInt32(value);
        public static implicit operator URealDecimal(double value) => FromDouble(value);

        public static URealDecimal operator +(URealDecimal d1, URealDecimal d2)
        {
            var digits = new Dictionary<int, byte>();
            foreach (var _ in d1.GetDigitsAsPairs())
                Add(digits, _.i, _.v);
            foreach (var _ in d2.GetDigitsAsPairs())
                Add(digits, _.i, _.v);
            return ToURealDecimal(digits);
        }

        // Assert d1 >= d2.
        public static URealDecimal operator -(URealDecimal d1, URealDecimal d2)
        {
            var digits = new Dictionary<int, byte>();
            foreach (var _ in d1.GetDigitsAsPairs())
                Add(digits, _.i, _.v);
            foreach (var _ in d2.GetDigitsAsPairs())
                Subtract_1(digits, _.i, _.v);
            return ToURealDecimal(digits);
        }

        public static URealDecimal operator *(URealDecimal d1, URealDecimal d2)
        {
            var digits = new Dictionary<int, byte>();

            var q = from p1 in d1.GetDigitsAsPairs()
                    from p2 in d2.GetDigitsAsPairs()
                    select (i: p1.i + p2.i, v: p1.v * p2.v);

            foreach (var _ in q)
                Add(digits, _.i, _.v);
            return ToURealDecimal(digits);
        }

        public static URealDecimal operator /(URealDecimal d1, URealDecimal d2)
        {
            throw new NotImplementedException();
        }

        // Power
        public static URealDecimal operator ^(URealDecimal d, int power)
        {
            if (power < 0) throw new NotImplementedException();

            URealDecimal result = "1";
            for (var i = 0; i < power; i++)
                result *= d;
            return result;
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

        IEnumerable<(int i, byte v)> GetDigitsAsPairs()
        {
            var d = this;
            return Digits
                .Select((v, i) => (i: d.Degree.Value - i, v: v))
                .Where(_ => _.v != 0);
        }

        static void Add(Dictionary<int, byte> digits, int index, int value)
        {
            var v = digits.ContainsKey(index) ? digits[index] : 0;
            v += value;

            if (v < 10)
            {
                digits[index] = (byte)v;
            }
            else
            {
                Add(digits, index + 1, v / 10);
                digits[index] = (byte)(v % 10);
            }
        }

        // digits >= value
        // value: 0...9
        static void Subtract_1(Dictionary<int, byte> digits, int index, int value)
        {
            var v = digits.ContainsKey(index) ? digits[index] : 0;
            v -= value;

            if (v >= 0)
            {
                digits[index] = (byte)v;
            }
            else
            {
                Subtract_1(digits, index + 1, 1);
                digits[index] = (byte)(v + 10);
            }
        }

        static URealDecimal ToURealDecimal(Dictionary<int, byte> digits_dic)
        {
            var indexes = digits_dic
                .Where(p => p.Value != 0)
                .Select(p => p.Key)
                .ToArray();
            if (indexes.Length == 0) return Zero;

            var maxIndex = indexes.Max();
            var minIndex = indexes.Min();

            var digits = Enumerable.Range(0, maxIndex - minIndex + 1)
                .Select(j => maxIndex - j)
                .Select(i => digits_dic.ContainsKey(i) ? digits_dic[i] : (byte)0)
                .ToArray();
            return new URealDecimal(digits, maxIndex);
        }
    }
}
