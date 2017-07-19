using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossCuttingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var category = CrossCuttingProxy.CreateProxy<CategoryBusiness>();
            category.Property1 = 100;
            category.InsertCategory("New Category");
        }
    }

    public class CategoryBusiness : MarshalByRefObject
    {
        public int Property1 { get; [TraceLog]set; }

        [TraceLog]
        [TransactionScope]
        public void InsertCategory(string name)
        {
            Console.WriteLine("InsertCategory");
        }
    }
}
