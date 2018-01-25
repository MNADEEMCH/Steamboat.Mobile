using Steamboat.Mobile.Models.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class WelcomeModalViewModel:ModalViewModelBase
    {
        #region Properties

        public ICommand CloseModalCommand { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetPropertyValue(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetPropertyValue(ref _message, value); }
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set { SetPropertyValue(ref _buttonText, value); }
        }
        #endregion

        public WelcomeModalViewModel()
        {
            IsLoading = true;
            CloseModalCommand = new Command(async () => await CloseModal());
        }

        public override Task InitializeAsync(object parameter)
        {
            ModalParam modalParam = parameter as ModalParam;

            if (modalParam != null)
            {
                Title = modalParam.Title;
                Message = modalParam.Message;
                ButtonText = modalParam.ButtonText.ToUpper();
            }
            IsLoading = false;
            return base.InitializeAsync(parameter);
        }

    }
}
