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

    public Order CreateOrder(int userId, decimal amount)
    {
      var order = new Order(){
        UserId = userId,
        DeliveryFees = 500, // let's make this static for now
        Amount = amount
      };

      _applicationDBContext.Add(order);

      SaveTransaction();

      return order;
    }

    public bool CheckOut (int userId, List<CartItem> cartItems, Coupon? coupon)
    {
      decimal totalAmount = cartItems.Sum(ci => ci.Price);

      if (coupon != null) {
        totalAmount = totalAmount * coupon.Percentage / 100;
      }

      var order = CreateOrder(userId, totalAmount);

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

      _applicationDBContext.AddRange(allOrderItems);

      return SaveTransaction();
    }

    public IEnumerable<OrderedProduct> GetAllOrders(int pageNumber, int pageSize)
    {
      return _applicationDBContext.Orders.SelectMany(sc => sc.OrderedProducts).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public IEnumerable<OrderedProduct> GetLoggedInUserOrders(int userId, int pageNumber, int pageSize)
    {
      return _applicationDBContext.Orders.Where(o => o.UserId == userId).SelectMany(
        sc => sc.OrderedProducts
      ).Skip((pageNumber - 1) * pageSize)
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