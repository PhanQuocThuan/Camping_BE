using Microsoft.EntityFrameworkCore;
using WebCamping.Models;

namespace WebCamping.Models
{
    public class CampingContext : DbContext
    {
        public CampingContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<WebCamping.Models.Brand> Brand { get; set; } = default!;

    }
}
