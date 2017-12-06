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
using PrintMyLife.Web.Common.Constants;
using System.Net;

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
    [Route(TokenConstants.FacebookProvider)]
    public async Task<IActionResult> ExternalFacebookLogin([FromQuery]string code)
    {
      var accessToken = await _externalSocialService.ExchangeCodeToAnAccessTokenAsync(code);
      var appAccessToken = await _externalSocialService.GetAppAccessTokenAsync();
      var userToken = await _externalSocialService.InspectAccessTokenAsync(accessToken.Token, appAccessToken.ToString());
      var signInResult = await _signInManager.ExternalLoginSignInAsync(TokenConstants.FacebookProvider, userToken.UserId, isPersistent: false);
      IdentityResult identityResult = null;
      if (!signInResult.Succeeded)
      {
        var userProfile = await _externalSocialService.GetUserProfileAsync(userToken.UserId, appAccessToken.Token);
        var newUser = new User(
          userProfile.Id,
          userProfile.Email,
          userProfile.FirstName,
          userProfile.LastName
        );
        identityResult = await _userManager.CreateAsync(newUser);
        if (!identityResult.Succeeded)
        {
          throw new ApiException(identityResult.Errors.First().Description);
        }
        var userInfo = new UserLoginInfo(TokenConstants.FacebookProvider, userToken.UserId, userProfile.FullName);
        identityResult = await _userManager.AddLoginAsync(newUser, userInfo);
        if (!identityResult.Succeeded)
        {
          throw new ApiException(identityResult.Errors.First().Description);
        }
        await _signInManager.SignInAsync(newUser, false);
        signInResult = await _signInManager.ExternalLoginSignInAsync(TokenConstants.FacebookProvider, userToken.UserId, isPersistent: false);
        if (!signInResult.Succeeded)
        {
          if (signInResult.IsNotAllowed)
          {
            return Unauthorized();
          }else if (signInResult.IsLockedOut)
          {
            throw new ApiException("Your account is locked.", HttpStatusCode.Unauthorized);
          }
        }
      }
      var user = await _userManager.FindByLoginAsync(TokenConstants.FacebookProvider, userToken.UserId);

      identityResult = await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.FacebookProvider, "facebook_token", accessToken.Token);
      if (!identityResult.Succeeded)
      {
        throw new ApiException(identityResult.Errors.First().Description);
      }

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
      var user = this.CurrentUser;
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
