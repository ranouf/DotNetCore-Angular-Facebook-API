using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Social.Entities;

namespace PrintMyLife.Infrastructure.EntityFramework
{
  public class AppDbContext : IdentityDbContext<User, Role, Guid>
  {
    public DbSet<Account> Accounts { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<UserAccount>()
          .HasKey(bc => new { bc.UserId, bc.AccountId });

      builder.Entity<UserAccount>()
          .HasOne(bc => bc.Account)
          .WithMany(b => b.Users)
          .HasForeignKey(bc => bc.AccountId);

      builder.Entity<UserAccount>()
          .HasOne(bc => bc.User)
          .WithMany(c => c.Accounts)
          .HasForeignKey(bc => bc.UserId);

    }
  }
}
