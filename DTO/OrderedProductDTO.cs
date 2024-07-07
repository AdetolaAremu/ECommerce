using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class OrderedProductDTO
  {
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int productId { get; set; }
  }

  public class CreateOrderedProductDTO
  {
    [Required]
    public int OrderId { get; set; }

    [Required]
    public int productId { get; set; }
  }
}