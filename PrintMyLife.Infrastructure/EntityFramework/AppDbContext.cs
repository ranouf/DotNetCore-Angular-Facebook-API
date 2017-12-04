using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Sample.Entities;

namespace PrintMyLife.Infrastructure.EntityFramework
{
  public class AppDbContext : IdentityDbContext<User, Role, Guid>
  {
    public DbSet<MySample> MySamples { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
