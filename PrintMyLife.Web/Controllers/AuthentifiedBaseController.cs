using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Common.Extensions;
using PrintMyLife.Core.Runtime.Session;

namespace PrintMyLife.Web.Controllers
{
  public class AuthentifiedBaseController : BaseController
  {
    private UserManager<User> _userManager;
    private IAppSession _session;
    private User _currentUser;
    public User CurrentUser
    {
      get
      {
        if (_currentUser == null && _session.UserId.HasValue)
        {
          _currentUser = _userManager.FindByIdAsync(_session.UserId.Value).Result;
        }
        return _currentUser;
      }
    }

    public AuthentifiedBaseController(
    [FromServices] IAppSession session,
    [FromServices]UserManager<User> userManager
    ) : base()
    {
      _userManager = userManager;
      _session = session;
    }
  }
}
