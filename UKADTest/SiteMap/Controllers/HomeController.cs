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
using SiteMap.Repo;
using SiteMap.Data;

namespace SiteMap.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IRepository repository, ILogger<HomeController> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost]
        public void GetMap(URL newURL)
        {
            if(!_repository.Equality(newURL.Url))
            {
                _repository.UpLoadDomainString(newURL);
            }

            IEnumerable<URL> IenumCurrentUrl = _repository.GetDomain(newURL.Url);

            URL CurrentUrl = IenumCurrentUrl.First();

            string URL;

            URL = newURL.Url + "/robots.txt";

            List<string> RobotsLinks = DataAccess.GetRobotTxt(URL);

            int i = 0;  //variable for placing break point here

            List<string> DomainUrls = new List<string>();

            foreach(string x in RobotsLinks)
            {
                DataAccess.GetUrls(x, newURL.Url, DomainUrls);
            }

            SiteMapUrl siteMapUrl = new SiteMapUrl();
            foreach (string x in DomainUrls)
            {
                siteMapUrl.URL = CurrentUrl;
                siteMapUrl.SiteMapUrlString = x;
                siteMapUrl.AccessMS = DataAccess.ResponseTime(CurrentUrl.Url, x);
                _repository.UpLoadDomainLink(siteMapUrl);
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
