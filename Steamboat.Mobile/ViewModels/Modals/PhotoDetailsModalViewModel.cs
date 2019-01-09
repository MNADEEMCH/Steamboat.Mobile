using System;
using System.Threading.Tasks;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class PhotoDetailsModalViewModel : ModalViewModelBase
    {
        #region Properties

        private string _url;

        public string Url { get { return _url; } set { SetPropertyValue(ref _url, value); } }

        #endregion

        public PhotoDetailsModalViewModel()
        {
        }

        public override Task InitializeAsync(object parameter)
        {
            if (parameter is string url)
            {
                Url = url;
            }

            IsLoading = false;

            return base.InitializeAsync(parameter);
        }
    }
}
