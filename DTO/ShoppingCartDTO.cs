using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class GetShoppingCartDTO
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CartItemId { get; set; }
  }

  public class CreateShoppingCartDTO
  {
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int CartItemId { get; set; }
  }
}