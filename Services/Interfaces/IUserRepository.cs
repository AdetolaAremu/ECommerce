using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IUserRepository
  {
    // get all users
    IEnumerable<User> GeAllUsers();

    // get logged In User
    User GetLoggedInUser();
  }
}