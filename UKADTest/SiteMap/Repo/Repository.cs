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

        //compares 2 strings of type SiteMapUrl
        public bool DomainLinkEquality(string link)
        {
            if (appContext.SiteMapUrls.Any(o => o.SiteMapUrlString == link))
            {
                return true;
            }
            else return false;
        }

        //compares 2 strings of type URL
        public bool DomainEquality(string domain)
        {
            if (appContext.URLs.Any(o => o.Url == domain))
            {
                return true;
            }
            else return false;
        }

        //Loads all links of specific domain
        public IEnumerable<SiteMapUrl> GetDomainLinks(int dominaId)
        {
            return appContext.SiteMapUrls.Where(x => x.URL.ID == dominaId);
        }

        //Loads all domains
        public List<URL> GetAllDomains()
        {
            return appContext.URLs.ToList();
        }

        //get single domain by id
        public IEnumerable<URL> GetDomain(string domain)
        {
            return appContext.URLs.Where(x => x.Url == domain);
        }

        //add Link to DB
        public void UpLoadDomainLink(SiteMapUrl siteMapUrl)
        {
            appContext.SiteMapUrls.Add(new SiteMapUrl { SiteMapUrlString = siteMapUrl.SiteMapUrlString, AccessMS = siteMapUrl.AccessMS, URL = siteMapUrl.URL });
            appContext.SaveChanges();
        }

        //add Domain to Db
        public void UpLoadDomainString(URL url)
        {
            appContext.URLs.Add(url);
            appContext.SaveChanges();
        }

       
    }
}
