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

        public static URealDecimal Zero { get; } = default(URealDecimal);

        byte[] _digits;
        byte[] Digits => _digits ?? _digits_empty;

        public int? Degree { get; }

        public bool IsZero => !Degree.HasValue;

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
            var digits_dic = new Dictionary<int, byte>();
            foreach (var _ in d1.GetDigitsAsPairs().Concat(d2.GetDigitsAsPairs()))
                Add(digits_dic, _.i, _.v);

            if (digits_dic.Count == 0) return Zero;

            var indexes = digits_dic
                .Where(p => p.Value != 0)
                .Select(p => p.Key)
                .ToArray();
            var maxIndex = indexes.Max();
            var minIndex = indexes.Min();

            var digits = new byte[maxIndex - minIndex + 1];
            for (var i = 0; i < digits.Length; i++)
            {
                var j = maxIndex - i;
                digits[i] = digits_dic.ContainsKey(j) ? digits_dic[j] : (byte)0;
            }
            return new URealDecimal(digits, maxIndex);
        }

        // Assert d1 >= d2.
        public static URealDecimal operator -(URealDecimal d1, URealDecimal d2)
        {
            var digits_dic = new Dictionary<int, byte>();
            foreach (var _ in d1.GetDigitsAsPairs())
                Add(digits_dic, _.i, _.v);
            foreach (var _ in d2.GetDigitsAsPairs())
                Subtract1(digits_dic, _.i, _.v);

            if (digits_dic.Count == 0) return Zero;

            var indexes = digits_dic
                .Where(p => p.Value != 0)
                .Select(p => p.Key)
                .ToArray();
            var maxIndex = indexes.Max();
            var minIndex = indexes.Min();

            var digits = new byte[maxIndex - minIndex + 1];
            for (var i = 0; i < digits.Length; i++)
            {
                var j = maxIndex - i;
                digits[i] = digits_dic.ContainsKey(j) ? digits_dic[j] : (byte)0;
            }
            return new URealDecimal(digits, maxIndex);
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

        IEnumerable<(int i, byte v)> GetDigitsAsPairs()
        {
            if (!Degree.HasValue) return Enumerable.Empty<(int, byte)>();

            var degree = Degree.Value;
            return Digits.Select((v, i) => (degree - i, v));
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
        static void Subtract1(Dictionary<int, byte> digits, int index, int value)
        {
            var v = digits.ContainsKey(index) ? digits[index] : 0;
            v -= value;

            if (v >= 0)
            {
                digits[index] = (byte)v;
            }
            else
            {
                Subtract1(digits, index + 1, 1);
                digits[index] = (byte)(v + 10);
            }
        }
    }
}
