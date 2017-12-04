using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrintMyLife.Core.Common.Entities;
using PrintMyLife.Core.Sample.Entities;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class User : IdentityUser<Guid>, IEntity
  {
    [Required]
    public string Firstname { get; set; }
    [Required]
    public string Lastname { get; set; }
    public virtual ICollection<MySample> Samples { get; set; } = new List<MySample>();

    internal User() { }

    public User(string email, string firstname, string lastname)
    {
      UserName = email;
      Email = email;
      Firstname = firstname;
      Lastname = lastname;
    }
  }
}
