using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FFImageLoading;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Steamboat.Mobile.Helpers;
using Steamboat.Mobile.Managers.Participant;
using Steamboat.Mobile.Models.NavigationParameters;
using Steamboat.Mobile.Models.Participant.Photojournaling;
using Steamboat.Mobile.ViewModels.Modals;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels
{
    public class PhotojournalingViewModel : ViewModelBase
    {
        #region Properties

        private IParticipantManager _participantManager;
        private ObservableCollection<Photograph> _photoCollection;
        private string _photosTaken;
        private bool _showPhotos;
        private const int _photoGoal = 20;

        public ICommand OpenCameraCommand { get; set; }
        public ICommand MoreInfoCommand { get; set; }
        public ICommand OpenPhotoDetailCommand { get; set; }
        public ObservableCollection<Photograph> PhotoCollection { get { return _photoCollection; } set { SetPropertyValue(ref _photoCollection, value); } }
        public string PhotosTaken { get { return _photosTaken; } set { SetPropertyValue(ref _photosTaken, value); } }
        public bool ShowPhotos { get { return _showPhotos; } set { SetPropertyValue(ref _showPhotos, value); } }

        #endregion

        public PhotojournalingViewModel(IParticipantManager participantManager = null)
        {
            IsLoading = true;
            OpenCameraCommand = new Command(async () => await OpenCamera());
            MoreInfoCommand = new Command(async () => await MoreInfo());
            OpenPhotoDetailCommand = new Command(async (selectedPhoto) => await OpenPhoto(selectedPhoto));

            _participantManager = participantManager ?? DependencyContainer.Resolve<IParticipantManager>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            await TryExecute(async () =>
            {
                var photos = await _participantManager.GetPhotographs();
                PreparePhotos(ref photos);
                PhotoCollection = photos.ToObservableCollection();
                var photosCount = PhotoCollection.Count;
                PhotosTaken = string.Format("{0}/{1}", photosCount, _photoGoal.ToString());
                ShowPhotos = photosCount > 0;

                if (parameter is PhotoUploadedParameter && photosCount == _photoGoal)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ModalService.PushAsync<PhotojournalingGoalViewModel>();
                    });
                }
            }, null, () => IsLoading = false);
        }

        private async Task OpenCamera()
        {
            await TryExecute(async () =>
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Camera))
                        status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    await NavigationService.NavigateToAsync<CameraViewModel>(animate: false);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ModalService.PushAsync<CameraAccessModalViewModel>();
                    });
                }
            }, null, null);
        }

        private async Task MoreInfo()
        {
            await TryExecute(async () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ModalService.PushAsync<PhotojournalingMoreInfoModalViewModel>();
                });
            });
        }

        private void PreparePhotos(ref List<Photograph> photos)
        {
            foreach (var photo in photos)
            {
                photo.ReviewPending = photo.ReviewerComment == null && photo.ReviewerOpinionRatingName == null;
                photo.Acknowledged = photo.ReviewPending && photo.ReviewedTimestamp != DateTime.MinValue;
            }
        }

        private async Task OpenPhoto(object selectedPhoto)
        {
            await TryExecute(async () =>
            {
                await NavigationService.NavigateToAsync<PhotoDetailsViewModel>(selectedPhoto);
            });
        }
    }
}
