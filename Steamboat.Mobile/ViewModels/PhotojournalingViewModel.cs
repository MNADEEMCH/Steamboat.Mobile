using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.ViewModels.Modals;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PhotojournalingViewModel : ViewModelBase
    {
        #region Properties

        public ICommand OpenCameraCommand { get; set; }
        public ICommand MoreInfoCommand { get; set; }

        #endregion

        public PhotojournalingViewModel()
        {
            OpenCameraCommand = new Command(async () => await OpenCamera());
            MoreInfoCommand = new Command(async () => await MoreInfo());
        }

        public async override Task InitializeAsync(object parameter)
        {
            await base.InitializeAsync(parameter);
        }

        private async Task OpenCamera()
        {
            await NavigationService.NavigateToAsync<CameraViewModel>(animate:false);
        }

        private async Task MoreInfo()
        {
            await ModalService.PushAsync<PhotojournalingMoreInfoModalViewModel>();
        }
    }
}
