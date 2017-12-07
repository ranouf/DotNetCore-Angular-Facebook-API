using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintMyLife.Web.Controllers.Authentication.Dto;
using Microsoft.AspNetCore.Identity;
using PrintMyLife.Core.Authentication.Entities;
using AutoMapper;
using Microsoft.Extensions.Options;
using PrintMyLife.Web.Helpers;
using PrintMyLife.Web.Controllers.Authorization.Dto;
using PrintMyLife.Core.Runtime.Session;
using PrintMyLife.Core.Authentication;
using PrintMyLife.Common.Configuration;
using PrintMyLife.Common.Constants;

namespace PrintMyLife.Web.Controllers.Authorization
{
  [Route("api/v1/[controller]")]
  public class AuthenticationController : AuthentifiedBaseController
  {
    private readonly AuthenticationManager _authenticationManager;
    private readonly AuthenticationSettings _authSettings;
    private readonly UserManager<User> _userManager;

    public AuthenticationController(
      AuthenticationManager authenticationManager,
      IOptions<AuthenticationSettings> authSettings,
      UserManager<User> userManager,
      IAppSession session
    ) : base(session, userManager)
    {
      _authenticationManager = authenticationManager;
      _authSettings = authSettings.Value;
      _userManager = userManager;
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

      return Ok(Mapper.Map<User, UserAuthenticationDto>(
        user,
        opt => opt.AfterMap((src, dest) => AfterMapConversion(src, dest)))
      );
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [Route(TokenConstants.FacebookProvider)]
    public async Task<IActionResult> ExternalFacebookLogin([FromQuery]string code)
    {
      var user = await _authenticationManager.LoginWithFacebookAsync(code);

      return Ok(Mapper.Map<User, UserAuthenticationDto>(
        user,
        opt => opt.AfterMap((src, dest) => AfterMapConversion(src, dest)))
      );
    }


    #region Private
    private UserAuthenticationDto AfterMapConversion(User src, UserAuthenticationDto dest)
    {
      var token = TokenHelper.GenerateToken(_authSettings, src);
      dest.Token = token.TokenId;
      dest.ExpirationDate = token.ExpirationDate;
      return dest;
    }
    #endregion

  }
}
