using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Sample.Entities;
using PrintMyLife.Core.Social.Entities;

namespace PrintMyLife.Infrastructure.EntityFramework
{
  public class AppDbContext : IdentityDbContext<User, Role, Guid>
  {
    public DbSet<MySample> MySamples { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
  }
}
