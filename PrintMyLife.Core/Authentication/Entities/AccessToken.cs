using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class AccessToken
  {
    public string Token { get; set; }
    public string TokenType { get; set; }
    public double ExpiresIn { get; set; }
  }
}
