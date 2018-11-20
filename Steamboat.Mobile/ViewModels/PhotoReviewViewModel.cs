using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PhotoReviewViewModel : ViewModelBase
    {
        #region Properties

        private ImageSource _imageSource;
        private IParticipantManager _participantManager;
        private byte[] _imageArray;
        private bool _isBusy;
        private bool _isUploaded;
        private string _resultLabel;

        public ImageSource ImageSource { set { SetPropertyValue(ref _imageSource, value); } get { return _imageSource; } }
        public ICommand SubmitCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ImageFinishedCommand { get; set; }
        public bool IsBusy { set { SetPropertyValue(ref _isBusy, value); } get { return _isBusy; } }
        public bool IsUploaded { set { SetPropertyValue(ref _isUploaded, value); } get { return _isUploaded; } }
        public string ResultLabel { set { SetPropertyValue(ref _resultLabel, value); } get { return _resultLabel; } }

        #endregion

        public PhotoReviewViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            IsBusy = false;
            IsUploaded = false;
            ResultLabel = "Uploading...";

            SubmitCommand = new Command(async () => await SubmitPhoto());
            GoBackCommand = new Command(async () => await GoBack());
            ImageFinishedCommand = new Command(() => ImageFinished());
        }

        public async override Task InitializeAsync(object parameter)
        {
            if (parameter is PhotoTakenParameter photoParam)
            {
                ImageSource = photoParam.ImageSource;
                _imageArray = photoParam.Media;
            }

            await base.InitializeAsync(parameter);
        }

        private async Task SubmitPhoto()
        {
            IsBusy = true;
            await TryExecute(async () =>
            {
                var imageResponse = await _participantManager.UploadPhoto(_imageArray);
                IsUploaded = true;
                ResultLabel = "Done!";
                await Task.Delay(1500);
            }, null, async () => await NavigationService.NavigateToAsync<PhotojournalingViewModel>(mainPage: true));
        }

        private async Task GoBack()
        {
            IsBusy = true;
            await TryExecute(async () =>
            {
                await NavigationService.PopAsync(1, animate:false);
            });
        }

        private void ImageFinished()
        {
            IsLoading = false;
        }

    }
}
