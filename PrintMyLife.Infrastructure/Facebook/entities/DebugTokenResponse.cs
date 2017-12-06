using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook.Entities
{
  public class Data
  {
    [JsonProperty(PropertyName = "app_id")]
    public string AppId { get; set; }
    [JsonProperty(PropertyName = "type")]
    public string Type { get; set; }
    [JsonProperty(PropertyName = "application")]
    public string Application { get; set; }
    [JsonProperty(PropertyName = "expires_at")]
    public int ExpiresAt { get; set; }
    [JsonProperty(PropertyName = "is_valid")]
    public bool IsValid { get; set; }
    [JsonProperty(PropertyName = "issued_at")]
    public int IssuedAt { get; set; }
    [JsonProperty(PropertyName = "scopes")]
    public List<string> Scopes { get; set; }
    [JsonProperty(PropertyName = "user_id")]
    public string UserId { get; set; }
  }

  public class DebugTokenResponse
  {
    [JsonProperty(PropertyName = "data")]
    public Data Data { get; set; }
  }
}
