using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

        public static List<string> GetUrls(string Link, string domainUrl, List<string> DomainUrls)
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
                    GetUrls(x, domainUrl, DomainUrls);
                }
            }
            foreach(string x in listXMLUrlStrings)
            {
                DomainUrls.Add(x);
            }
            return listXMLUrlStrings;
        }

        
        public static double ResponseTime(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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



