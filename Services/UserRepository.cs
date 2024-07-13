using ecommerce.DataStore;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class UserRepository : IUserRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public UserRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<User> GeAllUsers(int pageSize, int pageNumber)
    {
      return _applicationDBContext.Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool CheckIfUserIsAnAdmin(int userId)
    {
      var user = _applicationDBContext.Users.Where(u => u.Id == userId).First();

      return user.UserType == Enums.EnumUserType.Admin ? true : false;
    }

    public bool CheckIfUserExist(int userId)
    {
      return _applicationDBContext.Users.Any(u => u.Id == userId);
    }
  }
}