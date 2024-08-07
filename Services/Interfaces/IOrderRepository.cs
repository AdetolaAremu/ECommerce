using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IOrderRespository
  {
    // Get All Orders
    List<OrderDTO> GetAllOrders(int pageNumber, int pageSize);

    // Get All Orders by logged in user
    List<OrderDTO> GetLoggedInUserOrders(int userId, int pageNumber, int pageSize);

    // Get one order, alongside orderedItems
    IEnumerable<Order> GetOneOrder(int orderId);

    // create order
    Order CreateOrder(int userId, decimal amount);

    // check if order exists
    bool CheckIfOrderExists(int orderId);

    bool CheckOut(int userId, List<CartItem> cartItems, Coupon? coupon);

    bool SaveTransaction();
  }
}