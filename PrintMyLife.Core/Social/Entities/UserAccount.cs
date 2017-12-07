using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Common.Entities;
using System;

namespace PrintMyLife.Core.Social.Entities
{
  public class UserAccount : Entity
  {
    public Guid UserId { get; internal set; }
    public virtual User User { get; internal set; }
    public string AccountId { get; internal set; }
    public virtual Account Account { get; internal set; }

    public UserAccount(User user, Account account)
    {
      UserId = user.Id;
      AccountId = account.Id;
    }
  }
}
