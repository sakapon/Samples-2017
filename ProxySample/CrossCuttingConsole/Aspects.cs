using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Transactions;

namespace CrossCuttingConsole
{
    public class TraceLogAttribute : AspectAttribute
    {
        public override IMethodReturnMessage Invoke(Func<IMethodReturnMessage> baseInvoke, MarshalByRefObject target, IMethodCallMessage methodCall)
        {
            throw new NotImplementedException();
        }
    }

    public class TransactionScopeAttribute : AspectAttribute
    {
        public TransactionScopeOption TransactionScopeOption { get; }
        public TransactionOptions TransactionOptions { get; }

        public TransactionScopeAttribute(
            TransactionScopeOption scopeOption = TransactionScopeOption.Required,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            double timeoutInSeconds = 30)
        {
            TransactionScopeOption = scopeOption;
            TransactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
            };
        }

        public override IMethodReturnMessage Invoke(Func<IMethodReturnMessage> baseInvoke, MarshalByRefObject target, IMethodCallMessage methodCall)
        {
            throw new NotImplementedException();
        }
    }
}
