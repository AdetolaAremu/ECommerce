using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class CartItemDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ShoppingCartId { get; set; }
  }

  public class CreateCartItemDTO
  {
    [Required]
    public int ProductId { get; set; }
  }
}