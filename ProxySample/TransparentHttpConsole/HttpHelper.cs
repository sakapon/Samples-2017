using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace TransparentHttpConsole
{
    public static class HttpHelper
    {
        public static string Get(string uri, object query) =>
            query == null ? Get(uri) :
            IsPropertiesObject(query) ? Get(uri, ToDictionary(query)) :
            Get(uri, new { query });

        static bool IsPropertiesObject(object o)
        {
            if (o == null) return false;
            var oType = o.GetType();
            return oType.IsClass && oType != typeof(string);
        }

        static IDictionary<string, object> ToDictionary(object obj) =>
            TypeDescriptor.GetProperties(obj)
                .Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.GetValue(obj));

        public static string Get(string uri, IDictionary<string, object> query = null)
        {
            using (var web = new WebClient { Encoding = Encoding.UTF8 })
            {
                if (query != null)
                    web.QueryString = query.ToNameValueCollection();
                return web.DownloadString(uri);
            }
        }

        public static NameValueCollection ToNameValueCollection(this IDictionary<string, object> dictionary)
        {
            var collection = new NameValueCollection();
            foreach (var item in dictionary)
                collection[item.Key] = item.Value?.ToString();
            return collection;
        }
    }
}
