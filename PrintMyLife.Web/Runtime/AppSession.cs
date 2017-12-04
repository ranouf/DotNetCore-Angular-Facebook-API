using PrintMyLife.Core.Runtime.Session;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace PrintMyLife.Web.Runtime
{
  public class AppSession : IAppSession
  {
    private IHttpContextAccessor _context;

    public Guid? UserId
    {
      get
      {
        Guid result = Guid.Empty;
        var userId = _context.HttpContext.User.Claims
          .Where(c => c.Type == ClaimTypes.Sid)
          .Select(c => c.Value)
          .FirstOrDefault();

        if (Guid.TryParse(userId, out result))
        {
          return result;
        }
        return null;
      }
    }

    public AppSession(IHttpContextAccessor context)
    {
      _context = context;
    }
  }
}
