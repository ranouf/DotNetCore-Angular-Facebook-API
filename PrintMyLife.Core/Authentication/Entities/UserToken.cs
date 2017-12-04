using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class UserToken
  {
    public string AppId { get; set; }
    public string Type { get; set; }
    public string Application { get; set; }
    public int ExpiresAt { get; set; }
    public bool IsValid { get; set; }
    public int IssuedAt { get; set; }
    public List<string> Scopes { get; set; }
    public string UserId { get; set; }
  }
}
