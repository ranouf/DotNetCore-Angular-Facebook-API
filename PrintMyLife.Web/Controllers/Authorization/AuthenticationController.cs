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
using PrintMyLife.Core.Authentication;
using PrintMyLife.Web.Common.Exceptions;
using System.Linq;

namespace PrintMyLife.Web.Controllers.Authorization
{
  [Route("api/v1/[controller]")]
  public class AuthenticationController : AuthentifiedBaseController
  {
    private UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private AuthenticationSettings _authSettings;
    private readonly IExternalSocialService _externalSocialService;

    public AuthenticationController(
      UserManager<User> userManager,
      SignInManager<User> signInManager,
      IOptions<AuthenticationSettings> authSettings,
      IExternalSocialService externalSocialService,
      IAppSession session
    ) : base(session, userManager)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _authSettings = authSettings.Value;
      _externalSocialService = externalSocialService;
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
    [Route("facebook")]
    public async Task<IActionResult> ExternalFacebookLogin([FromQuery]string code)
    {
      var accessToken = await _externalSocialService.ExchangeCodeToAnAccessTokenAsync(code);
      var appToken = await _externalSocialService.GetAppAccessTokenAsync();
      var userToken = await _externalSocialService.InspectAccessTokenAsync(accessToken.Token, appToken.ToString());
      var signInResult = await _signInManager.ExternalLoginSignInAsync("facebook", userToken.UserId, isPersistent: false);
      if (!signInResult.Succeeded)
      {
        var userProfile = await _externalSocialService.GetUserProfileAsync(userToken.UserId, appToken);
        var newUser = new User(
          userProfile.Id,
          userProfile.Email,
          userProfile.FirstName,
          userProfile.LastName
        );
        var identityResult = await _userManager.CreateAsync(newUser);
        if (!identityResult.Succeeded)
        {
          throw new ApiException(identityResult.Errors.First().Description);
        }
        var userInfo = new UserLoginInfo("facebook", userToken.UserId, userProfile.FullName);
        identityResult = await _userManager.AddLoginAsync(newUser, userInfo);
        if (!identityResult.Succeeded)
        {
          throw new ApiException(identityResult.Errors.First().Description);
        }
        await _signInManager.SignInAsync(newUser, false);
        signInResult = await _signInManager.ExternalLoginSignInAsync("facebook", userToken.UserId, isPersistent: false);
        if (!signInResult.Succeeded)
        {
          //TODO Manage Errors
        }
      }
      var user = await _userManager.FindByLoginAsync("facebook", userToken.UserId);

      return Ok(Mapper.Map<User, UserAuthenticationDto>(
        user,
        opt => opt.AfterMap((src, dest) => AfterMapConversion(src, dest)))
      );
    }


    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(object), 200)]
    public IActionResult Test()
    {
      return Ok();
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
