using Microsoft.Extensions.Options;
using PrintMyLife.Core.Authentication;
using PrintMyLife.Common.Configuration;
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
      return await GetAsync<AccessTokenResponse, AccessToken>($"oauth/access_token", new Parameter[] {
        new QueryStringParameter("client_id", _facebookSettings.AppId ),
        new QueryStringParameter("client_secret", _facebookSettings.AppSecret ),
        new QueryStringParameter("redirect_uri", "http://localhost:53918/api/v1/authentication/facebook" ), // TODO: Get From AppSettings (or somewhere else)
        new QueryStringParameter("code", code ),
      });
    }

    public async Task<AccessToken> GetAppAccessTokenAsync()
    {
      return await GetAsync<AccessTokenResponse, AccessToken>($"/oauth/access_token", new Parameter[] {
        new QueryStringParameter("client_id",_facebookSettings.AppId ),
        new QueryStringParameter("client_secret",_facebookSettings.AppSecret ),
        new QueryStringParameter("grant_type","client_credentials" )
      });
    }

    public async Task<UserProfile> GetUserProfileAsync(string userId, string appToken)
    {
      return await GetAsync<UserProfileResponse, UserProfile>("/{userId}", new Parameter[] {
        new UrlSegmentParameter("userId",userId),
        new QueryStringParameter("fields","email,first_name,last_name,name,installed,cover" ),
        new QueryStringParameter("access_token",appToken ),
      });
    }

    public async Task<UserToken> GetUserTokenAsync(string token, string appToken)
    {
      return await GetAsync<DebugTokenResponse, UserToken>($"/debug_token", new Parameter[] {
        new QueryStringParameter("input_token",token ),
        new QueryStringParameter("access_token",appToken ),
      });
    }

    public async Task<IEnumerable<Account>> GetAccountsAsync(string userId, string appToken)
    {
      return await GetAsync<AccountsResponse, IEnumerable<Account>>($"/me/accounts", new Parameter[] {
        new QueryStringParameter("access_token",appToken ),
        new QueryStringParameter("fields","cover,name,access_token,category" ),
      });
    }

    public async Task<object> GetAccountAsync(string accountId, string accountToken)
    {
      return await GetAsync<AccountsResponse>("/{accountId}/feed", new Parameter[] {
        new UrlSegmentParameter("accountId",accountId),
        new QueryStringParameter("access_token",accountToken ),
        new QueryStringParameter("fields","fields=id,created_time,message,story,icon,link,object_id,parent_id,place,properties,status_type,type,attachments{title,description,url,media,type,target,subattachments}" ),
      });
    }



    #region Private
    private async Task<T> GetAsync<T>(string url, Parameter[] parameters = null)
    {
      var client = new RestClient
      {
        BaseUrl = new Uri("https://graph.facebook.com/v2.11/")
      };
      var request = new RestRequest(url, Method.GET);
      request.AddHeader("Content-Type", "application/json");
      if (parameters != null)
      {
        foreach (var parameter in parameters)
        {
          request.AddParameter(parameter);
        }
      }

      TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();

      RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => taskCompletion.SetResult(r));

      RestResponse response = (RestResponse)(await taskCompletion.Task);

      var result = JsonConvert.DeserializeObject<T>(response.Content);

      return result;
    }
    private async Task<TResult> GetAsync<T, TResult>(string url, Parameter[] parameters = null)
    {
      var result = await GetAsync<T>(url, parameters);
      return Mapper.Map<T, TResult>(result);
    }
    #endregion
  }

  internal class QueryStringParameter : Parameter
  {
    public QueryStringParameter(string name, string value)
    {
      Name = name;
      Value = value;
      Type = ParameterType.QueryString;
    }
  }

  internal class UrlSegmentParameter : Parameter
  {
    public UrlSegmentParameter(string name, string value)
    {
      Name = name;
      Value = value;
      Type = ParameterType.UrlSegment;
    }
  }
}
