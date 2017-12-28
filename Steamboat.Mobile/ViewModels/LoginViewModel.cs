using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Exceptions;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Services.Navigation;
using Steamboat.Mobile.Validations;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        private IAccountManager _accountManager;
        private ValidatableObject<string> _username;
        private ValidatableObject<string> _password;
        private bool _isBusy;

        public ICommand LoginCommand { get; set; }
        public ValidatableObject<string> Username { set { SetPropertyValue(ref _username, value); } get { return _username; } }
        public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }

        #endregion

        public LoginViewModel(IAccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();

            LoginCommand = new Command(async () => await this.Login());
            IsBusy = false;

            _username = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
        }

        public async override Task InitializeAsync(object parameter)
        {
            Username.Value = await GetCurrentUser();

            await base.InitializeAsync(parameter);
        }

        private async Task Login()
        {
            IsBusy = true;
            bool isValid = Validate();

            if(isValid)
            {
                try
                {
                    var result = await _accountManager.Login(_username.Value, _password.Value);

                    if(IsPasswordExpired(result.AuthenticatedAccount.IsPasswordExpired))
                    {
                        await NavigationService.NavigateToAsync<InitPasswordViewModel>();
                    }
                    else
                    {
                        await NavigationService.NavigateToAsync<StatusViewModel>(mainPage:true);
                    }

                    Password.Value = String.Empty;
                }
                catch(Exception e)
                {
                    await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
                IsBusy = false;
        }

        private async Task<string> GetCurrentUser()
        {
            var user = await _accountManager.GetLocalUser();

            if (user != null)
            {
                App.CurrentUser = user;
                return user.Email;
            }
            else
                return string.Empty;
        }

        private bool IsPasswordExpired(string passwordExpired)
        {
            bool res;
            bool.TryParse(passwordExpired, out res);
            return res;
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
