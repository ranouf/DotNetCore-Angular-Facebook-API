using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrintMyLife.Core.Common.Entities;
using PrintMyLife.Core.Sample.Entities;
using PrintMyLife.Core.Social.Entities;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class User : IdentityUser<Guid>, IEntity
  {
    [Required]
    public string Firstname { get; set; }
    [Required]
    public string Lastname { get; set; }
    public virtual ICollection<MySample> Samples { get; } = new List<MySample>();
    public virtual ICollection<UserAccount> Accounts { get; } = new List<UserAccount>();

    internal User() { }

    public User(string username, string email, string firstname, string lastname)
    {
      UserName = username;
      Email = email;
      Firstname = firstname;
      Lastname = lastname;
      SecurityStamp = Guid.NewGuid().ToString();
    }
  }
}
