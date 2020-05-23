using Microsoft.EntityFrameworkCore;
using SiteMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMap
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<URL> URLs { get; set; }
        public DbSet<SiteMapUrl> SiteMapUrls { get; set; }
    }

}

