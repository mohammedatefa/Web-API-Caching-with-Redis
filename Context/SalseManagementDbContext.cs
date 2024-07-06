using Caching_with_Redis.Models;
using Microsoft.EntityFrameworkCore;

namespace Caching_with_Redis.Context
{
    public class SalseManagementDbContext:DbContext
    {
        public SalseManagementDbContext(DbContextOptions<SalseManagementDbContext>options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
