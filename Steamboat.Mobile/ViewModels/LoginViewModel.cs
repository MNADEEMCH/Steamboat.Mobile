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
        private bool isBusy;

        public ICommand LoginCommand { get; set; }
        public ValidatableObject<string> Username { set { SetPropertyValue(ref _username, value); } get { return _username; } }
        public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public bool IsBusy { set { SetPropertyValue(ref isBusy, value); } get { return isBusy; } }

        #endregion

        public LoginViewModel(IAccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();

            LoginCommand = new Command(async () => await this.Login());
            IsBusy = false;

            _username = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            Username.Value = Task.Run(() => GetCurrentUser()).Result;

            AddValidations();
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
                    await NavigationService.NavigateToAsync<StatusViewModel>();
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
