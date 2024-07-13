using ecommerce.DataStore;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class OrderRepository : IOrderRespository
  {
    private ApplicationDBContext _applicationDBContext;

    public OrderRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Order> GetAllOrders(int pageNumber, int pageSize)
    {
      return _applicationDBContext.Orders.Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public IEnumerable<Order> GetLoggedInUserOrders(int userId, int pageNumber, int pageSize)
    {
      return _applicationDBContext.Orders.Where(o => o.UserId == userId).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public bool CheckIfOrderExists(int orderId)
    {
      return _applicationDBContext.Orders.Any(o => o.Id == orderId);
    }

    public Order GetOneOrder(int orderId)
    {
      return _applicationDBContext.Orders.Where(o => o.Id == orderId).First();
    }
  }
}