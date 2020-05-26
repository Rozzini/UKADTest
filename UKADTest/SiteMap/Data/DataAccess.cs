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

        //this method checks if there are any site map links in domin/robots.txt 
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

            //searching for line that contains Sitemap
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

            //from txt file i get xml links in format of "Sitemap: https....." so i just remove first 9 chars
            for (int i = 0; i < XMLSiteMapsLinks.Count; i++)
            {
                XMLSiteMapsLinks[i] = XMLSiteMapsLinks[i].Remove(0, 9);
            }
            return XMLSiteMapsLinks;           
        }

        //this method gets all links from xml file
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


            //take data from all nodes with tag "loc"
            XmlNodeList XMLUrlStrings = doc.GetElementsByTagName("loc");
            List<string> listXMLUrlStrings = new List<string>();

            //adding urls from xml to list
            for (int i = 0; i < XMLUrlStrings.Count; i++)
            {
                listXMLUrlStrings.Add(XMLUrlStrings[i].InnerXml);
            }

            //sometimes first element could be empty, so just for this case
            if (listXMLUrlStrings[0] == "")
            {
                listXMLUrlStrings.RemoveAt(0);
            }


            //if list  of strings from xml file contains few more xmls, use recursion
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

        

        //method for mesuring respons time from web page
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



