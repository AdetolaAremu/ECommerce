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
    Discount GetDiscount(int discountId);

    // get product discount
    Discount GetProductDiscount(int productId);

    // discount exists
    bool DiscountExists(int discountId);

    // discount for product exists
    bool DiscountExistsForProduct(int productId);

    // create a discount
    bool CreateDiscount(CreateDiscountDTO createDiscountDTO);

    // edit a discount
    bool UpdateDiscount(DiscountDTO discountDTO, int discountId);

    // delete a discount
    bool DeleteDiscount(Discount discount);

    // is discount valid
    bool isDiscountValid(Discount discount);

    bool SaveTransaction();
  }
}