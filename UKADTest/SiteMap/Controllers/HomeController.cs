using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteMap.Models;

namespace SiteMap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void GetMap(URL newURL)
        {
            string SiteMapString = null;
            string url;
            string SiteMap = "Sitemap:";
            string DotXML = ".xml";
            url = newURL.Url + "/robots.txt";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(url);
            //StreamReader reader = new StreamReader(stream);
            //String content = reader.ReadToEnd();
            List<string> content = new List<string>();
            string line;
            using (StreamReader file = new StreamReader(stream))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("Sitemap"))
                    {
                        content.Add(line);
                    }
                }
            }
           for(int i = 0; i<content.Count; i++)
            {
                content[i] = content[i].Remove(0, 9);
            }
            for (int i = 0; i < content.Count; i++)
            {
                SiteMapString = content[i];
            }
            
            XmlDocument doc = new XmlDocument();            
            doc.Load(SiteMapString);
            string[] XMLUrlStrings = doc.InnerText.Split(new string[] { newURL.Url }, StringSplitOptions.None);
            if(XMLUrlStrings[1].Contains("xml"))
            {
                XmlDocument docr = new XmlDocument();
                docr.Load(newURL.Url + XMLUrlStrings[1].ToString());
                string[] XMLUrlStringsr = docr.InnerText.Split(new string[] { newURL.Url }, StringSplitOptions.None);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newURL.Url + XMLUrlStringsr[1]);

                System.Diagnostics.Stopwatch timer = new Stopwatch();
                timer.Start();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                timer.Stop();

                TimeSpan timeTaken = timer.Elapsed;
            }
            else
            {
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
