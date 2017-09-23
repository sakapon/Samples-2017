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
            URealDecimal_Add("0", "0", "0");
            URealDecimal_Add("1", "0", "1");
            URealDecimal_Add("3", "1", "2");
            URealDecimal_Add("1.2", "1", "0.2");
            URealDecimal_Add("10", "1", "9");
            URealDecimal_Add("20", "11", "9");
            URealDecimal_Add("1", "0.8", "0.2");
        }

        static void URealDecimal_Subtract()
        {
            URealDecimal_Subtract("0", "0", "0");
            URealDecimal_Subtract("1", "1", "0");
            URealDecimal_Subtract("2", "3", "1");
            URealDecimal_Subtract("1", "1.2", "0.2");
            URealDecimal_Subtract("9", "10", "1");
            URealDecimal_Subtract("11", "20", "9");
            URealDecimal_Subtract("0.8", "1", "0.2");
        }

        static void URealDecimal_Multiply()
        {
            URealDecimal_Multiply("0", "0", "0");
            URealDecimal_Multiply("0", "1", "0");
            URealDecimal_Multiply("3", "3", "1");
            URealDecimal_Multiply("0.6", "2", "0.3");
            URealDecimal_Multiply("323", "17", "19");
            URealDecimal_Multiply("0.00018", "0.02", "0.009");
            URealDecimal_Multiply("1", "32", "0.03125");
        }

        static void URealDecimal_Power()
        {
            URealDecimal_Power("0", "0", 1);
            URealDecimal_Power("1", "2", 0);
            URealDecimal_Power("81", "3", 4);
            URealDecimal_Power("0.001", "0.1", 3);
            URealDecimal_Power("65536", "2", 16);
        }

        static void URealDecimal_Add(URealDecimal expected, URealDecimal d1, URealDecimal d2)
        {
            if (expected != d1 + d2) WriteLine($"{expected} != {d1} + {d2}");
        }

        static void URealDecimal_Subtract(URealDecimal expected, URealDecimal d1, URealDecimal d2)
        {
            if (expected != d1 - d2) WriteLine($"{expected} != {d1} - {d2}");
        }

        static void URealDecimal_Multiply(URealDecimal expected, URealDecimal d1, URealDecimal d2)
        {
            if (expected != d1 * d2) WriteLine($"{expected} != {d1} * {d2}");
        }

        static void URealDecimal_Power(URealDecimal expected, URealDecimal d, int power)
        {
            if (expected != (d ^ power)) WriteLine($"{expected} != {d} ^ {power}");
        }

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
