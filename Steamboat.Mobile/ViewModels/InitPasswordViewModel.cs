using System;
using System.Windows.Input;
using Steamboat.Mobile.Validations;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class InitPasswordViewModel : ViewModelBase
    {
        #region Properties

        //private ValidatableObject<string> _password;
        private string _password;
        private bool _buttonEnabled;

        public ICommand ToggleCommand { get; set; }
        //public ValidatableObject<string> Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public string Password { set { SetPropertyValue(ref _password, value); } get { return _password; } }
        public bool ButtonEnabled { set { SetPropertyValue(ref _buttonEnabled, value); } get { return _buttonEnabled; } }

        #endregion

        public InitPasswordViewModel()
        {
            ToggleCommand = new Command(() => this.Toggle());
            ButtonEnabled = false;
        }

        private void Toggle()
        {
            ButtonEnabled = !ButtonEnabled;
        }
    }
}
