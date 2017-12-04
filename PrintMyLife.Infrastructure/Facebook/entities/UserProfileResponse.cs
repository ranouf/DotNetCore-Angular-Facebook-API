using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook.entities
{
  public class Cover
  {
    [JsonProperty(PropertyName = "id")]
    public string id { get; set; }
    [JsonProperty(PropertyName = "offset_x")]
    public int OffsetX { get; set; }
    [JsonProperty(PropertyName = "offset_y")]
    public int OffsetY { get; set; }
    [JsonProperty(PropertyName = "source")]
    public string Source { get; set; }
  }

  public class UserProfileResponse
  {
    [JsonProperty(PropertyName = "first_name")]
    public string FirstName { get; set; }
    [JsonProperty(PropertyName = "last_name")]
    public string LastName { get; set; }
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; }
    [JsonProperty(PropertyName = "installed")]
    public bool Installed { get; set; }
    [JsonProperty(PropertyName = "cover")]
    public Cover Cover { get; set; }
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
  }
}
