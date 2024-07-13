using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class DiscountRepository : IDiscountRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public DiscountRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Discount> GetAllDiscounts(int pageNumber, int pageSize)
    {
      return _applicationDBContext.Discounts.Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public Discount GetOneDiscount(int discountId)
    {
      return _applicationDBContext.Discounts.Where(d => d.Id == discountId).First();
    }

    public bool CreateDiscount(CreateDiscountDTO createDiscountDTO)
    {
      var discount = new Discount(){
        ProductId = createDiscountDTO.ProductId,
        Percentage = createDiscountDTO.Percentage,
        DiscountStarts = createDiscountDTO.DiscountStarts,
        DiscountEnds = createDiscountDTO.DiscountEnds
      };

      _applicationDBContext.Add(discount);

      return SaveTransaction();
    }

    public bool UpdateDiscount(DiscountDTO discountDTO, int discountId)
    {
      var discount = _applicationDBContext.Discounts.Where(d => d.Id == discountId).First();

      discount.Percentage = discountDTO.Percentage;
      discount.DiscountStarts = discountDTO.DiscountStarts;
      discount.DiscountEnds = discountDTO.DiscountEnds;

      return SaveTransaction();
    }

    public bool DiscountExists(int discountId)
    {
      return _applicationDBContext.Coupons.Any(d => d.Id == discountId);
    }

    public bool DeleteDiscount(Discount discount)
    {
      _applicationDBContext.Remove(discount);

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var discount = _applicationDBContext.SaveChanges();

      return discount >= 0 ? true : false;
    }
  }
}