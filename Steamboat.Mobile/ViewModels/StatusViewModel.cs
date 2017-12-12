using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Account;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class StatusViewModel : ViewModelBase
    {
        #region Properties

        private IAccountManager _accountManager;
        //private string username;
        //private string password;
        //private string loginResult;

        public ICommand LogoutCommand { get; set; }
        //public string Username { set { SetPropertyValue(ref username, value); } get { return username; } }
        //public string Password { set { SetPropertyValue(ref password, value); } get { return password; } }
        //public string LoginResult { set { SetPropertyValue(ref loginResult, value); } get { return loginResult; } }

        #endregion

        public StatusViewModel(IAccountManager accountManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();

            LogoutCommand = new Command(async () => await this.Logout());
        }

        private async Task Logout()
        {
            IsLoading = true;

            try
            {
                var res = await _accountManager.Logout();
                if (res)
                {
                    await NavigationService.NavigateToAsync<LoginViewModel>();
                }
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
               IsLoading = false;
            }
        }
    }
}
