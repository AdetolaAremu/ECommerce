using ecommerce.Helpers;
using ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
  [Authorize]
  [ApiController]
  [Route("/api/user")]
  public class UserController : ControllerBase
  {
    private IUserRepository _userRepository;
    private ResponseHelper _responseHelper;

    public UserController(IUserRepository userRepository, ResponseHelper responseHelper)
    {
      _userRepository = userRepository;
      _responseHelper = responseHelper;
    }

    // get Logged in user
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLoggedInUser()
    {
      var getUser = _userRepository.GetLoggedInUser();

      if (getUser == null) return _responseHelper.ErrorResponseHelper<string>("Unauthorized", null, 401);

      return _responseHelper.SuccessResponseHelper("User retrieved successfully", getUser);
    }

    // get all users

    // get one user

    // change user login status
  } 
}