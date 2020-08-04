using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Models
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {

        }

        public bool MyProperty { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
    }
}
