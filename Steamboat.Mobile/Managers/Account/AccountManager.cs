using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.DeviceInfo;
using Steamboat.Mobile.Models.Account;
using Steamboat.Mobile.Models.User;
using Steamboat.Mobile.Repositories.User;
using Steamboat.Mobile.Services.Account;
using System.Linq;

namespace Steamboat.Mobile.Managers.Account
{
    public class AccountManager : IAccountManager
    {
        private IAccountService _accountService;
        private IUserRepository _userRepository;
        private IUserAlertRepository _userAlertRepository;

        public AccountManager(IAccountService accountService = null,
                              IUserRepository userRepository = null,
                              IUserAlertRepository userAlertRepository = null)
        {
            _accountService = accountService ?? DependencyContainer.Resolve<IAccountService>();
            _userRepository = userRepository ?? DependencyContainer.Resolve<IUserRepository>();
            _userAlertRepository = userAlertRepository ?? DependencyContainer.Resolve<IUserAlertRepository>();
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
                    account.AvatarUrl = ResolveUrl(account.AvatarUrl);
                    var user = App.CurrentUser == null ?
                                  await _userRepository.AddUser(username, account.AvatarUrl) : await _userRepository.UpdateUser(App.CurrentUser.Id, username, account.AvatarUrl);

                    App.CurrentUser = user;
                    App.SessionID = account.Session;
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
                }, App.SessionID);

                return initResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<int> AddUserAlert(string username, int alertId)
        {
            try
            {
                return await _userAlertRepository.AddUserAlert(new UserAlert { UserName = username, AlertId = alertId });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }
        }

        public async Task<UserAlerts> GetUserAlerts(string username)
        {
            UserAlerts userAlerts = null;

            try
            {
                var userAlertItems = await _userAlertRepository.GetUserAlert(username);
                if (userAlertItems != null)
                    userAlerts = userAlertItems.GroupBy(x => x.UserName)
                                           .Select(g => new UserAlerts()
                                           {
                                               AlertsIds = g.Select(x => x.AlertId).ToList(),
                                               UserName = g.Key as string
                                           }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in login: {ex}");
                throw ex;
            }

            return userAlerts;
        }

        private string ResolveUrl(string avatarUrl)
        {
            //TODO: Get BaseURL from config
            var apiUrlBase = "https://dev.momentumhealth.co/";
            return avatarUrl.Replace("~/", apiUrlBase);
        }
    }
}
