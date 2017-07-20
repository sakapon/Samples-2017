using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace CrossCuttingConsole
{
    public class CrossCuttingProxy : RealProxy
    {
        public static T CreateProxy<T>() where T : MarshalByRefObject, new()
        {
            return (T)new CrossCuttingProxy(new T()).GetTransparentProxy();
        }

        public MarshalByRefObject Target { get; private set; }

        // For non-ContextBoundObject.
        internal CrossCuttingProxy(MarshalByRefObject target) : base(target.GetType())
        {
            Target = target;
        }

        // For ContextBoundObject.
        internal CrossCuttingProxy(Type classToProxy) : base(classToProxy)
        {
        }

        public override IMessage Invoke(IMessage msg)
        {
            if (msg is IMethodCallMessage methodCall)
                return InvokeMethod(methodCall);

            if (msg is IConstructionCallMessage constructionCall)
                return InvokeConstructor(constructionCall);

            throw new InvalidOperationException();
        }

        IMethodReturnMessage InvokeMethod(IMethodCallMessage methodCall)
        {
            Func<IMethodReturnMessage> baseInvoke = () => RemotingServices.ExecuteMessage(Target, methodCall);

            var newInvoke = methodCall.MethodBase.GetCustomAttributes<AspectAttribute>(true)
                .Reverse()
                .Aggregate(baseInvoke, (f, a) => () => a.Invoke(f, Target, methodCall));

            return newInvoke();
        }

        IConstructionReturnMessage InvokeConstructor(IConstructionCallMessage constructionCall)
        {
            var constructionReturn = InitializeServerObject(constructionCall);
            Target = GetUnwrappedServer();
            SetStubData(this, Target);
            return constructionReturn;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public abstract class AspectAttribute : Attribute
    {
        public abstract IMethodReturnMessage Invoke(Func<IMethodReturnMessage> baseInvoke, MarshalByRefObject target, IMethodCallMessage methodCall);
    }
}
