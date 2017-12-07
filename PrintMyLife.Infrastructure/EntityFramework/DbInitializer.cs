using Microsoft.EntityFrameworkCore;

namespace PrintMyLife.Infrastructure.EntityFramework
{
  /// <summary>
  /// Based on Microsft Documentation (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro)
  /// </summary>
  public static class DbInitializer
  {
    public static void Initialize(AppDbContext context)
    {
      context.Database.Migrate();
      context.SaveChanges();
    }
  }
}
