using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services.Auth;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class UserRepository : IUserRepository
  {
    private ApplicationDBContext _applicationDBContext;
    private AuthService _authService;

    public UserRepository(ApplicationDBContext applicationDBContext, AuthService authService)
    {
      _applicationDBContext = applicationDBContext;
      _authService = authService;
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

    public bool Register(CreateUserDTO createUserDTO)
    {
      var gethashedPassword = _authService.HashPassword(createUserDTO.Password);

      var user = new User(){
        FirstName = createUserDTO.FirstName,
        LastName = createUserDTO.LastName,
        Email = createUserDTO.Email,
        Password = gethashedPassword,
        // LoginStatus = createUserDTO.LoginStatus
      };

      _applicationDBContext.Add(user);

      return SaveTransaction();
    }

    public string LoginUser(UserLoginDTO userLoginDTO)
    {
      var user = _applicationDBContext.Users.Where(u => u.Email.ToLower() == userLoginDTO.Email.ToLower()).First();
      var gethashedPassword = _authService.HashPassword(userLoginDTO.Password);

      if (user.Password != gethashedPassword)
      {
        return null;
      }

      return _authService.GenerateToken(user.Id, user.Email);
    }

    public bool CheckIfEmailExists(string email)
    {
      return _applicationDBContext.Users.Any(u => u.Email.ToLower() == email.ToLower());
    }

    public bool SaveTransaction()
    {
      var save = _applicationDBContext.SaveChanges();
      return save >= 0 ? true : false;
    }
  }
}