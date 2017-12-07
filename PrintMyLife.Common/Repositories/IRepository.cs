using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Common.Repositories
{
  /// <summary>
  /// This interface must be implemented by all repositories to identify them by convention.
  /// Implement generic version instead of this one.
  /// </summary>
  public interface IRepository
  {
    DbContext GetDbContext();
  }
}
