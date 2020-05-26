using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UKADTest.Models
{
    public class SiteMapUrl
    {
        public string SiteMapUrlString { get; set; }

        public int URLId { get; set; }
        public URL URL { get; set; }
    }
}
