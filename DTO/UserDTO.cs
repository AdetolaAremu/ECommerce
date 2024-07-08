using System.ComponentModel.DataAnnotations;
using ecommerce.Enums;

namespace ecommerce.DTO
{
  public class UserDTO
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool LoginStatus { get; set; } = false;
    public string? avatar { get; set; }
    public EnumUserType UserType { get; set; }
  }

  public class CreateUserDTO
  {
    [Required]
    [StringLength(200)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(200)]
    public string LastName { get; set; }

    [Required]
    [StringLength(200)]
    public bool LoginStatus { get; set; }

    [Required]
    public string? avatar { get; set; }

    [Required]
    public EnumUserType UserType { get; set; }
  }
}