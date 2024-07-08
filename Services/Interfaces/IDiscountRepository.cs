using System.ComponentModel.DataAnnotations;
using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IDiscountRepository
  {
    // get all discounts
    IEnumerable<Discount> GetAllDiscounts();

    // get a discount
    Discount GetOneDiscount(int discountId);

    // create a discount
    bool CreateDiscount(CreateDiscountDTO createDiscountDTO);

    // edit a discount
    bool UpdateDiscount(DiscountDTO discountDTO);

    // delete a discount
    bool SaveTransaction();
  }
}