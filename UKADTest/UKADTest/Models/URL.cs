using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UKADTest.Models
{
    public class URL
    {
        [Required]
        public string Url { get; set; }
    }
}
