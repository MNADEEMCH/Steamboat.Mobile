using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
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
                var devicePlatform = CrossDeviceInfo.Current.Platform.ToString();
                var deviceModel = CrossDeviceInfo.Current.Model;
                var deviceLocalID = CrossDeviceInfo.Current.Id;

                var account = await _accountService.AccountLogin(new AccountLogin()
                {
                    EmailAddress = username,
                    Password = password,
                    DevicePlatform = devicePlatform,
                    DeviceModel = deviceModel,
                    DeviceLocalID = deviceLocalID
                });

                if (account != null)
                {
                    var user = App.CurrentUser == null ?
                              await _userRepository.AddUser(username) : await _userRepository.UpdateUser(App.CurrentUser.Id, App.CurrentUser.Email);

                    App.CurrentUser = user;
                    App.SessionID = account.AuthenticatedAccount.Session;
                }

                return account;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<CurrentUser> GetLocalUser()
        {
            try
            {
                return await _userRepository.GetCurrentUser();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Logout()
        {
            var res = await _accountService.AccountLogout(App.SessionID);
            App.SessionID = null;

            return res != null;
        }

        public async Task<AccountLogin> InitPassword(string password, string confirm)
        {
            try
            {
                var initResponse = await _accountService.AccountInitPassword(new AccountInitPassword()
                {
                    Password = password,
                    RetypePassword = confirm
                });

                return initResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }
    }
}
