using Microsoft.Extensions.Options;
using PrintMyLife.Core.Authentication;
using PrintMyLife.Core.Configuration;
using System;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using PrintMyLife.Infrastructure.Facebook.Entities;
using PrintMyLife.Core.Authentication.Entities;
using AutoMapper;
using PrintMyLife.Core.Social.Entities;
using System.Collections.Generic;

namespace PrintMyLife.Infrastructure.Facebook
{
  public class FacebookService : IExternalSocialService
  {
    private readonly FacebookSettings _facebookSettings;

    public FacebookService(IOptions<FacebookSettings> facebookSettings)
    {
      _facebookSettings = facebookSettings.Value;
    }
    public async Task<AccessToken> ExchangeCodeToAnAccessTokenAsync(string code)
    {
      return await GetAsync<AccessTokenResponse, AccessToken>($"https://graph.facebook.com/v2.11/oauth/access_token?client_id={_facebookSettings.AppId}&redirect_uri=http://localhost:53918/api/v1/authentication/facebook&client_secret={_facebookSettings.AppSecret}&code={code}");
    }

    public async Task<AccessToken> GetAppAccessTokenAsync()
    {
      return await GetAsync<AccessTokenResponse, AccessToken>($"https://graph.facebook.com/v2.11/oauth/access_token?client_id={_facebookSettings.AppId}&client_secret={_facebookSettings.AppSecret}&grant_type=client_credentials");
    }

    public async Task<UserProfile> GetUserProfileAsync(string userId, string appToken)
    {
      return await GetAsync<UserProfileResponse, UserProfile>($"https://graph.facebook.com/{userId}/?fields=email,first_name,last_name,name,installed,cover&access_token={appToken}");
    }

    public async Task<UserToken> InspectAccessTokenAsync(string token, string appToken)
    {
      return await GetAsync<DebugTokenResponse, UserToken>($"https://graph.facebook.com/debug_token?input_token={token}&access_token={appToken}");
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string userId, string appToken)
    {
      return await GetAsync<AccountsResponse, IEnumerable<Account>>($"https://graph.facebook.com/me/accounts?access_token={appToken}&fields=cover,name,access_token,category");
    }

    public async Task<IEnumerable<Account>> GetAccountAsync(string accountId, string accountToken)
    {
      return await GetAsync<AccountsResponse, IEnumerable<Account>>($"https://graph.facebook.com/{accountId}/accounts?access_token={accountToken}?fields=cover");
    }



    #region Private
    private async Task<T> GetAsync<T>(string url)
    {
      var client = new RestClient
      {
        BaseUrl = new Uri(url)
      };
      var request = new RestRequest("", Method.GET);
      request.AddHeader("Content-Type", "application/json");

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<T>(response.Content);

      return result;
    }
    private async Task<TResult> GetAsync<T,TResult>(string url)
    {
      var result = await GetAsync<T>(url);
      return Mapper.Map<T, TResult>(result);
    }
    #endregion
  }
}
