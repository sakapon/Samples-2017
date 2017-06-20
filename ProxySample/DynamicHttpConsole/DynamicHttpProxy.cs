using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DynamicHttpConsole
{
    public class DynamicHttpProxy : DynamicObject
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "Get")
            {
                result = Get(binder.CallInfo.ArgumentNames, args);
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        static string Get(ICollection<string> argNames, object[] args)
        {
            var query = argNames
                .Zip(args.Skip(1), (n, v) => new { n, v })
                .ToDictionary(_ => _.n, _ => _.v);
            return HttpHelper.Get((string)args[0], query);
        }

        public string Get(string uri, object query = null) => HttpHelper.Get(uri, query);
    }
}
