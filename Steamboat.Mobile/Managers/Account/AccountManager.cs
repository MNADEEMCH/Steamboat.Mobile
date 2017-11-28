using System;
using System.Threading.Tasks;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.User;
using Steamboat.Mobile.Services.Account;

namespace Steamboat.Mobile.Managers.Account
{
    public class AccountManager : IAccountManager
    {
        private IAccountService _accountService;
        private IUserRepository _userRepository;

        public AccountManager(IAccountService accountService = null, IUserRepository userRepository = null)
        {
            _accountService = accountService ?? DependencyContainer.Resolve<IAccountService>();
            _userRepository = userRepository ?? DependencyContainer.Resolve<IUserRepository>();
        }

        public async Task<AccountInfo> Login(string username, string password)
        {
            try
            {
                var account = await _accountService.AccountLogin(new AccountLogin()
                {
                    EmailAddress = username,
                    Password = password,
                    DevicePlatform = "iOS",
                    DeviceModel = "iPhone 8",
                    DeviceLocalID = "123456789"
                });

                var user = App.CurrentUser == null ?
                              await _userRepository.AddUser(username) : await _userRepository.UpdateUser(App.CurrentUser.Id, App.CurrentUser.Email);
                
                App.CurrentUser = user;

                return account;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<CurrentUser> GetLocalUser()
        {
            try
            {
                return await _userRepository.GetCurrentUser();
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
