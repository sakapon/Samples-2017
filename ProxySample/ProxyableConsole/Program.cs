using System;
using System.Transactions;

namespace ProxyableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AspectTest();
            NorthwindBusinessTest();
        }

        static void AspectTest()
        {
            var result = Proxyable.Body(() =>
                {
                    Console.WriteLine("Body");
                    return 123;
                })
                .Aspect(f =>
                {
                    Console.WriteLine("Before");
                    var r = f();
                    Console.WriteLine("After");
                    return r;
                })
                .Aspect(f =>
                {
                    Console.WriteLine("Begin");
                    var r = f();
                    Console.WriteLine("End");
                    return r;
                })
                .Execute();

            Proxyable.Body(() => Console.WriteLine("Body"))
                .Aspect(a =>
                {
                    Console.WriteLine("Before");
                    a();
                    Console.WriteLine("After");
                })
                .Aspect(a =>
                {
                    Console.WriteLine("Begin");
                    a();
                    Console.WriteLine("End");
                })
                .Execute();
        }

        static void NorthwindBusinessTest()
        {
            var units = NorthwindBusiness.SelectUnitsInStock();
            NorthwindBusiness.InsertCategory("Books");
            try
            {
                NorthwindBusiness.ErrorTest();
            }
            catch (Exception)
            {
            }
        }
    }

    public static class NorthwindBusiness
    {
        // cf. https://sakapon.wordpress.com/2011/10/02/dirtyread2/
        public static int SelectUnitsInStock() =>
            Proxyable.Body(() =>
                {
                    Console.WriteLine("SelectUnitsInStock");
                    return 123;
                })
                .TransactionScope()
                .TraceLog()
                .Execute();

        // cf. https://sakapon.wordpress.com/2011/12/14/phantomread2/
        public static void InsertCategory(string name) =>
            Proxyable.Body(() => Console.WriteLine("InsertCategory"))
                .TransactionScope(IsolationLevel.Serializable)
                .TraceLog()
                .Execute();

        public static void ErrorTest() =>
            Proxyable.Body(() => throw new InvalidOperationException("This is an error test."))
                .TraceLog()
                .Execute();
    }
}
