using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class CartItemDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Title {get; set;}
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int ShoppingCartId { get; set; }
  }

  public class CreateCartItemDTO
  {
    [Required]
    public int ProductId { get; set; }

    public int couponId {get; set;}

    public int couponCode {get; set;}

    public int disCountPercentage {get; set;}

    public int discountId {get; set;}

    // [Required]
    // public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }
  }
}