using PrintMyLife.Common.Entities;
using System.Collections.Generic;

namespace PrintMyLife.Core.Social.Entities
{
  public class Account : Entity<string>
  {
    public string Name { get; set; }
    public string Category { get; set; } // TODO: Create Category Class
    public string AccessToken { get; set; }
    public string CoverUrl { get; set; }
    public virtual ICollection<UserAccount> Users { get; } = new List<UserAccount>();
  }
}
