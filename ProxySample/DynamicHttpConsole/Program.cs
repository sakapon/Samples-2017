using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicHttpConsole
{
    class Program
    {
        const string Uri_ZipCloud = "http://zipcloud.ibsnet.co.jp/api/search";
        const string Uri_Cgis = "http://zip.cgis.biz/xml/zip.php";

        static void Main(string[] args)
        {
            HttpHelperTest();
            DynamicHttpProxyTest();
        }

        static void HttpHelperTest()
        {
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud));
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud, new Dictionary<string, object> { { "zipcode", "6040973" } }));
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud, new { zipcode = "6040973" }));

            Console.WriteLine(HttpHelper.Get(Uri_Cgis));
            Console.WriteLine(HttpHelper.Get(Uri_Cgis, new { zn = "6020881" }));
        }

        static void DynamicHttpProxyTest()
        {
            dynamic http = new DynamicHttpProxy();

            Console.WriteLine(http.Get(Uri_Cgis));
            Console.WriteLine(http.Get(Uri_Cgis, new { zn = "6020881" }));
            Console.WriteLine(http.Get(Uri_Cgis, zn: "6020881"));
        }
    }
}
