using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Authentication.Entities
{
    public class UserProfile
    {
    public string Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string CoverUrl { get; set; }
  }
}
