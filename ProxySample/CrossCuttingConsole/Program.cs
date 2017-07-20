using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace CrossCuttingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var category = CrossCuttingProxy.CreateProxy<CategoryBusiness>();
            category.PropertyTest = 123;
            category.InsertCategory("Books");
            try
            {
                category.ErrorTest();
            }
            catch (Exception)
            {
            }
        }
    }

    public class CategoryBusiness : MarshalByRefObject
    {
        public int PropertyTest { get; [TraceLog]set; }

        [TraceLog]
        [TransactionScope(IsolationLevel.Serializable)]
        public void InsertCategory(string name)
        {
            Console.WriteLine("InsertCategory");
        }

        [TraceLog]
        public void ErrorTest()
        {
            throw new InvalidOperationException("This is an error test.");
        }
    }
}
