using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrintMyLife.Core.Sample.Entities;

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
      // Look for any sample.
      if (context.MySamples.Any())
      {
        return;   // DB has been seeded
      }
      var mySamples = new MySample[]
      {
            new MySample("Sample 1"),
            new MySample("Sample 2"),
            new MySample("Sample 3"),
      };
      foreach (var mySample in mySamples)
      {
        context.MySamples.Add(mySample);
      }
      context.SaveChanges();
    }
  }
}
