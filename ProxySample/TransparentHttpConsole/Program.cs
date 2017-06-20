using System;
using System.Collections.Generic;
using System.Linq;

namespace TransparentHttpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpProxyTest();
            ObjectMethodsTest();
        }

        static void HttpProxyTest()
        {
            var zipCloud = HttpProxy.CreateProxy<IZipCloudService>();
            Console.WriteLine(zipCloud.Get("6020881"));

            var cgis = HttpProxy.CreateProxy<ICgisService>();
            Console.WriteLine(cgis.Get("6048301"));
            Console.WriteLine(cgis.Get("501", 1));
        }

        static void ObjectMethodsTest()
        {
            var zipCloud = HttpProxy.CreateProxy<IZipCloudService>();

            var h = zipCloud.GetHashCode();
            var b = zipCloud.Equals(zipCloud);
            var t = zipCloud.GetType();
            var s = zipCloud.ToString();
        }
    }

    // http://zipcloud.ibsnet.co.jp/doc/api
    [BaseUri("http://zipcloud.ibsnet.co.jp/api/search")]
    public interface IZipCloudService
    {
        string Get(string zipcode);
    }

    // http://zip.cgis.biz/
    [BaseUri("http://zip.cgis.biz/xml/zip.php")]
    public interface ICgisService
    {
        string Get(string zn);
        string Get(string zn, int ver);
    }
}
