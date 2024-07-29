using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class CouponRepository : ICouponRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public CouponRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Coupon> GetAllCoupons(int pageSize, int pageNumber)
    {
      return _applicationDBContext.Coupons.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public Coupon GetOneCoupon(int couponId)
    {
      return _applicationDBContext.Coupons.Where(c => c.Id == couponId).First();
    }

    public IEnumerable<Coupon> GetAllCouponsPerProduct(int ProductId, int pageSize, int pageNumber)
    {
      var query = _applicationDBContext.Coupons.AsQueryable();

      query = query.Where(c => c.Product.Id == ProductId);

      return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool CouponExists(int couponId)
    {
      return _applicationDBContext.Coupons.Any(c => c.Id == couponId);
    }

    public bool CreateCoupon(CreateCouponDTO createCouponDTO)
    {
      var coupon = new Coupon(){
        Code = createCouponDTO.Code,
        ProductId = createCouponDTO.ProductId,
        DiscountStarts = createCouponDTO.DiscountStarts,
        DiscountEnds = createCouponDTO.DiscountEnds,
      };

      _applicationDBContext.Add(coupon);

      return SaveTransaction();
    }

    public bool UpdateCoupon(int couponId, CouponDTO couponDTO)
    {
      var coupon = _applicationDBContext.Coupons.Where(c => c.Id == couponId).First();

      coupon.Code = couponDTO.Code;
      coupon.DiscountStarts = couponDTO.DiscountStarts;
      coupon.DiscountEnds = couponDTO.DiscountEnds;

      return SaveTransaction();
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