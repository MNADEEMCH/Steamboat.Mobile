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

        #endregion

        public CameraViewModel(IParticipantManager participantManager = null)
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();

            SaveCommand = new Command(async (imageArray) => await SendPhoto(imageArray));
        }

        private async Task SendPhoto(object imageArray)
        {
            var image = imageArray as byte[];

            var foo = await _participantManager.UploadPhoto(image);
            await DialogService.ShowAlertAsync($"GUID: {foo.Guid}", "YEAH", "OK");
        }
    }
}
