using HtmlAgilityPack;
using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SiteMap.Data
{
    public static class DataAccess
    {
       
        public static List<string> GetRobotTxt(string url)
        {
            WebClient client = new WebClient();
            Stream stream;
            try
            {
                stream = client.OpenRead(url);

            }
            catch (WebException)
            {
                return null;
            }
            
            List<string> XMLSiteMapsLinks = new List<string>();

            string line;
            using (StreamReader file = new StreamReader(stream))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Sitemap"))
                    {
                        XMLSiteMapsLinks.Add(line);
                    }
                }
            }
            for (int i = 0; i < XMLSiteMapsLinks.Count; i++)
            {
                XMLSiteMapsLinks[i] = XMLSiteMapsLinks[i].Remove(0, 9);
            }
            return XMLSiteMapsLinks;           
        }

        public static List<string> GetUrls(string Link, List<string> DomainUrls)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(Link);

            }
            catch (WebException)
            {
                return null;
            }
            catch (XmlException)
            {
                return null;
            }

            XmlNodeList XMLUrlStrings = doc.GetElementsByTagName("loc");
            List<string> listXMLUrlStrings = new List<string>();

            for (int i = 0; i < XMLUrlStrings.Count; i++)
            {
                listXMLUrlStrings.Add(XMLUrlStrings[i].InnerXml);
            }

            if (listXMLUrlStrings[0] == "")
            {
                listXMLUrlStrings.RemoveAt(0);
            }

            if (listXMLUrlStrings.Count>1 && listXMLUrlStrings[0].Contains("xml"))
            {
                foreach(string x in listXMLUrlStrings)
                {
                    GetUrls(x, DomainUrls);
                }
            }
            foreach(string x in listXMLUrlStrings)
            {
                DomainUrls.Add(x);
            }
            return listXMLUrlStrings;
        }


        public static bool GetUrlsHtmlParse(string url, string domain, List<string> DomainUrls)
        {
            int found;
            string innerDomain;
            try
            {
                found = domain.IndexOf(".");
                innerDomain = domain.Substring(8, found - 8);

            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            if (innerDomain == null) return false;
            if (url.Contains("mailto:")) return false;
            if (url.Contains("http") && !url.Contains(innerDomain)) return false;

            string URL;
            
            if (!url.Contains("http"))
            {
                URL = domain + url;
            }
            else URL = url;

            HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
            HttpWebResponse response;
            try
            {
               response = request.GetResponse() as HttpWebResponse;

            }
            catch (WebException)
            {
                return false;
            }

            List<string> links = new List<string>();

            HtmlWeb hw = new HtmlWeb() { AutoDetectEncoding = true };
            HtmlDocument doc;
            try
            {
               doc = hw.Load(url);
            }
            catch (ArgumentException)
            {
                return false;
            }
            string TempContainerForUrl = null;

            try
            {
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute attribute = link.Attributes["href"];
                    if (!DomainUrls.Contains(domain + attribute.Value) && !DomainUrls.Contains(attribute.Value) && attribute.Value != "#" && attribute.Value != "/")
                    {
                        if (!attribute.Value.Contains("http")) TempContainerForUrl = domain + attribute.Value;
                        else TempContainerForUrl = attribute.Value;
                        if (TempContainerForUrl.Contains(innerDomain))
                            links.Add(TempContainerForUrl);
                    }
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
            

            foreach (string a in links)
            {
                if(!DomainUrls.Contains(a))
                DomainUrls.Add(a);
            }

            foreach (string x in links)
            {
                GetUrlsHtmlParse(x, domain, DomainUrls);
            }
            return true;
        }

        public static double ResponseTime(string url)
        {
            HttpWebRequest request;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);

            }
            catch (WebException)
            {
                return 0;
            }
            catch (UriFormatException)
            {
                return 0;
            }

            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            }
            catch (WebException)
            {
                return 0;
            }
           

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            return timeTaken.TotalMilliseconds;
        }
    }
}



