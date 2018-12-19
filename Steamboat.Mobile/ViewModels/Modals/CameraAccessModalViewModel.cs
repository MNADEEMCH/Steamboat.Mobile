using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Helpers;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class CameraAccessModalViewModel : ModalViewModelBase
    {
        private IDeviceInfo _deviceInfo;
        public ICommand OpenSettingsCommand { get; set; }

        public CameraAccessModalViewModel(IDeviceInfo deviceInfo = null)
        {
            OpenSettingsCommand = new Command(async () => await OpenSettings());

            _deviceInfo = deviceInfo ?? DependencyContainer.Resolve<IDeviceInfo>();
        }

        public override Task InitializeAsync(object parameter)
        {
            IsLoading = false;
            return base.InitializeAsync(parameter);
        }

        private async Task OpenSettings()
        {
            try
            {
                _deviceInfo.OpenAppSettings();
                await ModalService.PopAsync();
            }
            catch (Exception ex)
            {
                await DialogService.ShowAlertAsync(ex.Message, "Error", "OK");
            }
        }
    }
}
