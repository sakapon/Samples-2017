using System;

namespace ProxyableConsole
{
    public static class Proxyable
    {
        public static IProxyable<TResult> Body<TResult>(Func<TResult> execute) => new BodyProxyable<TResult>(execute);

        public static IProxyable<TResult> Aspect<TResult>(this IProxyable<TResult> source, Func<Func<TResult>, TResult> execute) => new AspectProxyable<TResult>(source, execute);
    }
}
