using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Core.Social.Entities
{
    public class UserAccount : Entity
  {
    public virtual Guid UserId { get; }
    public virtual string AccountId { get; }
    public virtual Account Account { get; }

    public UserAccount(User user, Account account)
    {
      UserId = user.Id;
      AccountId = account.Id;
    }
  }
}
