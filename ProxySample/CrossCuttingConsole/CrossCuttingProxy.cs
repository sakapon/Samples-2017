using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace CrossCuttingConsole
{
    public class CrossCuttingProxy : RealProxy
    {
        public override IMessage Invoke(IMessage msg)
        {
            throw new NotImplementedException();
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class AspectAttribute : Attribute
    {
        public abstract IMethodReturnMessage Invoke(Func<IMethodReturnMessage> baseInvoke, MarshalByRefObject target, IMethodCallMessage methodCall);
    }
}
