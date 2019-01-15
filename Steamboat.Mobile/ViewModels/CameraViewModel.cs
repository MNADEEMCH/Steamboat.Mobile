using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.Camera;
using Steamboat.Mobile.Models.NavigationParameters;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class CameraViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private bool _isFrontCameraAvailable;
        private bool _isFlashAvailable;
        private bool _isCameraBusy;
        private bool _enableFlash;

        public ICommand CameraReadyCommand { get; set; }
        public ICommand TakePictureCommand { get; set; }
        public ICommand CameraTakePictureCommand { get; set; }
        public ICommand PictureTakenCommand { get; set; }
        public ICommand ToggleCameraCommand { get; set; }
        public ICommand CameraToggleCameraCommand { get; set; }
        public ICommand ToggleFlashCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public bool IsFrontCameraAvailable { get { return _isFrontCameraAvailable; } set { SetPropertyValue(ref _isFrontCameraAvailable, value); } }
        public bool IsFlashAvailable { get { return _isFlashAvailable; } set { SetPropertyValue(ref _isFlashAvailable, value); } }
        public bool EnableFlash { get { return _enableFlash; } set { SetPropertyValue(ref _enableFlash, value); } }

        #endregion

        public CameraViewModel(IParticipantManager participantManager = null)
        {
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            _isCameraBusy = true;
            EnableFlash = true;

            CameraReadyCommand = new Command<CamCharacteristics>((param) => { CameraReady(param); });
            TakePictureCommand = new Command(() => {  TakePicture(); });
            PictureTakenCommand = new Command<PhotoTakenParameter>(async (param) => await PictureTaken(param));
            ToggleCameraCommand = new Command(ToggleCamera);
            ToggleFlashCommand = new Command(ToggleFlash);
            GoBackCommand = new Command(async () => await GoBack());
        }

        private void CameraReady(CamCharacteristics camCharacteristics)
        {
            IsFrontCameraAvailable = camCharacteristics.HasFrontCamera;
            IsFlashAvailable = camCharacteristics.IsFlashSupported;
            _isCameraBusy = false;
        }

        private void TakePicture()
        {
            if (!_isCameraBusy)
            {
                _isCameraBusy = true;
                CameraTakePictureCommand?.Execute(new PictureSettings()
                {
                    EnableFlash = this.EnableFlash
                });
            }
        }

        private async Task PictureTaken(PhotoTakenParameter pictureTakenParam)
        {
            await NavigationService.NavigateToAsync<PhotoReviewViewModel>(pictureTakenParam, animate: false);
        }

        private void ToggleCamera()
        {
            if (!_isCameraBusy)
            {
                _isCameraBusy = true;
                CameraToggleCameraCommand?.Execute(null);
            }
        }

        private void ToggleFlash()
        {
            EnableFlash = !EnableFlash;
        }

        private async Task GoBack()
        {
            //await NavigationService.NavigateToAsync<PhotojournalingViewModel>(mainPage: true, animate: false);
            await NavigationService.PopAsync(animate: false);
        }

    }
}
