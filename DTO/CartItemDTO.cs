using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class GetCartItemDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ShoppingCartId { get; set; }
  }

  public class CreateCartItemDTO
  {
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int ShoppingCartId { get; set; }
  }
}