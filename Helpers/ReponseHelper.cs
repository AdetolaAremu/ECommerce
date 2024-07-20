using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Helpers
{
  [DataContract]
  public class ResponseHelper
  {
    [DataContract]
    private class SuccesResponseModel<T>
    {
      [DataMember]
      public string Status { get; set; }

      [DataMember]
      public string Message { get; set; }

      [DataMember]
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

    [DataContract]
    private class ErrorResponseModel<T>
    {
      [DataMember]
      public string Status { get; set; }

      [DataMember]
      public string Message { get; set; }

      [DataMember]
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