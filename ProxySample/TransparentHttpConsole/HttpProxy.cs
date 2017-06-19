using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace TransparentHttpConsole
{
    public static class HttpProxy
    {
        public static IService CreateProxy<IService>()
        {
            return (IService)new HttpProxy<IService>().GetTransparentProxy();
        }
    }

    public class HttpProxy<IService> : RealProxy
    {
        public string BaseUri { get; }

        public HttpProxy() : base(typeof(IService))
        {
            var serviceType = typeof(IService);

            if (!serviceType.IsInterface) throw new InvalidOperationException("IService must be an interface.");

            var baseUriAttribute = serviceType.GetCustomAttribute<BaseUriAttribute>(true);
            if (baseUriAttribute == null) throw new InvalidOperationException("IService must have a BaseUriAttribute.");
            BaseUri = baseUriAttribute.BaseUri;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = (IMethodCallMessage)msg;
            if (methodCall.MethodBase.DeclaringType == typeof(object))
                throw new InvalidOperationException("Methods of the Object type can not be invoked.");

            var query = Enumerable.Range(0, methodCall.InArgCount)
                .ToDictionary(methodCall.GetInArgName, methodCall.GetInArg);

            switch (methodCall.MethodName)
            {
                case "Get":
                    return CreateReturnMessage(HttpHelper.Get(BaseUri, query), methodCall);
                default:
                    throw new InvalidOperationException($"The method \"{methodCall.MethodName}\" is not available.");
            }
        }

        static IMethodReturnMessage CreateReturnMessage(object returnValue, IMethodCallMessage methodCall)
        {
            return new ReturnMessage(returnValue, new object[0], 0, methodCall.LogicalCallContext, methodCall);
        }
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class BaseUriAttribute : Attribute
    {
        public string BaseUri { get; }

        public BaseUriAttribute(string baseUri)
        {
            BaseUri = baseUri;
        }
    }
}
