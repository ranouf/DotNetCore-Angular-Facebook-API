using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Social.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrintMyLife.Core.Authentication
{
    public interface IExternalSocialService
  {
    Task<AccessToken> ExchangeCodeToAnAccessTokenAsync(string token);

    Task<AccessToken> GetAppAccessTokenAsync();

    Task<UserToken> GetUserTokenAsync(string token, string appToken);

    Task<UserProfile> GetUserProfileAsync(string userId, string appToken);

    Task<IEnumerable<Account>> GetAccountsAsync(string userId, string appToken);

    Task<object> GetAccountAsync(string accountId, string accountToken);
  }
}
