using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Account;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Validations;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class InitPasswordViewModel : ViewModelBase
    {
        #region Properties

        private ValidatableObject<string> _password;
        private ValidatableObject<string> _confirm;
        private bool _buttonEnabled;
        private bool _isBusy;
        private IAccountManager _accountManager;
        private IParticipantManager _participantManager;

        public ICommand ValidatePasswordFocusCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public ValidatableObject<string> Confirm { set { SetPropertyValue(ref _confirm, value); } get { return _confirm; } }
        public bool ButtonEnabled { set { SetPropertyValue(ref _buttonEnabled, value); } get { return _buttonEnabled; } }
        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }

        #endregion

        public InitPasswordViewModel(IAccountManager accountManager = null, IParticipantManager participantManager = null)
        {
            _accountManager = accountManager ?? DependencyContainer.Resolve<IAccountManager>();
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();

            ValidatePasswordFocusCommand = new Command(() => this.ValidatePasswordFocus());
            UpdateCommand = new Command(async () => await this.Update());
        }

        public async override Task InitializeAsync(object parameter)
        {
            ButtonEnabled = false;
            IsBusy = false;
            Password = new ValidatableObject<string>();
            Confirm = new ValidatableObject<string>();
            AddValidations();

            await base.InitializeAsync(parameter);
        }

        private async Task Update()
        {
            bool isValid = Validate();

            try
            {
                ValidatePasswordAndConfirm();
                var initPassword = await _accountManager.InitPassword(Password.Value, Confirm.Value);
                await _accountManager.Login(initPassword.EmailAddress, initPassword.Password);

                var status = await _participantManager.GetStatus();
                var viewModelType = DashboardStatusHelper.GetViewModelForStatus(status.Dashboard.NextStepContent);
                await NavigationService.NavigateToAsync(viewModelType);

            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ValidatePasswordAndConfirm()
        {
            if(!Password.Value.Equals(Confirm.Value))
            {
                throw new Exception("The passwords did not match. Please try again.");
            }
        }

        private void ValidatePasswordFocus()
        {
            if(!string.IsNullOrEmpty(Password.Value) && !string.IsNullOrEmpty(Confirm.Value))
            {
                ButtonEnabled = true;
            }
            else
            {
                ButtonEnabled = false;
            }
        }

        #region Validations

        private bool Validate()
        {            
            bool isValidPassword = ValidatePassword();
            bool isValidConfirm = ValidateConfirm();

            return isValidConfirm && isValidPassword;
        }

        private bool ValidatePassword()
        {
            return Password.Validate();
        }

        private bool ValidateConfirm()
        {
            return Confirm.Validate();
        }

        private void AddValidations()
        {
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please enter a password" });
            Confirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please confirm your password" });
            Password.Validations.Add(new PasswordRule<string> { ValidationMessage = "Please check the password requirements" });
        }

        #endregion
    }
}
