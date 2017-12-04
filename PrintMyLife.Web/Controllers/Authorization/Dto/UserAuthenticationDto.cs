using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrintMyLife.Web.Controllers.Authorization.Dto
{
    public class UserAuthenticationDto
    {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string Token { get; set; }
  }
}
