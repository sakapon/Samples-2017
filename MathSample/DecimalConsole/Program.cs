using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimalConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            EnumerableHelperTest();
        }

        static void EnumerableHelperTest()
        {
            var a = Enumerable.Range(0, 10).ToArray()
                .Split(3);
        }
    }
}
