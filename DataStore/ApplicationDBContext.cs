using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.DataStore
{
  public class ApplicationDBContext : DbContext
  {
    // public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CouponUsage> CouponUsages { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<ProductCategory>().HasKey(pc => new{ pc.CategoryId, pc.ProductId });

      modelBuilder.Entity<ProductCategory>()
        .HasOne(p => p.Product)
        .WithMany(pc => pc.ProductCategories)
        .HasForeignKey(p => p.ProductId);

      modelBuilder.Entity<ProductCategory>()
        .HasOne(p => p.Category)
        .WithMany(pc => pc.ProductCategories)
        .HasForeignKey(c => c.CategoryId);

      modelBuilder.Entity<User>()
        .HasMany(p => p.Products)
        .WithOne(u => u.User)
        .HasForeignKey(u => u.UserId);

      modelBuilder.Entity<User>()
        .HasMany(r => r.Reviews)
        .WithOne(u => u.User)
        .HasForeignKey(u => u.UserId);

      modelBuilder.Entity<User>()
        .HasOne(sc => sc.ShoppingCart)
        .WithOne(u => u.User)
        .HasForeignKey<ShoppingCart>(sc => sc.UserId);
      
      modelBuilder.Entity<User>()
        .HasMany(cu => cu.CouponUsage)
        .WithOne(u => u.User)
        .HasForeignKey(u => u.UserId);

      modelBuilder.Entity<User>()
        .HasMany(o => o.Orders)
        .WithOne(u => u.User)
        .HasForeignKey(u => u.UserId);

      modelBuilder.Entity<Product>()
        .HasMany(cu => cu.CouponUsage)
        .WithOne(p => p.Product)
        .HasForeignKey(p => p.ProductId);

      modelBuilder.Entity<Order>()
        .HasMany(o => o.OrderedProducts)
        .WithOne(o => o.Order)
        .HasForeignKey(o => o.OrderId);
    }
  }
}