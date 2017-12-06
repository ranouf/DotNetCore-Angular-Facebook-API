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
    private User _currentUser;
    public User CurrentUser
    {
      get
      {
        if (_currentUser == null && Session.UserId.HasValue)
        {
          _currentUser = _userManager.FindByIdAsync(Session.UserId.Value).Result;
        }
        return _currentUser;
      }
    }

    public IAppSession Session { get; set; }

    public AuthentifiedBaseController(
    [FromServices] IAppSession session,
    [FromServices] UserManager<User> userManager
    ) : base()
    {
      _userManager = userManager;
      Session = session;
    }
  }
}
