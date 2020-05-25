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
            List<URL> DomainLinks = new List<URL>();
            DomainLinks = _repository.GetAllDomains();
            ViewBag.ListOfDomains = DomainLinks;
            return View();
        }




        public async Task<ActionResult> DropDownList(URL selectedUrl)
        {
            if(selectedUrl.ID == 0)
            {
                ModelState.AddModelError("", "Select domain");
            }

            return RedirectToAction("Action", new { selectedUrl.ID });
        }

        List<string> DomainUrls = new List<string>();


        public IActionResult Action(int ID)
        {
            return View(_repository.GetDomainLinks(ID));
        }

        [HttpPost]
        public async Task<ActionResult> GetUrls(URL newURL)
        {
            IEnumerable<URL> IenumCurrentUrl = _repository.GetDomain(newURL.Url);

            URL CurrentUrl = IenumCurrentUrl.First();

            if (!_repository.Equality(newURL.Url))
            {
                _repository.UpLoadDomainString(newURL);
            }
            else
            {
                return RedirectToAction("Action", new { CurrentUrl.ID });
            }


            

            string URL;

            URL = newURL.Url + "/robots.txt";

            List<string> RobotsLinks = DataAccess.GetRobotTxt(URL);


            

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
            return RedirectToAction("Action", new { CurrentUrl.ID });
        }

        //public async ActionResult SearchHistoty(URL newURL)
        //{
        //    URLsViewModel uRLsViewModel = new URLsViewModel();
        //    foreach (URL x in _repository.GetAllDomains())
        //    {
        //        uRLsViewModel.URLs.Add(x);

        //    }
        //}

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
