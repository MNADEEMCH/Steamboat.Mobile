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
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Services.Dialog;
using Xamarin.Forms;
using Steamboat.Mobile.Helpers.Settings;

namespace Steamboat.Mobile.Managers.Account
{
	public class AccountManager : ManagerBase, IAccountManager
	{
		private IAccountService _accountService;
		private IUserRepository _userRepository;
		private IUserAlertRepository _userAlertRepository;
		private ISettings _settings;

		public AccountManager(IAccountService accountService = null,
							  IUserRepository userRepository = null,
							  IUserAlertRepository userAlertRepository = null,
		                      ISettings settings = null)
		{
			_accountService = accountService ?? DependencyContainer.Resolve<IAccountService>();
			_userRepository = userRepository ?? DependencyContainer.Resolve<IUserRepository>();
			_userAlertRepository = userAlertRepository ?? DependencyContainer.Resolve<IUserAlertRepository>();
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();
		}

		public async Task<AccountInfo> Login(string username, string password)
		{
			return await TryExecute<AccountInfo>(async () =>
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

			});

		}

		public async Task<CurrentUser> GetLocalUser()
		{
			return await TryExecute<CurrentUser>(async () =>
			{
				return await _userRepository.GetCurrentUser();

			});
		}

		public async Task<bool> Logout(bool callBackend = true)
		{
			return await TryExecute<bool>(async () =>
			{
				var sessionId = App.SessionID;
				App.SessionID = null;
				if (callBackend)
				{
					var res = await _accountService.AccountLogout(sessionId);
					return res != null;
				}

				return true;
			});
		}

		public async Task<AccountLogin> InitPassword(string password, string confirm)
		{
			return await TryExecute<AccountLogin>(async () =>
			{
				var initResponse = await _accountService.AccountInitPassword(new AccountInitPassword()
				{
					Password = password,
					RetypePassword = confirm
				}, App.SessionID);

				return initResponse;
			});
		}

		public async Task<int> AddUserAlert(string username, int alertId)
		{
			return await TryExecute<int>(async () =>
			{
				return await _userAlertRepository.AddUserAlert(new UserAlert { UserName = username, AlertId = alertId });

			});
		}

		public async Task<UserAlerts> GetUserAlerts(string username)
		{
			return await TryExecute<UserAlerts>(async () =>
			{
				var userAlertItems = await _userAlertRepository.GetUserAlert(username);
				if (userAlertItems != null)
				{
					var userAlerts = userAlertItems.GroupBy(x => x.UserName)
										   .Select(g => new UserAlerts()
										   {
											   AlertsIds = g.Select(x => x.AlertId).ToList(),
											   UserName = g.Key as string
										   }).FirstOrDefault();
					return userAlerts;
				}

				return null;
			});

		}

		private string ResolveUrl(string avatarUrl)
		{
			return avatarUrl.Replace("~/", _settings.BaseUrl);
		}
	}
}
