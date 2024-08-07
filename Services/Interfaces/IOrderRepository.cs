using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IOrderRespository
  {
    // Get All Orders
    IEnumerable<OrderedProduct> GetAllOrders(int pageNumber, int pageSize);

    // Get All Orders by logged in user
    IEnumerable<OrderedProduct> GetLoggedInUserOrders(int userId, int pageNumber, int pageSize);

    // Get one order, alongside orderedItems
    Order GetOneOrder(int orderId);

    // create order
    Order CreateOrder(int userId, decimal amount);

    // check if order exists
    bool CheckIfOrderExists(int orderId);

    bool CheckOut(int userId, List<CartItem> cartItems, Coupon? coupon);

    bool SaveTransaction();
  }
}