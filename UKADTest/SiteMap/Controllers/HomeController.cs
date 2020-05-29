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
            
            bool SiteMapMethod = true;


            if (newURL.Url == null || newURL.Url.Length < 5)
            {
                TempData["ErrorMessage"] = "Wrong format";
                return RedirectToAction("Index");
            }

            List<string> RobotsLinks = DataAccess.GetRobotTxt(newURL.Url + "/robots.txt");

            if (RobotsLinks == null || RobotsLinks.Count == 0)
            {
                SiteMapMethod = false;
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

            if (SiteMapMethod)
            {
                foreach (string x in RobotsLinks)
                {
                    DataAccess.GetUrls(x, DomainUrls);
                }
            }
            else DataAccess.GetUrlsHtmlParse(newURL.Url, CurrentUrl.Url, DomainUrls);

            SiteMapUrl siteMapUrl = new SiteMapUrl();
            string tempUrl;

            foreach (string x in DomainUrls)
            {
                siteMapUrl.URL = CurrentUrl;
                if (x.Contains(CurrentUrl.Url)) siteMapUrl.SiteMapUrlString = x;
                else
                {
                    tempUrl = CurrentUrl.Url + x;
                    siteMapUrl.SiteMapUrlString = tempUrl;
                }
                siteMapUrl.AccessMS = DataAccess.ResponseTime(x);
                if (siteMapUrl.AccessMS != 0 && !_repository.DomainLinkEquality(x))
                {
                    _repository.UpLoadDomainLink(siteMapUrl);
                }
            }
            return RedirectToAction("Action", new { CurrentUrl.ID });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
