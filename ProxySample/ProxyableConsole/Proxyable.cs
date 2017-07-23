using System;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace ProxyableConsole
{
    public static class Proxyable
    {
        public static IProxyable<TResult> Body<TResult>(Func<TResult> execute) => new BodyProxyable<TResult>(execute);
        public static IProxyable Body(Action execute) => new BodyProxyable(execute);

        public static IProxyable<TResult> Aspect<TResult>(this IProxyable<TResult> source, Func<Func<TResult>, TResult> execute) => new AspectProxyable<TResult>(source, execute);
        public static IProxyable Aspect(this IProxyable source, Action<Action> execute) => new AspectProxyable(source, execute);
    }

    public static class ProxyableExtension
    {
        public static IProxyable<TResult> TraceLog<TResult>(this IProxyable<TResult> source, [CallerMemberName]string caller = "") =>
            source.Aspect(f =>
            {
                try
                {
                    Console.WriteLine($"Begin: {caller}");
                    var result = f();
                    Console.WriteLine($"Success: {caller}");
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {caller}");
                    Console.WriteLine(ex);
                    throw;
                }
            });

        public static IProxyable<TResult> TransactionScope<TResult>(this IProxyable<TResult> source,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            double timeoutInSeconds = 30,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
            };

            return source.Aspect(f =>
            {
                using (var scope = new TransactionScope(scopeOption, transactionOptions))
                {
                    var result = f();

                    scope.Complete();

                    return result;
                }
            });
        }
    }
}
