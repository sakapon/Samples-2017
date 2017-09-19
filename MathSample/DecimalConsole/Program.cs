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

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
