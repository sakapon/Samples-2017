using System;
using System.Transactions;

namespace CrossCuttingConsole
{
    public class NorthwindBusiness : MarshalByRefObject
    {
        [TraceLog]
        [TransactionScope]
        public int SelectUnitsInStock()
        {
            // cf. https://sakapon.wordpress.com/2011/10/02/dirtyread2/
            Console.WriteLine("SelectUnitsInStock");
            return 123;
        }

        [TraceLog]
        [TransactionScope(IsolationLevel.Serializable)]
        public void InsertCategory(string name)
        {
            // cf. https://sakapon.wordpress.com/2011/12/14/phantomread2/
            Console.WriteLine("InsertCategory");
        }

        public int PropertyTest { get; [TraceLog]set; }

        [TraceLog]
        public void ErrorTest()
        {
            throw new InvalidOperationException("This is an error test.");
        }
    }

    [CrossCutting]
    public class NorthwindBusiness2 : ContextBoundObject
    {
        [TraceLog]
        [TransactionScope]
        public int SelectUnitsInStock()
        {
            // cf. https://sakapon.wordpress.com/2011/10/02/dirtyread2/
            Console.WriteLine("SelectUnitsInStock");
            return 123;
        }

        [TraceLog]
        [TransactionScope(IsolationLevel.Serializable)]
        public void InsertCategory(string name)
        {
            // cf. https://sakapon.wordpress.com/2011/12/14/phantomread2/
            Console.WriteLine("InsertCategory");
        }

        public int PropertyTest { get; [TraceLog]set; }

        [TraceLog]
        public void ErrorTest()
        {
            throw new InvalidOperationException("This is an error test.");
        }
    }
}
