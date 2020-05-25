using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap.Models
{
    public class URL
    {
        [Key]
        public int ID { get; set; }

        public string Url { get; set; }

        public ICollection<SiteMapUrl> siteMapUrls { get; set; }
    }
}
