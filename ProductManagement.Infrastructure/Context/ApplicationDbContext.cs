using Microsoft.EntityFrameworkCore;
using ProductManagement.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the tenants.
        /// </summary>
        /// <value>The tenants.</value>
        public DbSet<Product> Products { get; set; }
    }
}
