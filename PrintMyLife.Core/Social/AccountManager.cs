using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Common.Dependencies;
using PrintMyLife.Core.Common.Repositories;
using PrintMyLife.Core.Common.UnitOWork;
using PrintMyLife.Core.Social.Entities;
using System.Threading.Tasks;

namespace PrintMyLife.Core.Social
{
  public class AccountManager : IManager
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Account, string> _accountRepository;
    private readonly IRepository<UserAccount> _userAccountRepository;

    public AccountManager(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
      _accountRepository = unitOfWork.GetRepository<Account, string>();
      _userAccountRepository = unitOfWork.GetRepository<UserAccount>();
    }

    public async Task<Account> AddAccountAsync(Account account, User user)
    {
      account = await _accountRepository.InsertAsync(account);
      await _userAccountRepository.InsertAsync(new UserAccount(user, account));

      await _unitOfWork.SaveChangesAsync();
      return account;
    }
  }
}
