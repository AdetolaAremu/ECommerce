using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IOrderRespository
  {
    // Get All Orders
    IEnumerable<Order> GetAllOrders(int pageNumber, int pageSize);

    // Get All Orders by logged in user
    IEnumerable<Order> GetLoggedInUserOrders(int userId, int pageNumber, int pageSize);

    // Get one order, alongside orderedItems
    Order GetOneOrder(int orderId);

    // check if order exists
    bool CheckIfOrderExists(int orderId);

    bool CheckOut(int userId, List<CartItem> cartItems);

    bool SaveTransaction();
  }
}