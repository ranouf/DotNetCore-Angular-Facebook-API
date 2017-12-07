using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Common.Dependencies;
using PrintMyLife.Common.Repositories;
using PrintMyLife.Common.UnitOWork;
using PrintMyLife.Core.Social.Entities;
using System.Threading.Tasks;
using PrintMyLife.Core.Authentication;
using Microsoft.AspNetCore.Identity;
using PrintMyLife.Common.Constants;
using System.Collections.Generic;

namespace PrintMyLife.Core.Social
{
  public class SocialManager : IManager
  {
    private readonly IExternalSocialService _externalSocialService;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Account, string> _accountRepository;
    private readonly IRepository<UserAccount> _userAccountRepository;

    public SocialManager(
      IExternalSocialService externalSocialService,
      UserManager<User> userManager,
      IUnitOfWork unitOfWork
    )
    {
      _externalSocialService = externalSocialService;
      _userManager = userManager;
      _unitOfWork = unitOfWork;
      _accountRepository = unitOfWork.GetRepository<Account, string>();
      _userAccountRepository = unitOfWork.GetRepository<UserAccount>();
    }

    public async Task<IEnumerable<Account>> LoadAccountsAsync(User user)
    {
      string userToken = await GetUserToken(user);

      var accounts = await _externalSocialService.GetAccountsAsync(user.UserName, userToken);
      var result = new List<Account>();
      foreach (var account in accounts)
      {
        var newAccount = await _accountRepository.InsertAsync(account);
        await _userAccountRepository.InsertAsync(new UserAccount(user, newAccount));
        result.Add(newAccount);
      }
      await _unitOfWork.SaveChangesAsync();

      return result;
    }

    public async Task<Account> LoadAccountAsync(Account account)
    {
      var x = await _externalSocialService.GetAccountAsync(account.Id, account.AccessToken);

      return account;
    }

    #region Private
    private async Task<string> GetUserToken(User user)
    {
      return await _userManager.GetAuthenticationTokenAsync(
        user,
        TokenConstants.FacebookProvider,
        TokenConstants.FacebookTokenName
      );
    }
    #endregion
  }
}
