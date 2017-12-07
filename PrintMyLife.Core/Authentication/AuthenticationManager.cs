using Microsoft.AspNetCore.Identity;
using PrintMyLife.Common.Constants;
using PrintMyLife.Common.Exceptions;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Common.Dependencies;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System;

namespace PrintMyLife.Core.Authentication
{
  public class AuthenticationManager : IManager
  {
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IExternalSocialService _externalSocialService;

    public AuthenticationManager(
      UserManager<User> userManager,
      SignInManager<User> signInManager,
      IExternalSocialService externalSocialService
    )
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _externalSocialService = externalSocialService;
    }

    public async Task<User> LoginWithFacebookAsync(string code)
    {
      var accessToken = await _externalSocialService.ExchangeCodeToAnAccessTokenAsync(code);
      var appAccessToken = await _externalSocialService.GetAppAccessTokenAsync();
      var userToken = await _externalSocialService.GetUserTokenAsync(accessToken.Token, appAccessToken.Token);
      var signInResult = await _signInManager.ExternalLoginSignInAsync(TokenConstants.FacebookProvider, userToken.UserId, isPersistent: false);
      if (!signInResult.Succeeded)
      {
        await RegisterWithFacebook();
      }
      var user = await _userManager.FindByLoginAsync(TokenConstants.FacebookProvider, userToken.UserId);
      var identityResult = await _userManager.SetAuthenticationTokenAsync(user, TokenConstants.FacebookProvider, TokenConstants.FacebookTokenName, accessToken.Token);
      if (!identityResult.Succeeded)
      {
        throw new ApiException(identityResult.Errors.First().Description);
      }
      return user;

      async Task RegisterWithFacebook()
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
          throw new ApiException("ExternalLoginSignInAsync failed");
        }
      }
    }
  }
}
