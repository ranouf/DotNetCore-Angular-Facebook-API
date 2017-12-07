using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Runtime.Session;
using Microsoft.AspNetCore.Authorization;
using PrintMyLife.Core.Social;
using PrintMyLife.Core.Social.Entities;
using PrintMyLife.Common.UnitOWork;
using PrintMyLife.Common.Repositories;

namespace PrintMyLife.Web.Controllers.Pages
{
  [Route("api/v1/[controller]")]
  [Authorize]
  public class PagesController : AuthentifiedBaseController
  {
    private readonly SocialManager _socialManager;
    private readonly IRepository<Account, string> _accountRepository;

    public PagesController(
      SocialManager socialManager,
      IUnitOfWork unitOfWork,
      [FromServices] IAppSession session,
      [FromServices] UserManager<User> userManager
    ) : base(session, userManager)
    {
      _socialManager = socialManager;
      _accountRepository = unitOfWork.GetRepository<Account, string>();
    }


    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetPages()
    {
      await _socialManager.LoadAccountsAsync(CurrentUser);
      //TODO Create Dto + Map Accounts to AccountDtos
      return Ok();
    }


    [HttpGet]
    [ProducesResponseType(200)]
    [Route("{id}")]
    public async Task<IActionResult> GetPage([FromRoute]string id)
    {
      var account = await _accountRepository.FirstOrDefaultAsync(id);
      if (account == null)
      {
        return NotFound();
      }

      await _socialManager.LoadAccountAsync(account);
      //TODO Create Dto + Map Accounts to AccountDtos
      return Ok();
    }

    //[HttpGet("{id}")]
    //[ProducesResponseType(200)]
    //public async Task<IActionResult> GetPage([FromQuery]string id)
    //{
    //  var account = await _accountRespository.FirstOrDefaultAsync(a => a.Id == id);

    //  return new ObjectResult(account);

    //}




  }
}
