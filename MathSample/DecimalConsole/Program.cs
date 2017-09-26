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
            URealDecimal_Divide();
            URealDecimal_Power();

            RealDecimal_Inequality();
            RealDecimal_Add();
            RealDecimal_Multiply();
            RealDecimal_Power();

            EnumerableHelperTest();
        }

        static void URealDecimal_ToString()
        {
            void Test(string expected, byte[] digits, int? degree)
            {
                var d = new URealDecimal(digits, degree);
                if (expected != d.ToString()) WriteLine($"{expected} != {d}");
            }

            Test("0", null, null);
            Test("1", new byte[] { 1 }, 0);
            Test("123", new byte[] { 1, 2, 3 }, 2);
            Test("12300", new byte[] { 1, 2, 3 }, 4);
            Test("0.1", new byte[] { 1 }, -1);
            Test("0.123", new byte[] { 1, 2, 3 }, -1);
            Test("0.00123", new byte[] { 1, 2, 3 }, -3);
            Test("12.34", new byte[] { 1, 2, 3, 4 }, 1);
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

        static void URealDecimal_Divide()
        {
            void Test(URealDecimal expected, URealDecimal d1, URealDecimal d2)
            {
                if (expected != d1 / d2) WriteLine($"{expected} != {d1} / {d2}");
            }

            Test(0, 0, 1);
            Test(2, 2, 1);
            Test(1, 2, 2);
            Test(3, 0.06, 0.02);
            Test(27, 999, 37);
            Test("0.1428571428", 1, 7);
            Test("169.1095890", 12345, 73);

            void Test_20(URealDecimal expected, URealDecimal d1, URealDecimal d2)
            {
                if (expected != URealDecimal.Divide(d1, d2, 20)) WriteLine($"{expected} != {d1} / {d2}");
            }

            Test_20("0.058823529411764705882", 1, 17);
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

        static void RealDecimal_Inequality()
        {
            void Test(bool expected, RealDecimal d1, RealDecimal d2)
            {
                if (expected != d1 < d2) WriteLine($"{expected} != {d1} < {d2}");
            }

            Test(false, 0, 0);
            Test(false, 1, 1);
            Test(false, -1, -1);

            Test(true, 0, 1);
            Test(false, 0, -1);
            Test(false, 1, 0);
            Test(true, -1, 0);

            Test(true, 1, 2);
            Test(false, 2, 1);
            Test(false, -1, -2);
            Test(true, -2, -1);
            Test(true, -1, 1);
            Test(false, 1, -1);
        }

        static void RealDecimal_Add()
        {
            void Test(RealDecimal expected, RealDecimal d1, RealDecimal d2)
            {
                if (expected != d1 + d2) WriteLine($"{expected} != {d1} + {d2}");
            }

            Test(0, 0, 0);
            Test(1, 0, 1);
            Test(-1, 0, -1);
            Test(1, 1, 0);
            Test(-1, -1, 0);

            Test(3, 1, 2);
            Test(-3, -1, -2);

            Test(0, 1, -1);
            Test(0, -1, 1);
            Test(1, 2, -1);
            Test(1, -1, 2);
            Test(-1, 1, -2);
            Test(-1, -2, 1);
        }

        static void RealDecimal_Multiply()
        {
            void Test(RealDecimal expected, RealDecimal d1, RealDecimal d2)
            {
                if (expected != d1 * d2) WriteLine($"{expected} != {d1} * {d2}");
            }

            Test(0, 0, 0);
            Test(0, 0, 1);
            Test(0, 0, -1);
            Test(0, 1, 0);
            Test(0, -1, 0);

            Test(6, 3, 2);
            Test(6, -3, -2);
            Test(-6, 3, -2);
            Test(-6, -3, 2);
        }

        static void RealDecimal_Power()
        {
            var d2_31 = (RealDecimal)(-2) ^ 31;
            var d3_30 = (RealDecimal)3 ^ 30;
            var d3_100 = (RealDecimal)3 ^ 100;
        }

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
