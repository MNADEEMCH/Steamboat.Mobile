using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Services.Account;

namespace Steamboat.Mobile.Managers.Account
{
    public class AccountManager : IAccountManager
    {
        private IAccountService _accountService;

        public AccountManager(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<AccountInfo> Login(string username, string password)
        {
            return await _accountService.AccountLogin(new AccountLogin(){
                EmailAddress = username,
                Password = password
            });
        }
    }
}
