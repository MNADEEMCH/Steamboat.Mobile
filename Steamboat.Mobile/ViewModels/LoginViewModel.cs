using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Account;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Properties

        private IAccountManager _accountManager;
        private string username;
        private string password;
        private string loginResult;

        public ICommand LoginCommand { get; set; }
        public string Username { set { SetPropertyValue(ref username, value); } get { return username; } }
        public string Password { set { SetPropertyValue(ref password, value); } get { return password; } }
        public string LoginResult { set { SetPropertyValue(ref loginResult, value); } get { return loginResult; } }

        #endregion

        public LoginViewModel(IAccountManager accountManager)
        {
            _accountManager = accountManager;

            LoginCommand = new Command(async () => await this.Login());

            LoginResult = "Try to login...";
        }

        private async Task Login()
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var result = await _accountManager.Login(username, password);
                if (result.AuthenticatedAccount == null)
                {
                    LoginResult = "Error";
                }
                else
                {
                    LoginResult = result.AuthenticatedAccount.FirstName + " " + result.AuthenticatedAccount.LastName;
                }
            }
            else {
                LoginResult = "Username and password can't be null";
            }
        }
    }
}
