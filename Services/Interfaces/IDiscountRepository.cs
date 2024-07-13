using System.ComponentModel.DataAnnotations;
using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IDiscountRepository
  {
    // get all discounts
    IEnumerable<Discount> GetAllDiscounts(int pageNumber, int pageSize);

    // get a discount
    Discount GetOneDiscount(int discountId);

    // discount exists
    bool DiscountExists(int discountId);

    // create a discount
    bool CreateDiscount(CreateDiscountDTO createDiscountDTO);

    // edit a discount
    bool UpdateDiscount(DiscountDTO discountDTO, int discountId);

    // delete a discount
    bool DeleteDiscount(Discount discount);

    bool SaveTransaction();
  }
}