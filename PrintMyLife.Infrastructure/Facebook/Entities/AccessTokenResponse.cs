using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook.Entities
{
  public class AccessTokenResponse
  {
    [JsonProperty(PropertyName = "access_token")]
    public string Token { get; set; }
    [JsonProperty(PropertyName = "token_type")]
    public string TokenType { get; set; }
    [JsonProperty(PropertyName = "expires_in")]
    public double ExpiresIn { get; set; }
  }
}
