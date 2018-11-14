using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Participant;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class CameraViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;

        public ICommand SaveCommand { get; set; }
        public ICommand GoBackCommand { get; set; }

        #endregion

        public CameraViewModel(IParticipantManager participantManager = null)
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();

            SaveCommand = new Command(async (imageArray) => await SendPhoto(imageArray));
            GoBackCommand = new Command(async () => await GoBack());
        }

        private async Task SendPhoto(object imageArray)
        {
            await TryExecute(async () =>
            {
                var image = imageArray as byte[];

                var foo = await _participantManager.UploadPhoto(image);
                await DialogService.ShowAlertAsync($"GUID: {foo.Guid}", "YEAH", "OK");
            });
        }

        private async Task GoBack()
        {
            await NavigationService.NavigateToAsync<PhotojournalingViewModel>(mainPage: true);
        }

    }
}
