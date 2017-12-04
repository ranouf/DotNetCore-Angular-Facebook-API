using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintMyLife.Web.Controllers.Authentication.Dto;
using Microsoft.AspNetCore.Identity;
using PrintMyLife.Core.Authentication.Entities;
using AutoMapper;
using Microsoft.Extensions.Options;
using PrintMyLife.Core.Configuration;
using PrintMyLife.Web.Helpers;
using PrintMyLife.Web.Controllers.Authorization.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using PrintMyLife.Core.Runtime.Session;

namespace PrintMyLife.Web.Controllers.Authorization
{
  [Route("api/v1/[controller]")]
  public class AuthenticationController : AuthentifiedBaseController
  {
    private UserManager<User> _userManager;
    private AuthenticationSettings _authSettings;

    public AuthenticationController(
      UserManager<User> userManager,
      IOptions<AuthenticationSettings> authSettings,
      IAppSession session
    ) : base(session, userManager)
    {
      _userManager = userManager;
      _authSettings = authSettings.Value;
    }

    // POST api/values
    [HttpPost]
    [ProducesResponseType(typeof(UserAuthenticationDto), 200)]
    public async Task<IActionResult> Login([FromBody]CredentialsDto dto)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      // is the user authorized
      var user = await _userManager.FindByEmailAsync(dto.Email);
      if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
      {
        return Unauthorized();
      }

      var token = TokenHelper.GenerateToken(_authSettings, user);

      return Ok(Mapper.Map<User, UserAuthenticationDto>(
        user,
        opt => opt.AfterMap((src, dest) => AfterMapConversion(dest)))
      );

      UserAuthenticationDto AfterMapConversion(UserAuthenticationDto dest){
        dest.Token = token.TokenId;
        dest.ExpirationDate = token.ExpirationDate;
        return dest;
      }
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(object), 200)]
    public IActionResult Test()
    {
      return Ok();
    }
  }
}
