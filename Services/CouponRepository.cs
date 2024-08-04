using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Auth;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class CouponRepository : ICouponRepository
  {
    private ApplicationDBContext _applicationDBContext;
    private AuthService _authService;

    public CouponRepository(ApplicationDBContext applicationDBContext, AuthService authService)
    {
      _applicationDBContext = applicationDBContext;
      _authService = authService;
    }

    public IEnumerable<Coupon> GetAllCoupons(int pageSize, int pageNumber)
    {
      return _applicationDBContext.Coupons.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public Coupon GetOneCoupon(int couponId)
    {
      return _applicationDBContext.Coupons.Where(c => c.Id == couponId).First();
    }

    public bool CouponExists(int couponId)
    {
      return _applicationDBContext.Coupons.Any(c => c.Id == couponId);
    }

    public bool CreateCoupon(CreateCouponDTO createCouponDTO)
    {
      var hashedCouponCode = _authService.HashString(createCouponDTO.Code);

      var coupon = new Coupon(){
        Code = hashedCouponCode,
        Percentage = createCouponDTO.Percentage,
        DiscountStarts = createCouponDTO.DiscountStarts,
        DiscountEnds = createCouponDTO.DiscountEnds,
      };

      _applicationDBContext.Add(coupon);

      return SaveTransaction();
    }

    public bool UpdateCoupon(int couponId, CouponDTO couponDTO)
    {
      var coupon = _applicationDBContext.Coupons.Where(c => c.Id == couponId).First();
 
      string hashedCouponCode = null;
      if (couponDTO.Code != null) {
        hashedCouponCode = _authService.HashString(couponDTO.Code);
      } 

      coupon.Code = couponDTO.Code != null ? hashedCouponCode : coupon.Code;
      coupon.DiscountStarts = couponDTO.DiscountStarts;
      coupon.DiscountEnds = couponDTO.DiscountEnds;

      return SaveTransaction();
    }

    public Coupon CheckCouponCode(string code)
    {
      var getCoupon = _applicationDBContext.Coupons.ToList();
      
      bool couponCorrect = false;
      Coupon currentCoupon = new Coupon{};

      foreach (var coupon in getCoupon)
      {
        if (_authService.verifyHashedString(coupon.Code, code)) {
          currentCoupon = coupon;
          couponCorrect = true;
          break;
        }
      }

      if (couponCorrect && CheckCouponExpiry(currentCoupon)) {
        return currentCoupon;
      } else {
        return null;
      }
    }

    public bool CheckCouponExpiry(Coupon coupon)
    {
      var today = DateTime.Now;
      // Console.WriteLine( today > coupon.DiscountEnds ? true : false);
      return today > coupon.DiscountEnds ? true : false;
    }

    public bool DeleteCoupon(Coupon coupon)
    {
      _applicationDBContext.Remove(coupon);

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var coupon = _applicationDBContext.SaveChanges();
      return coupon >= 0 ? true : false;
    }
  }
}