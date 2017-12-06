using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Runtime.Session;
using Microsoft.AspNetCore.Authorization;
using PrintMyLife.Web.Common.Exceptions;
using PrintMyLife.Web.Common.Constants;
using PrintMyLife.Core.Authentication;
using PrintMyLife.Core.Social;
using PrintMyLife.Core.Social.Entities;
using PrintMyLife.Core.Common.UnitOWork;
using PrintMyLife.Core.Common.Repositories;

namespace PrintMyLife.Web.Controllers.Pages
{
  [Route("api/v1/[controller]")]
  [Authorize]
  public class PagesController : AuthentifiedBaseController
  {
    private UserManager<User> _userManager;
    private readonly IExternalSocialService _externalSocialService;
    private readonly AccountManager _accountManager;
    private readonly IRepository<Account, string> _accountRespository;

    public PagesController(
      IExternalSocialService externalSocialService,
      IUnitOfWork unitOfWork,
      AccountManager accountManager,
      [FromServices] IAppSession session,
      [FromServices] UserManager<User> userManager
    ) : base(session, userManager)
    {
      _userManager = userManager;
      _externalSocialService = externalSocialService;
      _accountManager = accountManager;
      _accountRespository = unitOfWork.GetRepository<Account, string>();
    }


    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetPages()
    {

      var token = await _userManager.GetAuthenticationTokenAsync(
        CurrentUser,
        TokenConstants.FacebookProvider,
        TokenConstants.FacebookTokenName
      );

      var accounts = await _externalSocialService.GetAccountsAsync(CurrentUser.UserName,token);
      foreach (var account in accounts)
      {
        await _accountManager.AddAccountAsync(account, CurrentUser);
      }
      //TODO Create Dto + Map Accounts to AccountDtos
      return Ok();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetPage([FromQuery]string id)
    {
      var account = await _accountRespository.FirstOrDefaultAsync(a => a.Id == id);

      return new ObjectResult(account);

    }




  }
}
