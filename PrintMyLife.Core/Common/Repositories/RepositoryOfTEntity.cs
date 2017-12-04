using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PrintMyLife.Core.Common.Entities;

namespace PrintMyLife.Core.Common.Repositories
{
  public class Repository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
      where TEntity : class, IEntity<Guid>
  {
    public Repository(DbContext dbContext)
        : base(dbContext)
    {
    }
  }
}
