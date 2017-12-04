using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using PrintMyLife.Core.Common.Entities;

namespace PrintMyLife.Core.Authentication.Entities
{
  public class Role : IdentityRole<Guid>, IEntity
  {
  }
}
