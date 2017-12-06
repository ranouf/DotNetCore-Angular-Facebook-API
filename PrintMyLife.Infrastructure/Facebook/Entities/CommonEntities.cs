using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook.Entities
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
}
