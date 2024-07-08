using System.ComponentModel.DataAnnotations;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ICategoryUsageRepository
  {
    // get all coupon usages
    IEnumerable<CouponUsage> GetAllCouponUsages();

    // get all coupon usages for a product
    IEnumerable<CouponUsage> GetCouponUsagesForAProduct(int ProductId);
  }
}