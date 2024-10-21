using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsApi.Models
{
    public class ProductContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 1, ProductName = "Iphone 19", Price = 9500, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 2, ProductName = "Iphone 20", Price = 10500, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 3, ProductName = "Iphone 21", Price = 11500, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product { ProductId = 4, ProductName = "Iphone 22", Price = 12500, IsActive = true });
        }
        public DbSet<Product> Products => Set<Product>();
    }
}
