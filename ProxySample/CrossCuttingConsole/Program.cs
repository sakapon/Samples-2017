using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCuttingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            NorthwindBusinessTest();
        }

        static void NorthwindBusinessTest()
        {
            var nw = CrossCuttingProxy.CreateProxy<NorthwindBusiness>();
            var units = nw.SelectUnitsInStock();
            nw.InsertCategory("Books");
            nw.PropertyTest = 123;
            try
            {
                nw.ErrorTest();
            }
            catch (Exception)
            {
            }
        }
    }
}
