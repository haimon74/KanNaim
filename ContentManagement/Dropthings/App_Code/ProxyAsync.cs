
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Caching;
using System.Web.Services;
using System.Web.Script.Services;

using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
using System.IO.Compression;
using AJAXASMXHandler;
using System.Text.RegularExpressions;
using Dropthings.Widget.Widgets.RSS;

namespace Dropthings.Web.Framework
{
    /// <summary>
    /// Summary description for Proxy
    /// </summary>
    [WebService(Namespace = "http://www.dropthings.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ProxyAsync : System.Web.Services.WebService
    {
        private const string CACHE_KEY = "ProxyAsync.";

        public ProxyAsync()
        {
        }

        private class GetStringState : AsyncWebMethodState
        {
            public HttpWebRequest Request;
            public string Url;
            public int CacheDuration;
            public GetStringState(object state) : base(state) { }
        }

        [ScriptMethod]
        public IAsyncResult BeginGetString(string url, int cacheDuration, AsyncCallback cb, object state)
        {
            // See if the response from the URL is already cached on server
            string cachedContent = Context.Cache[CACHE_KEY + url] as string;
            if (!string.IsNullOrEmpty(cachedContent))
            {
                this.CacheResponse(Context, cacheDuration);
                return new AsmxHandlerSyncResult(cachedContent);
            }

            HttpWebRequest request = this.CreateHttpWebRequest(url);
            // As we will stream the response, don't want to automatically decompress the content
            request.AutomaticDecompression = DecompressionMethods.None;

            GetStringState myState = new GetStringState(state);
            myState.Request = request;
            myState.Url = url;
            myState.CacheDuration = cacheDuration;

            return request.BeginGetResponse(cb, myState);
        }

        [ScriptMethod]
        public string EndGetString(IAsyncResult result)
        {
            GetStringState state = result.AsyncState as GetStringState;
            MemoryStream responseBuffer = new MemoryStream();

            HttpWebRequest request = state.Request;
            using (HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    // produce cache headers for response caching
                    this.CacheResponse(state.Context, state.CacheDuration);

                    string contentLength = response.GetResponseHeader("Content-Length") ?? "-1";
                    state.Context.Response.AppendHeader("Content-Length", contentLength);

                    string contentEncoding = response.GetResponseHeader("Content-Encoding") ?? "";
                    state.Context.Response.AppendHeader("Content-Encoding", contentEncoding);

                    state.Context.Response.ContentType = response.ContentType;

                    const int BUFFER_SIZE = 4 * 1024;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int dataReceived;
                    while ((dataReceived = stream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                    {
                        if (!state.Context.Response.IsClientConnected) return string.Empty;

                        // Transmit to client (browser) immediately
                        byte[] outBuffer = new byte[dataReceived];
                        Array.Copy(buffer, outBuffer, dataReceived);

                        state.Context.Response.BinaryWrite(outBuffer);
                        //state.Context.Response.Flush();

                        // Store in buffer so that we can cache the whole stuff
                        responseBuffer.Write(buffer, 0, dataReceived);
                    }

                    // If the content is compressed, decompress it
                    Stream contentStream = contentEncoding == "gzip" ?
                        (new GZipStream(responseBuffer, CompressionMode.Decompress) as Stream)
                        :
                        (contentEncoding == "deflate" ?
                            (new DeflateStream(responseBuffer, CompressionMode.Decompress) as Stream)
                            :
                            (responseBuffer as Stream));

                    // Cache the decompressed content so that we can return it next time
                    using (StreamReader reader = new StreamReader(contentStream, true))
                    {
                        string content = reader.ReadToEnd();

                        state.Context.Cache.Insert(CACHE_KEY + state.Url, content, null,
                        Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(state.CacheDuration),
                        CacheItemPriority.Normal, null);
                    }

                    state.Context.Response.Flush();

                    return null;
                }
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetString(string url, int cacheDuration)
        {
            var cachedContent = Context.Cache[CACHE_KEY + url] as string;
            if (null != cachedContent) return cachedContent;

            using (WebClient client = new WebClient())
            {
                var content = client.DownloadString(url);
                Context.Cache.Insert(CACHE_KEY + url, content, null,
                        Cache.NoAbsoluteExpiration,
                        TimeSpan.FromMinutes(cacheDuration),
                        CacheItemPriority.Normal, null);
                return content;
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Xml)]
        public string GetXml(string url, int cacheDuration)
        {
            return GetString(url, cacheDuration);
        }

        [ScriptMethod]
        public IAsyncResult BeginGetXml(string url, int cacheDuration, AsyncCallback cb, object state)
        {
            return BeginGetString(url, cacheDuration, cb, state);
        }

        [ScriptMethod]
        public string EndGetXml(IAsyncResult result)
        {
            return EndGetString(result);
        }
        
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]        
        public static bool IsUrlInCache(Cache cache, string url)
        {
            var data = cache[CACHE_KEY + url];
            return (null != data);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public object GetRss(string url, int count, int cacheDuration)
        {
            var feed = Context.Cache[CACHE_KEY + url] as XElement;
            if (feed == null)
            {
                // We have failed to load the RSS before. So, let's not try again.
                if (string.Empty == (Context.Cache[CACHE_KEY + url] as string)) return null;

                try
                {
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                    request.Timeout = 15000;
                    using (WebResponse response = request.GetResponse())
                    {
                        using (XmlTextReader reader = new XmlTextReader(new StreamReader(response.GetResponseStream(), true)))
                        {
                            feed = XElement.Load(reader);
                        }
                    }

                    if (feed == null) return null;
                    Context.Cache.Add(CACHE_KEY + url, feed, null, DateTime.MaxValue, TimeSpan.FromMinutes(15), CacheItemPriority.Normal, null);

                }
                catch(Exception x)
                {
                    Debug.WriteLine(x.ToString());
                    // Let's remember that we failed to load this RSS feed and we will not try to load it again 
                    // in next 15 mins
                    Context.Cache.Insert(CACHE_KEY + url, string.Empty, null, DateTime.MaxValue, TimeSpan.FromMinutes(15));
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
                                Title = StripTags(item.Element("title").Value, 200),
                                Link = item.Element("link").Value,
                                Description = StripTags(item.Element("description").Value, 200)
                            }).Take(count);

                // Atom
                else if (feed.Element(ns + "entry") != null)
                    return (from item in feed.Elements(ns + "entry")
                            select new RssItem
                            {
                                Title = StripTags(item.Element(ns + "title").Value, 200),
                                Link = item.Element(ns + "link").Attribute("href").Value,
                                Description = StripTags(item.Element(ns + "content").Value, 200)
                            }).Take(count);

                // Invalid
                else
                    return null;
            }
            finally
            {
                this.CacheResponse(Context, cacheDuration);
            }
        }

        private void CacheResponse(HttpContext context, int durationInMinutes)
        {
            TimeSpan duration = TimeSpan.FromMinutes(durationInMinutes);

            // With the new AJAX ASMX handler, there's no need for this hack to set maxAge value
            /*FieldInfo maxAge = HttpContext.Current.Response.Cache.GetType().GetField("_maxAge", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(HttpContext.Current.Response.Cache, duration);*/

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.Add(duration));
            context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            context.Response.Cache.SetMaxAge(duration);
        }

        private HttpWebRequest CreateHttpWebRequest(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Headers.Add("Accept-Encoding", "gzip");
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.MaximumAutomaticRedirections = 2;
            request.MaximumResponseHeadersLength = 4 * 1024;
            request.ReadWriteTimeout = 1 * 1000;
            request.Timeout = 5 * 1000;

            return request;
        }

        private static Regex _StripTagEx = new Regex("</?[^>]+>", RegexOptions.Compiled);
        private string StripTags(string html, int trimAt)
        {
            string plainText = _StripTagEx.Replace(html, string.Empty);
            return plainText.Substring(0, Math.Min(plainText.Length, trimAt));
        }
    }

}