using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrintMyLife.Core.Runtime.Session;
using PrintMyLife.Web.Common.Exceptions;

namespace PrintMyLife.Web.Common.Filters
{
  public class ApiExceptionFilter : ExceptionFilterAttribute
  {
    private ILogger<ApiExceptionFilter> _Logger;
    private IAppSession _session;

    public ApiExceptionFilter(
        ILogger<ApiExceptionFilter> logger,
        IAppSession session)
    {
      _Logger = logger;
      _session = session;
    }

    public override void OnException(ExceptionContext context)
    {
      ApiError apiError = null;
      if (context.Exception is ApiException)
      {
        // handle explicit 'known' API errors
        var ex = context.Exception as ApiException;
        context.Exception = null;
        apiError = new ApiError(ex.Message);

        context.HttpContext.Response.StatusCode = ex.StatusCode;
      }
      else if (context.Exception is UnauthorizedAccessException)
      {
        apiError = new ApiError("Unauthorized Access");
        context.HttpContext.Response.StatusCode = 401;

        // handle logging here
      }
      else
      {
        // Unhandled errors
#if !DEBUG
                var msg = "An unhandled error occurred.";                
                string stack = null;
#else
        var msg = context.Exception.GetBaseException().Message;
        string stack = context.Exception.StackTrace;
#endif

        apiError = new ApiError(msg);
        apiError.detail = stack;

        context.HttpContext.Response.StatusCode = 500;

        // handle logging here
      }

      // always return a JSON result
      context.Result = new JsonResult(apiError);

      base.OnException(context);
    }
  }

  public class ApiError
  {
    public string message { get; set; }
    public bool isError { get; set; }
    public string detail { get; set; }

    public ApiError(string message)
    {
      this.message = message;
      isError = true;
    }
  }
}
