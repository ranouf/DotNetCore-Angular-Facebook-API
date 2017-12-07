using Microsoft.AspNetCore.Identity;
using PrintMyLife.Common.Entities;
using System;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class Role : IdentityRole<Guid>, IEntity
  {
  }
}
