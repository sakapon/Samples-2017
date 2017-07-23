using System;

namespace ProxyableConsole
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
