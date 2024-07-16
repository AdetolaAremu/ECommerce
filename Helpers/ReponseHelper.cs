using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Helpers
{
  public class ResponseHelper
  {
    private class SuccesResponseModel<T>
    {
      public string Status { get; set; }
      public string Message { get; set; }
      public T Data { get; set; }
    }

    public IActionResult SuccessResponseHelper<T>(string message, T data, int statusCode=200)
    {
      var successResponse = new SuccesResponseModel<T>
      {
        Status = "success",
        Message = message,
        Data = data
      };

      return new ObjectResult(successResponse) { StatusCode = statusCode };
    }

    private class ErrorResponseModel<T>
    {
      public string Status { get; set; }
      public string Message { get; set; }
      public T? Error { get; set; }
    }

    public IActionResult ErrorResponseHelper<T>(string message, T? error = default, int statusCode = 400)
    {
      var errorResponse = new ErrorResponseModel<T>
      {
        Status = "fail",
        Message = message,
        Error = error
      };

      return new ObjectResult(errorResponse) { StatusCode = statusCode };
    }
  }
}