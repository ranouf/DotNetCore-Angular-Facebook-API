using PrintMyLife.Core.Authentication.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrintMyLife.Core.Authentication
{
    public interface IExternalSocialService
  {
    Task<AccessToken> ExchangeCodeToAnAccessTokenAsync(string token);

    Task<string> GetAppAccessTokenAsync();

    Task<UserToken> InspectAccessTokenAsync(string token, string appToken);

    Task<UserProfile> GetUserProfileAsync(string userId, string appToken);
  }
}
