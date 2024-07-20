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
    private ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ResponseHelper responseHelper, ILogger<UserController> logger)
    {
      _userRepository = userRepository;
      _responseHelper = responseHelper;
      _logger = logger;
    }

    // get Logged in user
    [HttpGet("/logged-in-user")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetLoggedInUser()
    {
      var getUser = _userRepository.GetLoggedInUser();

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      if (getUser == null) return _responseHelper.ErrorResponseHelper<string>("Unauthorized", null, 401);

      return _responseHelper.SuccessResponseHelper("User retrieved successfully", getUser);
    }

    // get all users
    [HttpGet("/all-users")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAllUsers([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
      if (!_userRepository.CheckIfUserIsAnAdmin()) return _responseHelper.ErrorResponseHelper<string>("User is not an admin", null, 401);

      if (pageSize <= 0 || pageNumber <= 0) return _responseHelper.ErrorResponseHelper<string>("Query params is not correct");

      var allUsers = _userRepository.GeAllUsers(pageSize, pageNumber);

      if (!ModelState.IsValid) return _responseHelper.ErrorResponseHelper<string>($"Bad request {ModelState}");

      return _responseHelper.SuccessResponseHelper("Users retrieved successfully", allUsers);
    }

    // get one user
    // public IActionResult GetOneUser() {}

    // upload image

    // change user login status
  } 
}