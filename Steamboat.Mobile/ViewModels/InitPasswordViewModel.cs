using System;
using System.Windows.Input;
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

        public ICommand ToggleCommand { get; set; }
        public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public ValidatableObject<string> Confirm { set { SetPropertyValue(ref _confirm, value); } get { return _confirm; } }
        public bool ButtonEnabled { set { SetPropertyValue(ref _buttonEnabled, value); } get { return _buttonEnabled; } }

        #endregion

        public InitPasswordViewModel()
        {
            ToggleCommand = new Command(() => this.Toggle());
            ButtonEnabled = false;

            Password = new ValidatableObject<string>();
            Confirm = new ValidatableObject<string>();
        }

        private void Toggle()
        {
            ButtonEnabled = !ButtonEnabled;
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
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter a password" });
            _confirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter a password" });
        }

        #endregion
    }
}
