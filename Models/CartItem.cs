using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
  public class CartItem
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }
  }
}