using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap.Repo
{
    public interface IRepository
    {
        //compares 2 strings of type SiteMapUrl
        bool DomainLinkEquality(string link);

        //compares 2 strings of type URL
        bool DomainEquality(string domain);

        //get single domain by id
        IEnumerable<URL> GetDomain(string domain);

        //Loads all domains
        List<URL> GetAllDomains();

        //Loads all links of specific domain
        IEnumerable<SiteMapUrl> GetDomainLinks(int dominaId);

        //add Domain to Db
        void UpLoadDomainString(URL url);

        //add Link to DB
        void UpLoadDomainLink(SiteMapUrl siteMapUrl);

    }
}
