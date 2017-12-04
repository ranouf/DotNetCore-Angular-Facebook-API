using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Runtime.Session
{
  public interface IAppSession
  {
    Guid? UserId { get; }
  }
}
