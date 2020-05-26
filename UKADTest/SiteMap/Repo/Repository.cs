using Microsoft.EntityFrameworkCore;
using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteMap.Repo
{
    public class Repository : IRepository
    {
        private readonly AppContext appContext;

        public Repository(AppContext appDbContext)
        {
            this.appContext = appDbContext;
        }

        public bool DomainLinkEquality(string link)
        {
            if (appContext.SiteMapUrls.Any(o => o.SiteMapUrlString == link))
            {
                return true;
            }
            else return false;
        }

        public bool DomainEquality(string domain)
        {
            if (appContext.URLs.Any(o => o.Url == domain))
            {
                return true;
            }
            else return false;
        }

        public IEnumerable<SiteMapUrl> GetDomainLinks(int dominaId)
        {
            return appContext.SiteMapUrls.Where(x => x.URL.ID == dominaId);
        }

        public List<URL> GetAllDomains()
        {
            return appContext.URLs.ToList();
        }

        public IEnumerable<URL> GetDomain(string domain)
        {
            return appContext.URLs.Where(x => x.Url == domain);
        }

        public void UpLoadDomainLink(SiteMapUrl siteMapUrl)
        {
            appContext.SiteMapUrls.Add(new SiteMapUrl { SiteMapUrlString = siteMapUrl.SiteMapUrlString, AccessMS = siteMapUrl.AccessMS, URL = siteMapUrl.URL });
            appContext.SaveChanges();
        }

        public void UpLoadDomainString(URL url)
        {
            appContext.URLs.Add(url);
            appContext.SaveChanges();
        }

       
    }
}
