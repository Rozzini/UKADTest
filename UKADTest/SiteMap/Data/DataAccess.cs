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
            Stream stream = client.OpenRead(url);          
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

        public static string[] GetUrls(string Link, string domainUrl, List<string> DomainUrls)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Link);
            string[] XMLUrlStrings = doc.InnerText.Split(new string[] { domainUrl }, StringSplitOptions.None);
            List<string> listXMLUrlStrings = new List<string>(XMLUrlStrings);
            if(listXMLUrlStrings[0] == "")
            {
                listXMLUrlStrings.RemoveAt(0);
            }
            if (XMLUrlStrings.Length>1 && XMLUrlStrings[1].Contains("xml"))
            {
                foreach(string x in listXMLUrlStrings)
                {
                    GetUrls(domainUrl + x, domainUrl, DomainUrls);
                }
            }
            foreach(string x in listXMLUrlStrings)
            {
                DomainUrls.Add(x);
            }
            return XMLUrlStrings;
        }

        //public void countDown(int integer)
        //{
        //    if (integer > 0)
        //    {
        //        ListBox1.Items.Add(integer.ToString());
        //        integer--;
        //        countDown(integer);
        //    }
        //}


        public static double ResponseTime(string domain, string url, Func<SiteMapUrl> AddLinkToDbMethod)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(domain + url);
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            return timeTaken.TotalMilliseconds;
        }
    }
}



//XmlDocument doc = new XmlDocument();
//            
//    doc.Load(SiteMapString);
//            string[] XMLUrlStrings = doc.InnerText.Split(new string[] { newURL.Url }, StringSplitOptions.None);
//            if(XMLUrlStrings[1].Contains("xml"))
//            {
//                XmlDocument docr = new XmlDocument();
//docr.Load(newURL.Url + XMLUrlStrings[1].ToString());
//                string[] XMLUrlStringsr = docr.InnerText.Split(new string[] { newURL.Url }, StringSplitOptions.None);

