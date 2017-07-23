using System;

namespace ProxyableConsole
{
    public interface IProxyable<TResult>
    {
        TResult Execute();
    }

    class BodyProxyable<TResult> : IProxyable<TResult>
    {
        Func<TResult> execute;

        public BodyProxyable(Func<TResult> execute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public TResult Execute() => execute();
    }

    class AspectProxyable<TResult> : IProxyable<TResult>
    {
        IProxyable<TResult> source;
        Func<Func<TResult>, TResult> execute;

        public AspectProxyable(IProxyable<TResult> source, Func<Func<TResult>, TResult> execute)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public TResult Execute() => execute(source.Execute);
    }
}
