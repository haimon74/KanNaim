// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

using System;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;

using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Dropthings.Widget.Widgets.RSS;

/// <summary>
/// Summary description for Proxy
/// </summary>

namespace Dropthings.Web.Framework
{
    [WebService(Namespace = "http://www.dropthings.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Proxy : System.Web.Services.WebService
    {

        public Proxy()
        {
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetString(string url, int cacheDuration)
        {
            // See if the response from the URL is already cached on server
            string cachedContent = Context.Cache[url] as string;
            if (!string.IsNullOrEmpty(cachedContent))
            {
                this.CacheResponse(cacheDuration);
                return cachedContent;
            }
            else
            {
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(url);
                    Context.Cache.Insert(url, response, null,
                        Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(cacheDuration),
                        CacheItemPriority.Normal, null);

                    // produce cache headers for response caching
                    this.CacheResponse(cacheDuration);

                    return response;
                }
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public string GetXml(string url, int cacheDuration)
        {
            return GetString(url, cacheDuration);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public object GetRss(string url, int count, int cacheDuration)
        {
            var feed = Context.Cache[url] as XElement;
            if (feed == null)
            {
                if (string.IsNullOrEmpty(Context.Cache[url] as string)) return null;
                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                    request.Timeout = 15000;
                    using (WebResponse response = request.GetResponse())
                    {
                        using (XmlTextReader reader = new XmlTextReader(response.GetResponseStream()))
                        {
                            feed = XElement.Load(reader);
                        }
                    }

                    if (feed == null) return null;
                    Context.Cache.Insert(url, feed, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));

                }
                catch
                {
                    Context.Cache[url] = string.Empty;
                    return null;
                }
            }

            XNamespace ns = "http://www.w3.org/2005/Atom";

            // see if RSS or Atom

            try
            {
                // RSS
                if (feed.Element("channel") != null)
                    return (from item in feed.Element("channel").Elements("item")
                            select new RssItem
                            {
                                Title = item.Element("title").Value,
                                Link = item.Element("link").Value,
                                Description = item.Element("description").Value
                            }).Take(count);

                // Atom
                else if (feed.Element(ns + "entry") != null)
                    return (from item in feed.Elements(ns + "entry")
                            select new RssItem
                            {
                                Title = item.Element(ns + "title").Value,
                                Link = item.Element(ns + "link").Attribute("href").Value,
                                Description = item.Element(ns + "content").Value
                            }).Take(count);

                // Invalid
                else
                    return null;
            }
            finally
            {
                this.CacheResponse(cacheDuration);
            }
        }

        private void CacheResponse(int durationInMinutes)
        {
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            // With the new AJAX ASMX handler, there's no need for this hack to set maxAge value
            /*FieldInfo maxAge = HttpContext.Current.Response.Cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(HttpContext.Current.Response.Cache, duration);*/

            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.Add(duration));
            HttpContext.Current.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            HttpContext.Current.Response.Cache.SetMaxAge(duration);
        }

    }

}