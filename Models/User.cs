using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ecommerce.Enums;

namespace ecommerce.Models
{
  public class User
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool LoginStatus { get; set; } = true;
    public string? avatar { get; set; }
    public EnumUserType UserType { get; set; } = EnumUserType.Seller;

    public ICollection<Product> Products { get; set; }
    public ICollection<Review> Reviews { get; set; }
    public ShoppingCart ShoppingCart { get; set; }
    public ICollection<CouponUsage> CouponUsages { get; set; }
    public ICollection<Order> Orders { get; set; }
  }
}