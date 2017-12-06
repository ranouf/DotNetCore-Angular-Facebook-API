using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook.Entities
{
  public class Datum
  {
    [JsonProperty(PropertyName = "cover")]
    public Cover Cover { get; set; }
    [JsonProperty(PropertyName = "access_token")]
    public string AccessToken { get; set; }
    [JsonProperty(PropertyName = "category")]
    public string Category { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
  }

  public class Cursors
  {
    [JsonProperty(PropertyName = "before")]
    public string Before { get; set; }
    [JsonProperty(PropertyName = "after")]
    public string After { get; set; }
  }

  public class Paging
  {
    [JsonProperty(PropertyName = "cursors")]
    public Cursors Cursors { get; set; }
  }

  public class AccountsResponse
  {
    [JsonProperty(PropertyName = "data")]
    public List<Datum> Data { get; set; }
    [JsonProperty(PropertyName = "paging")]
    public Paging Paging { get; set; }
  }
}
