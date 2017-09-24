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
    }
}
