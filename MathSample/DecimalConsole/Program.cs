using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DecimalConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            URealDecimal_ToString();
            URealDecimal_Add();
            URealDecimal_Subtract();
            URealDecimal_Multiply();
            URealDecimal_Power();

            EnumerableHelperTest();
        }

        static void URealDecimal_ToString()
        {
            void Test(byte[] digits, int? degree = null) => WriteLine(new URealDecimal(digits, degree));

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
            void Test(URealDecimal expected, URealDecimal d1, URealDecimal d2)
            {
                if (expected != d1 + d2) WriteLine($"{expected} != {d1} + {d2}");
            }

            Test("0", "0", "0");
            Test("1", "0", "1");
            Test("3", "1", "2");
            Test("1.2", "1", "0.2");
            Test("130.42", "100.02", "30.4");
            Test("10", "1", "9");
            Test("20", "11", "9");
            Test("1", "0.8", "0.2");
        }

        static void URealDecimal_Subtract()
        {
            void Test(URealDecimal expected, URealDecimal d1, URealDecimal d2)
            {
                if (expected != d1 - d2) WriteLine($"{expected} != {d1} - {d2}");
            }

            Test("0", "0", "0");
            Test("1", "1", "0");
            Test("2", "3", "1");
            Test("1", "1.2", "0.2");
            Test("9", "10", "1");
            Test("11", "20", "9");
            Test("0.8", "1", "0.2");
        }

        static void URealDecimal_Multiply()
        {
            void Test(URealDecimal expected, URealDecimal d1, URealDecimal d2)
            {
                if (expected != d1 * d2) WriteLine($"{expected} != {d1} * {d2}");
            }

            Test("0", "0", "0");
            Test("0", "1", "0");
            Test("3", "3", "1");
            Test("0.6", "2", "0.3");
            Test("323", "17", "19");
            Test("0.00018", "0.02", "0.009");
            Test("485.45663", "60.007", "8.09");
            Test("1", "32", "0.03125");
        }

        static void URealDecimal_Power()
        {
            void Test(URealDecimal expected, URealDecimal d, int power)
            {
                if (expected != (d ^ power)) WriteLine($"{expected} != {d} ^ {power}");
            }

            Test("0", "0", 1);
            Test("1", "2", 0);
            Test("81", "3", 4);
            Test("0.001", "0.1", 3);
            Test("65536", "2", 16);
        }

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
