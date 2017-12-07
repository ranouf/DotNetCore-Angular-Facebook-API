using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Common.Configuration
{
  public class AuthenticationSettings
  {
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public int ExpirationDurationInDays { get; set; }
  }
}
