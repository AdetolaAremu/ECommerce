using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
  public class Product
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SKU { get; set; }
    public int Quantity { get; set; }
    public string CoverImage { get; set; }
    public string Manufacturer { get; set; }
    public string Slug { get; set; }

    public ICollection<Tag> Tags { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<ProductCategory> ProductCategories { get; set; }
    public ICollection<ProductImages> ProductImages { get; set; }
    public Discount Discount { get; set; }
    public ICollection<Coupon> Coupons { get; set; }
    public ICollection<CouponUsage> CouponUsages { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}