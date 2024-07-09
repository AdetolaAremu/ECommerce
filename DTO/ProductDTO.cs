using System.ComponentModel.DataAnnotations;
using ecommerce.Models;

namespace ecommerce.DTO
{
  public class ProductDTO
  {
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Title { get; set; }

     [Required]
    [StringLength(100, MinimumLength=10, ErrorMessage = "Description must not be less than 10 characters and cannot  be more than 1000 characters")]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string SKU { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public string CoverImage { get; set; }

    [Required]
    public ICollection<string> Images { get; set; }

    [Required]
    [StringLength(250)]
    public string Manufacturer { get; set; }

    [Required]
    public int UserId { get; set; }

    public ICollection<int> categoryIds { get; set; }

    public string Slug { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
  }

  public class CreateProductDTO
  {
    [Required]
    [StringLength(250)]
    public string Title { get; set; }

    [Required]
    [StringLength(100, MinimumLength=10, ErrorMessage = "Description must not be less than 10 characters and cannot  be more than 1000 characters")]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50)]
    public string SKU { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public string CoverImage { get; set; }

    [Required]
    public ICollection<string> Images { get; set; }

    [Required]
    [StringLength(250)]
    public string Manufacturer { get; set; }

    [Required]
    public int UserId { get; set; }

    public ICollection<int> categoryIds { get; set; }

    public string Slug { get; set; } // will be generated
  }
}