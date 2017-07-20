using System;
using System.Runtime.Remoting.Messaging;
using System.Transactions;

namespace CrossCuttingConsole
{
    public class TraceLogAttribute : AspectAttribute
    {
        public override IMethodReturnMessage Invoke(Func<IMethodReturnMessage> baseInvoke, MarshalByRefObject target, IMethodCallMessage methodCall)
        {
            var methodInfo = $"{methodCall.MethodBase.DeclaringType.Name}.{methodCall.MethodName}({string.Join(", ", methodCall.InArgs)})";
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Begin: {methodInfo}");

            var result = baseInvoke();

            if (result.Exception == null)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Success: {methodInfo}");
            else
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}: Error: {methodInfo}");
                Console.WriteLine(result.Exception);
            }

            return result;
        }
    }

    public class TransactionScopeAttribute : AspectAttribute
    {
        public TransactionOptions TransactionOptions { get; }
        public TransactionScopeOption TransactionScopeOption { get; }

        public TransactionScopeAttribute(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            double timeoutInSeconds = 30,
            TransactionScopeOption scopeOption = TransactionScopeOption.Required)
        {
            TransactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
            };
            TransactionScopeOption = scopeOption;
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
