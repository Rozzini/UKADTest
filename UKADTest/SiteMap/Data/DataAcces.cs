using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace SiteMap.Data
{
    public class DataAcces
    {
        public List<string> GetRobotTxt(string url)
        {
            string Url;
            Url = url + "/robots.txt";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(Url);          
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

        //public string[] GetUrls(List<string> Links)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(SiteMapString);
        //    string[] XMLUrlStrings = doc.InnerText.Split(new string[] { newURL.Url }, StringSplitOptions.None);
        //}
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

//HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newURL.Url + XMLUrlStringsr[1]);

//System.Diagnostics.Stopwatch timer = new Stopwatch();
//timer.Start();

//                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

//timer.Stop();

//                TimeSpan timeTaken = timer.Elapsed;
//}
//            else
//            {
//            }