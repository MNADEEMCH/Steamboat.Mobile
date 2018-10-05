using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Helpers.Settings;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Application;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Validations;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
		#region Properties

		private IAccountManager _accountManager;
		private IParticipantManager _participantManager;
		private IApplicationManager _applicationManager;
		private ISettings _settings;
		private ValidatableObject<string> _username;
		private ValidatableObject<string> _password;
		private string _environmentInfoText;
		private bool _isBusy;

		public ICommand LoginCommand { get; set; }
		public ICommand RegisterHereCommand { get; set; }
		public ICommand ForgotPasswordCommand { get; set; }
		public ValidatableObject<string> Username { set { SetPropertyValue(ref _username, value); } get { return _username; } }
		public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
		public string EnvironmentInfoText { set { SetPropertyValue(ref _environmentInfoText, value); } get { return _environmentInfoText; } }
		public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }

		#endregion

		public LoginViewModel(IAccountManager accountManager = null,
							  IParticipantManager participantManager = null,
							  IApplicationManager applicationManager = null,
							  ISettings settings = null)
		{
			_accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
			_participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
			_applicationManager = applicationManager ?? DependencyContainer.Resolve<IApplicationManager>();
			_settings = settings ?? DependencyContainer.Resolve<ISettings>();

			LoginCommand = new Command(async () => await this.Login());
			var apiUrl = _settings.BaseUrl;
			RegisterHereCommand = new Command(() => this.OpenUrl($"{apiUrl}register"));
			ForgotPasswordCommand = new Command(() => this.OpenUrl($"{apiUrl}account/resetpassword"));
			IsBusy = false;

			Username = new ValidatableObject<string>();
			Password = new ValidatableObject<string>();
            Password.Value = "Passw0rd";

			if (App.CurrentUser != null)
				Username.Value = App.CurrentUser.Email ?? string.Empty;

			AddValidations();
			AddEnvironmentInfo();
		}

		private void OpenUrl(string url)
		{
			Device.OpenUri(new Uri(url));
		}

		public async override Task InitializeAsync(object parameter)
		{
			if (parameter is Models.Application.Logout)
			{
				await Logout(parameter as Models.Application.Logout);
			}
			else
			{
				Username.Value = await GetCurrentUser();
			}
		}

		private async Task Logout(Models.Application.Logout logout)
		{
			await TryExecute(async () =>
			{
				_applicationManager.ResetTimer();
				await _accountManager.Logout(logout.CallBackend);
			});
		}

		private async Task Login()
		{
			IsBusy = true;
			bool isValid = Validate();

			if (isValid)
			{
				await TryExecute(async () =>
				{
					var result = await _accountManager.Login(_username.Value, _password.Value);
					_applicationManager.StartTimer();

					if (result.IsPasswordExpired)
					{
						await NavigationService.NavigateToAsync<InitPasswordViewModel>(result.AreConsentsAccepted);
					}
					else if (!result.AreConsentsAccepted)
					{
						await NavigationService.NavigateToAsync<ConsentsViewModel>();
					}
					else
					{
						_applicationManager.TrySendToken();
						Device.BeginInvokeOnMainThread(() => _applicationManager.UpdateNotificationBadge(1));
						var status = await _participantManager.GetStatus();
						Device.BeginInvokeOnMainThread(async () => await NavigationService.NavigateToAsync<MainViewModel>(status, mainPage: true));

					}
				}, null, () => IsBusy = false);
			}
			else
				IsBusy = false;
		}

		private async Task<string> GetCurrentUser()
		{
			var userEmail = string.Empty;
			await TryExecute(async () =>
			{
				var user = await _accountManager.GetLocalUser();

				if (user != null)
				{
					App.CurrentUser = user;
					userEmail = user.Email;
				}
			});

			return userEmail;
		}

		private void AddEnvironmentInfo()
		{

#if DEBUG
			EnvironmentInfoText = $"Debug Version\r\n{_settings.BaseUrl}";
#elif RELEASE
			EnvironmentInfoText = $"Release Version {_settings.BaseUrl}";
#else
			EnvironmentInfoText = string.Empty;
#endif

		}

#region Validations

		private bool Validate()
		{
			bool isValidUser = ValidateUserName();
			bool isValidPassword = ValidatePassword();

			return isValidUser && isValidPassword;
		}

		private bool ValidateUserName()
		{
			return _username.Validate();
		}

		private bool ValidatePassword()
		{
			return _password.Validate();
		}

		private void AddValidations()
		{
			_username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter a email" });
			_username.Validations.Add(new EmailRule<string> { ValidationMessage = "Enter a valid email" });
			_password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter a password" });
		}

#endregion
	}
}
