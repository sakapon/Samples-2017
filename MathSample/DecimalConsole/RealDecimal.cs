using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    public struct RealDecimal
    {
        public static RealDecimal Zero { get; } = default(RealDecimal);

        public URealDecimal AbsoluteValue { get; }
        public bool? IsPositive { get; }

        public int? Degree => AbsoluteValue.Degree;
        public bool IsZero => AbsoluteValue.IsZero;

        public byte this[int index] => AbsoluteValue[index];

        internal RealDecimal(URealDecimal absoluteValue, bool? isPositive)
        {
            if (absoluteValue.IsZero ^ !isPositive.HasValue) throw new ArgumentException("");

            AbsoluteValue = absoluteValue;
            IsPositive = isPositive;
        }

        public override string ToString() =>
            $"{(IsPositive == false ? "-" : "")}{AbsoluteValue}";

        public override bool Equals(object obj) =>
            obj is RealDecimal d && this == d;

        public override int GetHashCode() =>
            AbsoluteValue.GetHashCode();

        static RealDecimal FromString(string value)
        {
            var hasMinus = value?.StartsWith("-") ?? throw new ArgumentNullException();
            URealDecimal ud = value.TrimStart('-');
            var isPositive = ud.IsZero ? default(bool?) : !hasMinus;

            return new RealDecimal(ud, isPositive);
        }

        static RealDecimal FromInt32(int value) => value.ToString();
        static RealDecimal FromDouble(double value) => value.ToString();

        public static implicit operator RealDecimal(string value) => FromString(value);
        public static implicit operator RealDecimal(int value) => FromInt32(value);
        public static implicit operator RealDecimal(double value) => FromDouble(value);
        public static implicit operator RealDecimal(URealDecimal value) => value.IsZero ? Zero : new RealDecimal(value, true);

        public static RealDecimal operator +(RealDecimal d) => d;
        public static RealDecimal operator -(RealDecimal d) => new RealDecimal(d.AbsoluteValue, !d.IsPositive);

        public static RealDecimal operator +(RealDecimal d1, RealDecimal d2)
        {
            if ((d1.IsPositive == false) == (d2.IsPositive == false))
                return d1.IsPositive == false ?
                    new RealDecimal(d1.AbsoluteValue + d2.AbsoluteValue, false) :
                    d1.AbsoluteValue + d2.AbsoluteValue;

            if (d1.IsPositive == false)
                return d2.AbsoluteValue >= d1.AbsoluteValue ?
                    d2.AbsoluteValue - d1.AbsoluteValue :
                    new RealDecimal(d1.AbsoluteValue - d2.AbsoluteValue, false);
            else
                return d1.AbsoluteValue >= d2.AbsoluteValue ?
                    d1.AbsoluteValue - d2.AbsoluteValue :
                    new RealDecimal(d2.AbsoluteValue - d1.AbsoluteValue, false);
        }

        public static RealDecimal operator -(RealDecimal d1, RealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static RealDecimal operator *(RealDecimal d1, RealDecimal d2)
        {
            throw new NotImplementedException();
        }

        public static RealDecimal operator /(RealDecimal d1, RealDecimal d2)
        {
            throw new NotImplementedException();
        }

        // Power
        public static RealDecimal operator ^(RealDecimal d, int power)
        {
            if (power < 0) throw new NotImplementedException();

            RealDecimal result = "1";
            for (var i = 0; i < power; i++)
                result *= d;
            return result;
        }

        public static RealDecimal operator <<(RealDecimal d, int shift) =>
            new RealDecimal(d.AbsoluteValue << shift, d.IsPositive);

        public static RealDecimal operator >>(RealDecimal d, int shift) =>
            d << -shift;

        public static bool operator ==(RealDecimal d1, RealDecimal d2) =>
            d1.IsPositive == d2.IsPositive && d1.AbsoluteValue == d2.AbsoluteValue;

        public static bool operator !=(RealDecimal d1, RealDecimal d2) =>
            !(d1 == d2);

        public static bool operator <(RealDecimal d1, RealDecimal d2)
        {
            if ((d1.IsPositive == false) ^ (d2.IsPositive == false))
                return d1.IsPositive == false;

            return d1.IsPositive == false ?
                d1.AbsoluteValue > d2.AbsoluteValue :
                d1.AbsoluteValue < d2.AbsoluteValue;
        }

        public static bool operator >(RealDecimal d1, RealDecimal d2)
        {
            if ((d1.IsPositive == false) ^ (d2.IsPositive == false))
                return d2.IsPositive == false;

            return d1.IsPositive == false ?
                d1.AbsoluteValue < d2.AbsoluteValue :
                d1.AbsoluteValue > d2.AbsoluteValue;
        }

        public static bool operator <=(RealDecimal d1, RealDecimal d2) =>
            !(d1 > d2);

        public static bool operator >=(RealDecimal d1, RealDecimal d2) =>
            !(d1 < d2);
    }
}
