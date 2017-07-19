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
            var methodLog = $"{methodCall.MethodBase.DeclaringType.Name}.{methodCall.MethodName}({string.Join(", ", methodCall.InArgs)})";
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Begin: {methodLog}");

            var result = baseInvoke();

            if (result.Exception == null)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Success: {methodLog}");
            else
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Error: {methodLog}");
                Console.WriteLine(result.Exception);
            }

            return result;
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
            using (var scope = new TransactionScope(TransactionScopeOption, TransactionOptions))
            {
                var result = baseInvoke();

                scope.Complete();

                return result;
            }
        }
    }
}
