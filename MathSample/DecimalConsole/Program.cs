using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            URealDecimal_ToString();
            URealDecimal_Add();

            EnumerableHelperTest();
        }

        static void URealDecimal_ToString()
        {
            void Test(byte[] digits, int? degree = null) => Console.WriteLine(new URealDecimal(digits, degree));

            Test(null);
            Test(new byte[] { 1 }, 0);
            Test(new byte[] { 1, 2, 3 }, 2);
            Test(new byte[] { 1, 2, 3 }, 4);
            Test(new byte[] { 1 }, -1);
            Test(new byte[] { 1, 2, 3 }, -1);
            Test(new byte[] { 1, 2, 3 }, -3);
            Test(new byte[] { 1, 2, 3, 4 }, 1);
        }

        static void URealDecimal_Add()
        {
            URealDecimal_Add("0", "0", "0");
            URealDecimal_Add("1", "0", "1");
            URealDecimal_Add("3", "1", "2");
            URealDecimal_Add("1.2", "1", "0.2");
            URealDecimal_Add("10", "1", "9");
            URealDecimal_Add("20", "11", "9");
            URealDecimal_Add("1", "0.8", "0.2");
        }

        static void URealDecimal_Add(URealDecimal expected, URealDecimal d1, URealDecimal d2)
        {
            var actual = d1 + d2;
            if (expected != actual)
                Console.WriteLine($"{expected} != {d1} + {d2}");
        }

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
