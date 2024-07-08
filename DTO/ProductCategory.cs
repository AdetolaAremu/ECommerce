using System.ComponentModel.DataAnnotations;

namespace ecommerce.DTO
{
  public class ProductCategoryDTO
  {
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
  }

  public class CreateProductCategory
  {
    [Required]
    public int ProductId { get; set; }

    [Required]
    public int CategoryId { get; set; }
  }
}