using AutoMapper;
using PrintMyLife.Core.Social.Entities;
using PrintMyLife.Infrastructure.Facebook.Entities;
using System.Collections.Generic;

namespace PrintMyLife.Infrastructure.Facebook.Converters
{
  internal class AccountsConverter : ITypeConverter<AccountsResponse, IEnumerable<Account>>
  {
    public IEnumerable<Account> Convert(AccountsResponse source, IEnumerable<Account> destination, ResolutionContext context)
    {
      var result = new List<Account>();
      foreach (var datum in source.Data)
      {
        result.Add(Mapper.Map<Datum, Account>(datum));
      }
      return result;
    }
  }
}
