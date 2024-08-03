using ecommerce.DataStore;
using ecommerce.Models;
using ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Services
{
  public class OrderRepository : IOrderRespository
  {
    private ApplicationDBContext _applicationDBContext;

    public OrderRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public bool CheckOut (int userId, List<CartItem> cartItems)
    {
      decimal totalAmount = cartItems.Sum(ci => ci.Price);

      var order = new Order(){
        UserId = userId,
        DeliveryFees = 500, // let's make this static for now
        Amount = totalAmount
      };

      SaveTransaction();

      var allOrderItems = new List<OrderedProduct>();

      foreach (var item in cartItems)
      {
        var orderItems = new OrderedProduct(){
          OrderId = order.Id,
          Price = item.Price,
          productId = item.ProductId,
          Quantity = item.Quantity,
          Title = item.Title
        };

        allOrderItems.Add(orderItems);
      }

      return SaveTransaction();
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

    public bool SaveTransaction()
    {
      var save = _applicationDBContext.SaveChanges();

      return save >= 0 ? true : false;
    }
  }
}