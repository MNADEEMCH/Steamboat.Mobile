using Steamboat.Mobile.Models.Modal;
using System.Threading.Tasks;


namespace Steamboat.Mobile.ViewModels.Modals
{
    public class DispositionMoreInfoModalViewModel:ModalViewModelBase
    {
        #region Properties
        private string _iconSource;
        public string IconSource
        {
            get { return _iconSource; }
            set { SetPropertyValue(ref _iconSource, value); }
        }

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
        #endregion

        public DispositionMoreInfoModalViewModel():base()
        {
            
        }

        public override Task InitializeAsync(object parameter)
        {
            ModalParam modalParam = parameter as ModalParam;

            if (modalParam != null)
            {
                IconSource = modalParam.IconSource;
                Title = modalParam.Title;
                Message = modalParam.Message;
            }
            IsLoading = false;
            return base.InitializeAsync(parameter);
        }
    }
}
