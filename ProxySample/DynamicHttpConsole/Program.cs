using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicHttpConsole
{
    class Program
    {
        const string Uri_ZipCloud = "http://zipcloud.ibsnet.co.jp/api/search";
        const string Uri_Cgis_Xml = "http://zip.cgis.biz/xml/zip.php";

        static void Main(string[] args)
        {
            HttpHelperTest_ZipCloud();
            HttpHelperTest_Cgis();

            DynamicHttpProxyTest();
        }

        static void HttpHelperTest_ZipCloud()
        {
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud));
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud, new Dictionary<string, object> { { "zipcode", "6020881" } }));
            Console.WriteLine(HttpHelper.Get(Uri_ZipCloud, new { zipcode = "6068317" }));
        }

        static void HttpHelperTest_Cgis()
        {
            Console.WriteLine(HttpHelper.Get(Uri_Cgis_Xml));
            Console.WriteLine(HttpHelper.Get(Uri_Cgis_Xml, new Dictionary<string, object> { { "zn", "6048301" } }));
            Console.WriteLine(HttpHelper.Get(Uri_Cgis_Xml, new Dictionary<string, object> { { "zn", "501" }, { "ver", 1 } }));
            Console.WriteLine(HttpHelper.Get(Uri_Cgis_Xml, new { zn = "6050073" }));
            Console.WriteLine(HttpHelper.Get(Uri_Cgis_Xml, new { zn = "502", ver = 1 }));
        }

        static void DynamicHttpProxyTest()
        {
            dynamic http = new DynamicHttpProxy();

            Console.WriteLine(http.Get(Uri_Cgis_Xml));
            Console.WriteLine(http.Get(Uri_Cgis_Xml, new { zn = "1000001" }));
            Console.WriteLine(http.Get(Uri_Cgis_Xml, new { zn = "401", ver = 1 }));
            Console.WriteLine(http.Get(Uri_Cgis_Xml, zn: "1510052"));
            Console.WriteLine(http.Get(Uri_Cgis_Xml, zn: "402", ver: 1));
        }
    }
}
