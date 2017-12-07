using System;
using System.Net;

namespace PrintMyLife.Common.Exceptions
{
  public class ApiException : Exception
  {
    public int StatusCode { get; set; }

    public ApiException(string message,
                        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            base(message)
    {
      StatusCode = (int)statusCode;
    }

    public ApiException(Exception ex,
                        HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            base(ex.Message)
    {
      StatusCode = (int)statusCode;
    }
  }
}
