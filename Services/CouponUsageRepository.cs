using ecommerce.DataStore;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class CouponUsageRepository : ICouponUsageRepository
  {
    private ApplicationDBContext _applicationDbContext;

    public CouponUsageRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDbContext = applicationDBContext;
    }

    public IEnumerable<CouponUsage> GetAllCouponUsages(string searchTerm, int pageSize, int pageNumber)
    {
      var query = _applicationDbContext.CouponUsages.AsQueryable();

      if (!string.IsNullOrEmpty(searchTerm))
      {
        query = query.Where(cu => cu.User.FirstName.Contains(searchTerm) || cu.User.LastName.Contains(searchTerm)
         || cu.Product.Title.Contains(searchTerm) || cu.Product.Manufacturer.Contains(searchTerm));
      }

      return query.Skip((pageNumber -1) * pageSize).Take(pageSize).ToList();
    }

    public IEnumerable<CouponUsage> GetCouponUsagesForAProduct(int ProductId)
    {
      return _applicationDbContext.CouponUsages.Where(cu => cu.ProductId == ProductId).ToList();
    }

    public CouponUsage GetOneCouponUsage(int couponUsageId)
    {
      return _applicationDbContext.CouponUsages.Where(cu => cu.Id == couponUsageId).First();
    }

    public bool CouponUsageExists(int couponUsageId)
    {
      return _applicationDbContext.CouponUsages.Any(cu => cu.Id == couponUsageId);
    }
  }
}