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
        private bool _isUploading;
        private bool _isUploaded;
        private bool _isGoodSelected;
        private bool _isNetrualSelected;
        private bool _isBadSelected;
        private string _comment;

        public ImageSource ImageSource { set { SetPropertyValue(ref _imageSource, value); } get { return _imageSource; } }
        public ICommand SubmitCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand ImageFinishedCommand { get; set; }
        public ICommand GoodSelectedCommand { get; set; }
        public ICommand NeutralSelectedCommand { get; set; }
        public ICommand BadSelectedCommand { get; set; }
        public ICommand PlayLoopCommand { get; set; }
        public ICommand EndLoopCommand { get; set; }
        public ICommand AnimatePhotoCommand { get; set; }
        public bool IsUploading { set { SetPropertyValue(ref _isUploading, value); } get { return _isUploading; } }
        public bool IsUploaded { set { SetPropertyValue(ref _isUploaded, value); } get { return _isUploaded; } }
        public bool IsGoodSelected { set { SetPropertyValue(ref _isGoodSelected, value); } get { return _isGoodSelected; } }
        public bool IsNeutralSelected { set { SetPropertyValue(ref _isNetrualSelected, value); } get { return _isNetrualSelected; } }
        public bool IsBadSelected { set { SetPropertyValue(ref _isBadSelected, value); } get { return _isBadSelected; } }
        public string Comment { set { SetPropertyValue(ref _comment, value); } get { return _comment; } }

        #endregion

        public PhotoReviewViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
            IsUploading = false;
            IsUploaded = false;
            IsGoodSelected = IsNeutralSelected = IsBadSelected = true;

            SubmitCommand = new Command(async () => await SubmitPhoto());
            GoBackCommand = new Command(async () => await GoBack());
            ImageFinishedCommand = new Command(() => ImageFinished());
            GoodSelectedCommand = new Command(() => GoodSelected());
            NeutralSelectedCommand = new Command(() => NeutralSelected());
            BadSelectedCommand = new Command(() => BadSelected());
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
            IsUploading = true;
            PlayLoopCommand.Execute(null);
            await TryExecute(async () =>
            {
                var selectedEmoji = GetSelectedEmoji();
                var imageResponse = await _participantManager.UploadPhoto(_imageArray, Comment, selectedEmoji);
                AnimatePhotoCommand.Execute(null);
                IsUploaded = true;
                EndLoopCommand.Execute(null);
                await Task.Delay(1500);
            }, null, async () => await NavigationService.NavigateToAsync<PhotojournalingViewModel>(new PhotoUploadedParameter(), mainPage: true));
        }

        private async Task GoBack()
        {
            await TryExecute(async () =>
            {
                await NavigationService.PopAsync(1, animate: false);
            });
        }

        private void ImageFinished()
        {
            IsLoading = false;
        }

        private void GoodSelected()
        {
            IsGoodSelected = true;
            IsBadSelected = IsNeutralSelected = false;
        }

        private void NeutralSelected()
        {
            IsNeutralSelected = true;
            IsBadSelected = IsGoodSelected = false;
        }

        private void BadSelected()
        {
            IsBadSelected = true;
            IsGoodSelected = IsNeutralSelected = false;
        }

        private string GetSelectedEmoji()
        {
            var ret = string.Empty;
            if (IsGoodSelected && !IsNeutralSelected)
                ret = "Happy";
            else if (IsNeutralSelected && !IsGoodSelected)
                ret = "Meh";
            else if (IsBadSelected && !IsGoodSelected)
                ret = "Sad";

            return ret;
        }
    }
}
