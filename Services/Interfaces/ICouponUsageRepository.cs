using System.ComponentModel.DataAnnotations;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ICouponUsageRepository
  {
    // get all coupon usages
    IEnumerable<CouponUsage> GetAllCouponUsages(string searchTerm, int pageSize, int pageNumber);

    // get all coupon usages for a product
    IEnumerable<CouponUsage> GetCouponUsagesForAProduct(int ProductId);

    // get one coupon usage details
    CouponUsage GetOneCouponUsage(int couponUsageId);
    
    // check if coupon exists
    bool CouponUsageExists(int couponUsageId);
  }
}