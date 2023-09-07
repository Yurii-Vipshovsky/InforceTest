using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShorderLink.Models;

namespace ShorderLink.Data
{
    public class ShorderLinkContext : DbContext
    {
        public ShorderLinkContext (DbContextOptions<ShorderLinkContext> options)
            : base(options)
        {
        }

        public DbSet<ShorderLink.Models.Link> Link { get; set; } = default!;

        public DbSet<ShorderLink.Models.User> User { get; set; } = default!;
    }
}
