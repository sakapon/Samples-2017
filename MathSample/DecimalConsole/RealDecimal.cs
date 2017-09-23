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

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
