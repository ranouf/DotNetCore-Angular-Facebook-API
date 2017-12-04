using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Common.Entities
{
  public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
  {
    protected Entity() { }

    public virtual TPrimaryKey Id { get; set; }
  }

  public abstract class Entity : Entity<Guid>
  {
    protected Entity() { }
  }
}
