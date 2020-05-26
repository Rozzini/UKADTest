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
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ListOfDomains = _repository.GetAllDomains();
            return View();
        }

        
        public async Task<ActionResult> DropDownList(URL selectedUrl)
        {
            if(selectedUrl == null || selectedUrl.ID == 0)
            {
                TempData["ErrorMessage"] = "Search history is empty";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Action", new { selectedUrl.ID });
        }

        


        public IActionResult Action(int ID)
        {
            return View(_repository.GetDomainLinks(ID));
        }

        [HttpPost]
        public async Task<ActionResult> GetUrls(URL newURL)
        {
            if(newURL.Url == null || newURL.Url.Length < 5)
            {
                TempData["ErrorMessage"] = "Wrong format";
                return RedirectToAction("Index");
            }

            string URL;

            URL = newURL.Url + "/robots.txt";


            List<string> RobotsLinks = DataAccess.GetRobotTxt(URL);

            if (RobotsLinks == null || RobotsLinks.Count == 0)
            {
                TempData["ErrorMessage"] = "Wrong format or cannot find 'robots.txt'";
                return RedirectToAction("Index");
            }

            if (!_repository.DomainEquality(newURL.Url))
            {
                _repository.UpLoadDomainString(newURL);
            }
            else
            {
                return RedirectToAction("Action", new { _repository.GetDomain(newURL.Url).First().ID });
            }

            IEnumerable<URL> IenumCurrentUrl = _repository.GetDomain(newURL.Url);

            URL CurrentUrl = IenumCurrentUrl.First();


            List<string> DomainUrls = new List<string>();


            foreach (string x in RobotsLinks)
            {
                DataAccess.GetUrls(x, newURL.Url, DomainUrls);
            }

            SiteMapUrl siteMapUrl = new SiteMapUrl();
            foreach (string x in DomainUrls)
            {
                siteMapUrl.URL = CurrentUrl;
                siteMapUrl.SiteMapUrlString = x;
                siteMapUrl.AccessMS = DataAccess.ResponseTime(x);
                if (siteMapUrl.AccessMS != 0 && !_repository.DomainLinkEquality(x))
                {
                    _repository.UpLoadDomainLink(siteMapUrl);
                }
            }
            return RedirectToAction("Action", new { CurrentUrl.ID });
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
