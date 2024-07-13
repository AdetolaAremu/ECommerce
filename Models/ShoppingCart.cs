using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
  public class ShoppingCart
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    // public int CartItemId { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
  }
}