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
    public ICollection<string> Images { get; set; }
    public string Manufacturer { get; set; }

    public ICollection<Tag> Tags { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public User User { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public Discount Discount { get; set; }
    public Coupon Coupon { get; set; }
    public ICollection<CouponUsage> CouponUsage { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }
}