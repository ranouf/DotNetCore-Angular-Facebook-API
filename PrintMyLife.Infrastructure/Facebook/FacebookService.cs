using Microsoft.Extensions.Options;
using PrintMyLife.Core.Authentication;
using PrintMyLife.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using PrintMyLife.Infrastructure.Facebook.entities;
using PrintMyLife.Core.Authentication.Entities;

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
      var client = new RestClient
      {
        BaseUrl = new Uri($"https://graph.facebook.com/v2.11/oauth/access_token?client_id={_facebookSettings.AppId}&redirect_uri=http://localhost:53918/api/v1/authentication/facebook&client_secret={_facebookSettings.AppSecret}&code={code}")
      };
      var request = new RestRequest("", Method.GET);
      request.AddHeader("Content-Type", "application/json");

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<AccessTokenReponse>(response.Content);

      return new AccessToken()
      {
        Token = result.Token,
        ExpiresIn = result.ExpiresIn,
        TokenType = result.TokenType
      };
    }

    public async Task<string> GetAppAccessTokenAsync()
    {
      var client = new RestClient
      {
        BaseUrl = new Uri($"https://graph.facebook.com/v2.11/oauth/access_token?client_id={_facebookSettings.AppId}&client_secret={_facebookSettings.AppSecret}&grant_type=client_credentials")
      };
      var request = new RestRequest("", Method.GET);
      request.AddHeader("Content-Type", "application/json");

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<AccessTokenReponse>(response.Content);

      return result.Token;
    }

    public async Task<UserProfile> GetUserProfileAsync(string userId, string appToken)
    {
      var client = new RestClient
      {
        BaseUrl = new Uri($"https://graph.facebook.com/{userId}/?fields=email,first_name,last_name,name,installed,cover&access_token={appToken}")
      };
      var request = new RestRequest("", Method.GET);
      request.AddHeader("Content-Type", "application/json");

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<UserProfileResponse>(response.Content);

      return new UserProfile()
      {
        Id = result.Id,
        FirstName = result.FirstName,
        LastName = result.LastName,
        FullName = result.Name,
        Email = result.Email,
        CoverUrl = result.Cover.Source
      };
    }

    public async Task<UserToken> InspectAccessTokenAsync(string token, string appToken)
    {
      var client = new RestClient
      {
        BaseUrl = new Uri($"https://graph.facebook.com/debug_token?input_token={token}&access_token={appToken}")
      };
      var request = new RestRequest("", Method.GET);
      request.AddHeader("Content-Type", "application/json");

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<DebugTokenResponse>(response.Content);

      return new UserToken()
      {
        AppId = result.Data.AppId,
        Application = result.Data.Application,
        ExpiresAt = result.Data.ExpiresAt,
        IssuedAt = result.Data.IssuedAt,
        IsValid = result.Data.IsValid,
        Scopes = result.Data.Scopes,
        Type = result.Data.Type,
        UserId = result.Data.UserId
      };
    }
  }
}
