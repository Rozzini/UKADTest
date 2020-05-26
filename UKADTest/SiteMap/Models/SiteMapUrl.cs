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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string SiteMapUrlString { get; set; }

        public double AccessMS { get; set; }

        public virtual URL URL { get; set; }

    }
}
