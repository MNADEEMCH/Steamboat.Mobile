using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
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

            SaveCommand = new Command(async (param) => await SendPhoto(param));
            GoBackCommand = new Command(async () => await GoBack());
        }

        private async Task SendPhoto(object param)
        {
            await TryExecute(async () =>
            {
                await NavigationService.NavigateToAsync<PhotoReviewViewModel>(param, animate: false);
            });
        }

        private async Task GoBack()
        {
            await NavigationService.NavigateToAsync<PhotojournalingViewModel>(mainPage: true, animate: false);
        }

    }
}
