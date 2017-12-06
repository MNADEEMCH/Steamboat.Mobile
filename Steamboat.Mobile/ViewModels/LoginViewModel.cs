using System;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private string loginResult;

        public ICommand LoginCommand { get; set; }
        public ValidatableObject<string> Username { set { SetPropertyValue(ref _username, value); } get { return _username; } }
        public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public string LoginResult { set { SetPropertyValue(ref loginResult, value); } get { return loginResult; } }

        #endregion

        public LoginViewModel(IAccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();

            LoginCommand = new Command(async () => await this.Login());
            LoginResult = "Try to login...";

            _username = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _username.Value = Task.Run(() => GetCurrentUser()).Result;

            AddValidations();
        }

        private async Task Login()
        {            
            bool isValid = Validate();

            if(isValid)
            {
                var result = await _accountManager.Login(_username.Value, _password.Value);
                if (result.AuthenticatedAccount == null)
                {
                    LoginResult = "Error";
                }
                else
                {
                    LoginResult = result.AuthenticatedAccount.FirstName + " " + result.AuthenticatedAccount.LastName;
                    await NavigationService.NavigateToAsync<StatusViewModel>();
                }
            }
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
