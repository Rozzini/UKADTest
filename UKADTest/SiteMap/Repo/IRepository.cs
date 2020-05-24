using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap.Repo
{
    public interface IRepository
    {
        bool Equality(string domain);
        IEnumerable<URL> GetDomain(string domain);

        IEnumerable<SiteMapUrl> GetDomainLinks(int dominaId);
        void UpLoadDomainString(URL url);

        void UpLoadDomainLink(SiteMapUrl siteMapUrl);

    }
}
