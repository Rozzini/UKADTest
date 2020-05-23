using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap.Models
{
    public class SiteMapUrl
    {
        public int ID { get; set; }

        public string SiteMapUrlString { get; set; }

        public double AccessMS { get; set; }

        [ForeignKey("URL")]
        public int URLId { get; set; }
        public URL URL { get; set; }
    }
}
