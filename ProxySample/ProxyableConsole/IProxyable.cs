using System;

namespace ProxyableConsole
{
    public interface IProxyable : IProxyable<object>
    {
        new void Execute();
    }

    class BodyProxyable : BodyProxyable<object>, IProxyable
    {
        public BodyProxyable(Action execute) : base(() =>
            {
                execute?.Invoke();
                return null;
            })
        {
        }

        void IProxyable.Execute() => Execute();
    }

    class AspectProxyable : AspectProxyable<object>, IProxyable
    {
        public AspectProxyable(IProxyable source, Action<Action> execute) : base(source, f =>
            {
                execute?.Invoke(() => f());
                return null;
            })
        {
        }

        void IProxyable.Execute() => Execute();
    }
}
