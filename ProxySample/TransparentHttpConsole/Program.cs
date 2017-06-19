using System;
using System.Collections.Generic;
using System.Linq;

namespace TransparentHttpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var zipCloud = HttpProxy.CreateProxy<IZipCloudService>();
            Console.WriteLine(zipCloud.Get("6020881"));

            var cgis = HttpProxy.CreateProxy<ICgisService>();
            Console.WriteLine(cgis.Get("6048301"));
            Console.WriteLine(cgis.Get("501", 1));
        }
    }

    [BaseUri("http://zipcloud.ibsnet.co.jp/api/search")]
    public interface IZipCloudService
    {
        string Get(string zipcode);
    }

    [BaseUri("http://zip.cgis.biz/xml/zip.php")]
    public interface ICgisService
    {
        string Get(string zn);
        string Get(string zn, int ver);
    }
}
