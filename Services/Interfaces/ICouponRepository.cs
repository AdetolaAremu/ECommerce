using System.ComponentModel.DataAnnotations;
using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ICouponRepository
  {
    IEnumerable<Coupon> GetAllCoupons(int pageSize, int pageNumber);

    Coupon GetOneCoupon(int couponId);

    Coupon CheckCouponCode(string code);

    bool CheckCouponExpiry(Coupon coupon);

    bool CouponExists(int couponId);
    
    bool CreateCoupon(CreateCouponDTO createCouponDTO);

    bool UpdateCoupon(int couponId, CouponDTO couponDTO);

    bool DeleteCoupon(Coupon coupon);

    bool SaveTransaction();
  }
}